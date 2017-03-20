
using UnityEngine;
#if UNITY_EDITOR	
  using UnityEditor;
#endif
using System.Collections.Generic;
using System.IO;
using HeroLogic;
using HeroView;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Hero
{

/*
 	In this demo, we demonstrate:
	1.	Automatic asset bundle dependency resolving & loading.
		It shows how to use the manifest assetbundle like how to get the dependencies etc.
	2.	Automatic unloading of asset bundles (When an asset bundle or a dependency thereof is no longer needed, the asset bundle is unloaded)
	3.	Editor simulation. A bool defines if we load asset bundles from the project or are actually using asset bundles(doesn't work with assetbundle variants for now.)
		With this, you can player in editor mode without actually building the assetBundles.
	4.	Optional setup where to download all asset bundles
	5.	Build pipeline build postprocessor, integration so that building a player builds the asset bundles and puts them into the player data (Default implmenetation for loading assetbundles from disk on any platform)
	6.	Use WWW.LoadFromCacheOrDownload and feed 128 bit hash to it when downloading via web
		You can get the hash from the manifest assetbundle.
	7.	AssetBundle variants. A prioritized list of variants that should be used if the asset bundle with that variant exists, first variant in the list is the most preferred etc.
*/

// Loaded assetBundle contains the references count which can be used to unload dependent assetBundles automatically.
    public class LoadedAssetBundle
    {
        public AssetBundle m_AssetBundle;
        public int m_ReferencedCount;

        public LoadedAssetBundle(AssetBundle assetBundle)
        {
            m_AssetBundle = assetBundle;
            m_ReferencedCount = 1;
        }
    }

    public class BundlePathMap
    {
        public string BundleName;
        public bool IsInStreaming;
        public string SyncFilePath;
        public string ASyncFilePath;
    }

    public class BundleDependInfo
    {
        public string BundleName;
        public List<string> DependList = new List<string>();
    }

// Class takes care of loading assetBundle and its dependencies automatically, loading variants automatically.
    public class AssetBundleManager : MonoBehaviour
    {
//        public static string DataPathSync = "";
//        public static string DataPathASync = "";
//        public static string StreamingPathSync = "";
//        public static string StreamingPathASync = "";
        private static string[] m_Variants = {};
        private static AssetBundleManifest m_AssetBundleManifest;
        public static bool IsUnCompressLoad = true;
#if UNITY_EDITOR
        private static int m_SimulateAssetBundleInEditor = -1;
        private const string kSimulateAssetBundles = "SimulateAssetBundles";
#endif

        private static Dictionary<string, LoadedAssetBundle> m_LoadedAssetBundles =
            new Dictionary<string, LoadedAssetBundle>();

        private static Dictionary<string, WWW> m_DownloadingWWWs = new Dictionary<string, WWW>();
        private static Dictionary<string, string> m_DownloadingErrors = new Dictionary<string, string>();
        private static List<AssetBundleLoadOperation> m_InProgressOperations = new List<AssetBundleLoadOperation>();
        private static Dictionary<string, string[]> m_Dependencies = new Dictionary<string, string[]>();
        private List<string> m_KeysToRemove = new List<string>();

        private static List<string> m_UnloadBundles = new List<string>();

        private static MyDictionary<string, BundlePathMap> m_FilePathMap = new MyDictionary<string, BundlePathMap>();


        // Variants which is used to define the active variants.
        public static string[] Variants
        {
            get { return m_Variants; }
            set { m_Variants = value; }
        }

        // AssetBundleManifest object which can be used to load the dependecies and check suitable assetBundle variants.
        public static AssetBundleManifest AssetBundleManifestObject
        {
            set { m_AssetBundleManifest = value; }
            get { return m_AssetBundleManifest; }
        }

#if UNITY_EDITOR
        // Flag to indicate if we want to simulate assetBundles in Editor without building them actually.
        public static bool SimulateAssetBundleInEditor
        {
            get
            {
                if (m_SimulateAssetBundleInEditor == -1)
                    m_SimulateAssetBundleInEditor = EditorPrefs.GetBool(kSimulateAssetBundles, true) ? 1 : 0;

                return m_SimulateAssetBundleInEditor != 0;
            }
            set
            {
                int newValue = value ? 1 : 0;
                if (newValue != m_SimulateAssetBundleInEditor)
                {
                    m_SimulateAssetBundleInEditor = newValue;
                    EditorPrefs.SetBool(kSimulateAssetBundles, value);
                }
            }
        }
#endif

        // Get loaded AssetBundle, only return vaild object when all the dependencies are downloaded successfully.
        public static LoadedAssetBundle GetLoadedAssetBundle(string assetBundleName, out string error)
        {
            if (m_DownloadingErrors.TryGetValue(assetBundleName, out error))
                return null;

            LoadedAssetBundle bundle;
            m_LoadedAssetBundles.TryGetValue(assetBundleName, out bundle);
            if (bundle == null)
                return null;

            // No dependencies are recorded, only the bundle itself is required.
            string[] dependencies;
            if (!m_Dependencies.TryGetValue(assetBundleName, out dependencies))
                return bundle;
            // Make sure all dependencies are loaded
            int length = dependencies.Length;
            for (int i = 0; i < length; i++)
            {
                if (m_DownloadingErrors.TryGetValue(assetBundleName, out error))
                    return bundle;

                // Wait all the dependent assetBundles being loaded.
                LoadedAssetBundle dependentBundle;
                m_LoadedAssetBundles.TryGetValue(dependencies[i], out dependentBundle);
                if (dependentBundle == null)
                    return null;
            }

            return bundle;
        }

        // Load AssetBundleManifest.
        public static AssetBundleLoadManifestOperation Initialize(string manifestAssetBundleName)
        {
            var go = new GameObject("AssetBundleManager", typeof (AssetBundleManager));
            DontDestroyOnLoad(go);
//        BaseDownloadingURL = CBDelegateDefine.GetDataPath() + "/files/";
#if UNITY_EDITOR
            // If we're in Editor simulation mode, we don't need the manifest assetBundle.
            if (SimulateAssetBundleInEditor)
                return null;
#endif

            // LogSystem.LogWarning("Initialize: " + manifestAssetBundleName);
            LoadAssetBundle(manifestAssetBundleName, true);
            var operation = new AssetBundleLoadManifestOperation(manifestAssetBundleName, "AssetBundleManifest",
                typeof (AssetBundleManifest));
            m_InProgressOperations.Add(operation);
            return operation;
        }

        // Load AssetBundle and its dependencies.
        public static AssetBundle LoadAssetBundleFromFile(string assetBundleName,
            bool isLoadingAssetBundleManifest = false)
        {
#if UNITY_EDITOR
            // If we're in Editor simulation mode, we don't have to really load the assetBundle and its dependencies.
            if (SimulateAssetBundleInEditor)
                return null;
#endif
            // LogSystem.LogWarning("LoadAssetBundleFromFile: " + assetBundleName);
            if (!isLoadingAssetBundleManifest)
                assetBundleName = RemapVariantName(assetBundleName);

//        MonoBehaviour.print("LoadAssetBundleFromFile --------->>>> " + m_LoadedAssetBundles.Count);

            // load self
            bool bHasLoaded = LoadAssetBundleInternalFromFile(assetBundleName, isLoadingAssetBundleManifest);

            if (!bHasLoaded)
            {
                // Load dependencies.
                LoadDependencies(assetBundleName, LoadAssetBundleInternalFromFile);
            }

            if (m_LoadedAssetBundles.ContainsKey(assetBundleName))
            {
                return m_LoadedAssetBundles[assetBundleName].m_AssetBundle;
            }
            return null;
        }

        // Load AssetBundle and its dependencies.
        protected static void LoadAssetBundle(string assetBundleName, bool isLoadingAssetBundleManifest = false)
        {
#if UNITY_EDITOR
            // If we're in Editor simulation mode, we don't have to really load the assetBundle and its dependencies.
            if (SimulateAssetBundleInEditor)
                return;
#endif

            if (!isLoadingAssetBundleManifest)
                assetBundleName = RemapVariantName(assetBundleName);

            // Check if the assetBundle has already been processed.
            bool isAlreadyProcessed = LoadAssetBundleInternal(assetBundleName, isLoadingAssetBundleManifest);

            // Load dependencies.
            if (!isAlreadyProcessed && !isLoadingAssetBundleManifest)
                LoadDependencies(assetBundleName, LoadAssetBundleInternal);
        }

        // Remaps the asset bundle name to the best fitting asset bundle variant.
        protected static string RemapVariantName(string assetBundleName)
        {
            string[] bundlesWithVariant = m_AssetBundleManifest.GetAllAssetBundlesWithVariant();

            // If the asset bundle doesn't have variant, simply return.
            if (System.Array.IndexOf(bundlesWithVariant, assetBundleName) < 0)
                return assetBundleName;

            string[] split = assetBundleName.Split('.');

            int bestFit = int.MaxValue;
            int bestFitIndex = -1;
            // Loop all the assetBundles with variant to find the best fit variant assetBundle.
            for (int i = 0; i < bundlesWithVariant.Length; i++)
            {
                string[] curSplit = bundlesWithVariant[i].Split('.');
                if (curSplit[0] != split[0])
                    continue;

                int found = System.Array.IndexOf(m_Variants, curSplit[1]);
                if (found != -1 && found < bestFit)
                {
                    bestFit = found;
                    bestFitIndex = i;
                }
            }

            if (bestFitIndex != -1)
                return bundlesWithVariant[bestFitIndex];
            else
                return assetBundleName;
        }

        protected delegate bool delegateLoadBundleHandler(string assetBundleName, bool isLoadingAssetBundleManifest);

        private static void CheckFilePath(string assetBundleName)
        {
            if (!m_FilePathMap.ContainsKey(assetBundleName))
            {
                BundlePathMap bpm = new BundlePathMap();
                bpm.BundleName = assetBundleName;
                // 如果外部有,则地址为外部地址.
                if (File.Exists(ViewDefine.DataPathSync + assetBundleName))
                {
                    bpm.IsInStreaming = false;
                    bpm.SyncFilePath = ViewDefine.DataPathSync + assetBundleName;
                    bpm.ASyncFilePath = ViewDefine.DataPathASync + assetBundleName;
                }
                else
                {
                    // 外部没有,则地址为streaming地址
                    bpm.IsInStreaming = true;
                    bpm.SyncFilePath = ViewDefine.StreamingPathSync + assetBundleName;
                    bpm.ASyncFilePath = ViewDefine.StreamingPathASync + assetBundleName;
                }
                m_FilePathMap.Add(assetBundleName, bpm);
            }
        }

        protected static bool LoadAssetBundleInternalFromFile(string assetBundleName, bool isLoadingAssetBundleManifest)
        {
            // Already loaded.
            LoadedAssetBundle bundle;
            m_LoadedAssetBundles.TryGetValue(assetBundleName, out bundle);
            if (bundle != null)
            {
                bundle.m_ReferencedCount++;
                return true;
            }

            CheckFilePath(assetBundleName);

            if (m_DownloadingWWWs.ContainsKey(assetBundleName))
            {
                m_DownloadingWWWs[assetBundleName].Dispose();
                m_DownloadingWWWs.Remove(assetBundleName);
            }

            AssetBundle _Bundle;
            string url = m_FilePathMap[assetBundleName].SyncFilePath;
            if (IsUnCompressLoad)
            {
                _Bundle = AssetBundle.LoadFromFile(url);
            }
            else
            {
                byte[] bytes = File.ReadAllBytes(url);
                _Bundle = AssetBundle.LoadFromMemory(bytes);
            }
//        LogSystem.LogWarning("LoadAssetBundleInternalFromFile: " + url);

            m_LoadedAssetBundles.Add(assetBundleName, new LoadedAssetBundle(_Bundle));
            return false;
        }

        // Where we actuall call WWW to download the assetBundle.
        protected static bool LoadAssetBundleInternal(string assetBundleName, bool isLoadingAssetBundleManifest)
        {
            // Already loaded.
            LoadedAssetBundle bundle;
            m_LoadedAssetBundles.TryGetValue(assetBundleName, out bundle);
            if (bundle != null)
            {
                bundle.m_ReferencedCount++;
                return true;
            }

            // @TODO: Do we need to consider the referenced count of WWWs?
            // In the demo, we never have duplicate WWWs as we wait LoadAssetAsync()/LoadLevelAsync() to be finished before calling another LoadAssetAsync()/LoadLevelAsync().
            // But in the real case, users can call LoadAssetAsync()/LoadLevelAsync() several times then wait them to be finished which might have duplicate WWWs.
            if (m_DownloadingWWWs.ContainsKey(assetBundleName))
                return true;

            CheckFilePath(assetBundleName);
            WWW download;
            string url = m_FilePathMap[assetBundleName].ASyncFilePath;

//         LogSystem.LogWarning("LoadAssetBundleInternal: " + url);
            // For manifest assetbundle, always download it as we don't have hash for it.
            if (isLoadingAssetBundleManifest)
            {
                download = new WWW(url);
            }
            else
                download = WWW.LoadFromCacheOrDownload(url, m_AssetBundleManifest.GetAssetBundleHash(assetBundleName), 0);

            m_DownloadingWWWs.Add(assetBundleName, download);

            return false;
        }

        private static List<string> m_DependsList = new List<string>();
        // Where we get all the dependencies and load them all.
        protected static void LoadDependencies(string assetBundleName, delegateLoadBundleHandler loadHandler = null)
        {
            if (m_AssetBundleManifest == null)
            {
                Debug.LogError("Please initialize AssetBundleManifest by calling AssetBundleManager.Initialize()");
                return;
            }

            // Get dependecies from the AssetBundleManifest object..
            string[] dependencies = m_AssetBundleManifest.GetAllDependencies(assetBundleName);

            if (dependencies == null || dependencies.Length == 0)
                return;

            for (int i = 0; i < dependencies.Length; i++)
            {
                if (!string.IsNullOrEmpty(dependencies[i]))
                {
                    dependencies[i] = RemapVariantName(dependencies[i]);
                    m_DependsList.Add(dependencies[i]);
                }
            }

            // Record and load all dependencies.
            m_Dependencies.Add(assetBundleName, m_DependsList.ToArray());
            for (int i = 0; i < m_DependsList.Count; i++)
            {
                //loadHandler(dependencies[i], false);
//            MonoBehaviour.print("LoadDependencies Item[" + i + "] --->>>> " + dependencies[i]);
                loadHandler(m_DependsList[i], false);
            }
            m_DependsList.Clear();
        }

        // Unload assetbundle and its dependencies.
        public static void UnloadAssetBundle(string assetBundleName)
        {
#if UNITY_EDITOR
            // If we're in Editor simulation mode, we don't have to load the manifest assetBundle.
            if (SimulateAssetBundleInEditor)
                return;
#endif
            m_UnloadBundles.Add(assetBundleName);
            // LogSystem.Log(m_LoadedAssetBundles.Count + " assetbundle(s) in memory before unloading " + assetBundleName);

//		UnloadAssetBundleInternal(assetBundleName);
//		UnloadDependencies(assetBundleName);

            // LogSystem.Log(m_LoadedAssetBundles.Count + " assetbundle(s) in memory after unloading " + assetBundleName);
        }

        protected static void UnloadDependencies(string assetBundleName)
        {
            string[] dependencies;
            if (!m_Dependencies.TryGetValue(assetBundleName, out dependencies))
                return;

            // Loop dependencies.
            int length = dependencies.Length;
            for (int i = 0; i < length; i++)
            {
                UnloadAssetBundleInternal(dependencies[i]);
            }

            m_Dependencies.Remove(assetBundleName);
        }

        protected static void UnloadAssetBundleInternal(string assetBundleName)
        {
            string error;
            LoadedAssetBundle bundle = GetLoadedAssetBundle(assetBundleName, out error);
            if (bundle == null)
                return;

            if (--bundle.m_ReferencedCount == 0)
            {
                if (bundle.m_AssetBundle)
                {
                    bundle.m_AssetBundle.Unload(false);
                }
                m_LoadedAssetBundles.Remove(assetBundleName);
                //// LogSystem.Log("AssetBundle " + assetBundleName + " has been unloaded successfully");
            }
        }

        private void Update()
        {
            // Collect all the finished WWWs.
            foreach (KeyValuePair<string, WWW> keyValue in m_DownloadingWWWs)
            {
                WWW download = keyValue.Value;

                // If downloading fails.
                if (download.error != null)
                {
                    m_DownloadingErrors.Add(keyValue.Key, download.error);
                    m_KeysToRemove.Add(keyValue.Key);
                    continue;
                }

                // If downloading succeeds.
                if (download.isDone)
                {
                    //// LogSystem.Log("Downloading " + keyValue.Key + " is done at frame " + Time.frameCount);
                    if (!m_LoadedAssetBundles.ContainsKey(keyValue.Key))
                    {
                        m_LoadedAssetBundles.Add(keyValue.Key, new LoadedAssetBundle(download.assetBundle));
                    }
                    m_KeysToRemove.Add(keyValue.Key);
                }
            }

            // Remove the finished WWWs.
            int keysToRemoveLength = m_KeysToRemove.Count;
            for (int i = 0; i < keysToRemoveLength; i++)
            {
                string key = m_KeysToRemove[i];
                WWW download = m_DownloadingWWWs[key];
                m_DownloadingWWWs.Remove(key);
                download.Dispose();
            }
            m_KeysToRemove.Clear();


            // Update all in progress operations
            int m_InProgressOperationsLength = m_InProgressOperations.Count;
            for (int i = 0; i < m_InProgressOperationsLength;)
            {
                if (!m_InProgressOperations[i].Update())
                {
                    m_InProgressOperations.RemoveAt(i);
                    m_InProgressOperationsLength--;
                }
                else
                    i++;
            }

            for (int i = 0; i < m_UnloadBundles.Count; i++)
            {
//            Debug.LogWarning("Unload AssetBundle: " + m_UnloadBundles[i]);
                UnloadAssetBundleInternal(m_UnloadBundles[i]);
                UnloadDependencies(m_UnloadBundles[i]);
            }
            m_UnloadBundles.Clear();
        }

        // Load asset from the given assetBundle.
        public static AssetBundleLoadAssetOperation LoadAssetAsync(string assetBundleName, string assetName,
            System.Type type)
        {
            AssetBundleLoadAssetOperation operation;
#if UNITY_EDITOR
            if (SimulateAssetBundleInEditor)
            {
                string assetPath = "";
//			string[] assetPaths = AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(assetBundleName, assetName);
//			if (assetPaths.Length == 0)
//			{
//                Debug.LogError("There is no asset with name \"" + assetName + "\" in " + assetBundleName);
//				return null;
//			}

                // @TODO: Now we only get the main object from the first asset. Should consider type also.
                Object target = AssetDatabase.LoadMainAssetAtPath(assetPath);
                operation = new AssetBundleLoadAssetOperationSimulation(target);
            }
            else
#endif
            {
                LoadAssetBundle(assetBundleName);
                operation = new AssetBundleLoadAssetOperationFull(assetBundleName, assetName, type);

                m_InProgressOperations.Add(operation);
            }

            return operation;
        }

        // Load level from the given assetBundle.
        public static AssetBundleLoadOperation LoadLevelAsync(string assetBundleName, string levelName, bool isAdditive)
        {
            AssetBundleLoadOperation operation;
#if UNITY_EDITOR
            if (SimulateAssetBundleInEditor)
            {
                string levelPath = "";
//			string[] levelPaths = AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(assetBundleName, levelName);
//			if (levelPaths.Length == 0)
//			{
//				///@TODO: The error needs to differentiate that an asset bundle name doesn't exist
//				//        from that there right scene does not exist in the asset bundle...
//
//                Debug.LogError("There is no scene with name \"" + levelName + "\" in " + assetBundleName);
//				return null;
//			}

                SceneManager.LoadScene(levelPath, isAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single);
                operation = new AssetBundleLoadLevelSimulationOperation();
            }
            else
#endif
            {
                LoadAssetBundle(assetBundleName);
                operation = new AssetBundleLoadLevelOperation(assetBundleName, levelName, isAdditive);

                m_InProgressOperations.Add(operation);
            }

            return operation;
        }
    } // End of AssetBundleManager.
}