// =====================================================
// -  Filename:  GameLoader.cs
// -  Descrip:   
// -  Author:    zhangjiaxin
// -  Email:     zhangjiaxin@longtugame.com
// -  Created:   2015-12-12 14:12
// -  (C) Copyright 2008 - 2015, www.longtugame.com
// -  All Rights Reserved.
// =====================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using HeroLogic;
using HeroView;
//using LuaInterface;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Object = UnityEngine.Object;

namespace Hero
{
    public class PrefabPathMap
    {
        public string BundleName;
        public string AssetPath;
        public string ResourcePath;
        public bool IsHasBundle;
        public bool IsInStreaming;
    }

    public class SameAssetInfo
    {
        public Action<ILoadAsset> CallbackAction;
    }

    public class GameLoader : MonoBehaviour
    {
        #region Singleton

        private static GameLoader _instance;

        public static GameLoader Instance
        {
            get { return _instance; }
        }

        #endregion

        public static string StaticeDataBundleName = "assets/configdata.bytes";
        public static string ScriptsDataBundleName = "assets/scriptsdata.bytes";

        public AssetBundle StaticDataBundle;
        public AssetBundle ScriptsBundle;

        private SpawnPool m_ScenePool;
        private SceneAnimatorPool m_SceneAnimPool;
        private SpawnPool m_GlobalPool;

        private List<string> m_AddBundleList = new List<string>();
        private MyDictionary<string, int> m_WaitForUnloadDict = new MyDictionary<string, int>();

        List<string> m_RemovedList = new List<string>();
        private static MyDictionary<string, PrefabPathMap> m_PrefabPathMap = new MyDictionary<string, PrefabPathMap>();

        private MyDictionary<string, SameAssetInfo> m_SameAssetDict = new MyDictionary<string, SameAssetInfo>();

        public void Initialize()
        {
            _instance = this;
            // register handlers
//            RegisterHandlers();
            // Don't destroy the game object as we base on it to run the loading script.
            DontDestroyOnLoad(gameObject);

            //LuaStatic.Load = LoadLuaFile;

            m_GlobalPool = PoolManager.Pools.Create("GlobalPool");
            m_GlobalPool.group.name = "GlobalGroup";
            m_GlobalPool.group.SetParent(transform);
            m_GlobalPool.group.localPosition = new Vector3(0, 0, 0);
            m_GlobalPool.group.localRotation = Quaternion.identity;
            m_GlobalPool.dontDestroyOnLoad = true;
        }

        public void StopAllTask() { this.StopAllCoroutines(); }

//        public byte[] LoadLuaFile(string luaPath)
//        {
//            string path = Util.LuaPath(luaPath);
//            byte[] assetBytes;
//#if UNITY_EDITOR
//            path = Util.LuaPrePath + path;
//            assetBytes = File.ReadAllBytes(path);
//#else
//            path = path.Replace(".lua", ".bytes");
//            Object assetObj = ScriptsBundle.LoadAsset("Assets/LuaScripts/" + path, typeof(TextAsset));
//            assetBytes = (assetObj as TextAsset).bytes;
//            Resources.UnloadAsset(assetObj);
//#endif
//            return assetBytes;
//        }


        private string ReadTxtFileForString(string fileName)
        {
#if UNITY_EDITOR
        return File.ReadAllText("Assets/" + fileName);
#else
            Object assetObj = LoadTextSync(fileName);
            if (!assetObj)
            {
                string err = "ReadTxtFileForString error : fileName=" + fileName + " NULL!";
                LogSystem.LogError(err);
                throw new Exception(err);
            }
            return (assetObj as TextAsset).text;
#endif
        }

        private byte[] ReadTxtFileForByte(string fileName)
        {
#if UNITY_EDITOR
        return File.ReadAllBytes("Assets/" + fileName);
#else
            Object assetObj = LoadTextSync(fileName);
            if (!assetObj)
            {
                string err = "ReadTxtFileForString error : fileName=" + fileName + " NULL!";
                LogSystem.LogError(err);
                throw new Exception(err);
            }
            return (assetObj as TextAsset).bytes;
#endif
        }

        private StreamReader ReadTxtFileForLine(string fileName)
        {
            return new StreamReader(new MemoryStream(ReadTxtFileForByte(fileName)), Encoding.UTF8);
        }

        /// <summary>
        /// 检测是否缓存池存在
        /// </summary>
        private void CheckScenePool()
        {
            if (!m_ScenePool)
            {
                GameObject obj = new GameObject("ScenePoolObj");

                m_ScenePool = PoolManager.Pools.Create("ScenePool");
                m_ScenePool.group.name = "SceneGroup";
                m_ScenePool.group.SetParent(obj.transform);
                m_ScenePool.group.localPosition = new Vector3(0, 0, 0);
                m_ScenePool.group.localRotation = Quaternion.identity;

                if (!m_SceneAnimPool)
                {
                    m_SceneAnimPool = obj.AddComponent<SceneAnimatorPool>();
                }
            }
        }

        /// <summary>
        /// 异步缓存资源
        /// </summary>
        /// <param name="cacheType">缓存类型,场景,全局</param>
        /// <param name="assetPaths">资源列表</param>
        /// <param name="finishCallback">完成回调</param>
        /// <param name="processCallBack">分步回调</param>
        public void CacheAssetASync(CacheType cacheType, string[] assetPaths, Action finishCallback = null, Action processCallBack = null)
        {
            CheckScenePool();
            List<string> disList = assetPaths.Distinct().ToList();
            int resCount = disList.Count;
            int loadCount = 0;
            Action loadOkAction = () =>
            {
                loadCount++;
                if (processCallBack != null)
                {
                    processCallBack();
                }
                if (loadCount >= resCount)
                {
                    UnloadBundles();
                    if (finishCallback != null)
                    {
                        finishCallback();
                    }
                }
            };
            if (resCount <= 0)
            {
                loadOkAction();
                return;
            }
            SpawnPool cachePool = m_ScenePool;
            if (cacheType == CacheType.GlobalCache)
            {
                cachePool = m_GlobalPool;
            }
            for (int i = 0; i < resCount; i++)
            {
                //            Debug.Log("AsyncCacheSceneAsset: " + assetPaths[i]);
                CacheAsync(disList[i], loadOkAction, cachePool);
            }
        }

        /// <summary>
        /// 同步加载资源
        /// </summary>
        /// <param name="assetPath">路径</param>
        /// <returns></returns>
        public ILoadAsset LoadAssetSync(string assetPath)
        {
            CheckScenePool();
            int indexOf = assetPath.LastIndexOf(".");
            if (indexOf < 0)
            {
                return new ILoadAsset();
            }
            string extName = assetPath.Substring(assetPath.LastIndexOf(".")).ToLower();
            Object uObj = null;
            ILoadAsset _Node = new ILoadAsset();

            if (extName.Equals(".prefab"))
            {
                if (m_ScenePool.prefabs.ContainsKey(assetPath))
                {
                    uObj = m_ScenePool.Spawn(assetPath).gameObject;
                }
                else if (m_GlobalPool.prefabs.ContainsKey(assetPath))
                {
                    uObj = m_GlobalPool.Spawn(assetPath).gameObject;
                }
                else
                {
                    uObj = Inner_LoadGameObjectSync(assetPath, m_ScenePool);
                }
            }
            else if (extName.Equals(".controller"))
            {
                if (m_SceneAnimPool.controllers.ContainsKey(assetPath))
                {
                    uObj = m_SceneAnimPool.controllers[assetPath];
                }
                else
                {
                    uObj = Inner_LoadObjectSync(assetPath, typeof(RuntimeAnimatorController));
                }
            }
            else if (extName.Equals(".exr") || extName.Equals(".png") || extName.Equals(".BMP") || extName.Equals(".bmp"))
            {
                uObj = Inner_LoadObjectSync(assetPath, typeof(Texture2D));
            }
            else if (extName.Equals(".mat"))
            {
                uObj = Inner_LoadObjectSync(assetPath, typeof(Material));
            }
            else if (extName.Equals(".lua") || extName.Equals(".luac"))
            {
                uObj = LoadTextSync(assetPath);
            }
            else if (assetPath.StartsWith("Data/"))
            {
                uObj = LoadTextSync(assetPath);
            }
            else if (extName.Equals(".bytes"))
            {
                uObj = Inner_LoadObjectSync(assetPath, typeof(TextAsset));
            }
            _Node.ObjectAsset = uObj;
            return _Node;
        }

        /// <summary>
        /// 异步加载
        /// </summary>
        /// <param name="assetPath">路径</param>
        /// <param name="afterCBHandler">回调</param>
        public void LoadAssetASync(string assetPath, Action<ILoadAsset> afterCBHandler)
        {
            LoadAsync(assetPath, afterCBHandler);
        }

        /// <summary>
        /// 检查prefab是否有更新的bundle
        /// </summary>
        /// <param name="prefabName"></param>
        /// <param name="isScene"></param>
        private static PrefabPathMap CheckPrefabBundle(string prefabName, bool isScene = false)
        {
            if (!m_PrefabPathMap.ContainsKey(prefabName))
            {
                string assetBundleName;
                PrefabPathMap ppm = new PrefabPathMap();
                if (!isScene)
                {
                    assetBundleName = ("Assets/" + prefabName + ".bytes").ToLower();
                }
                else
                {
                    assetBundleName = ("Assets/Scene/" + prefabName + ".unity.bytes").ToLower();
                }
                ppm.BundleName = assetBundleName;
                if (!isScene)
                {
                    ppm.AssetPath = "Assets/" + prefabName;
                    ppm.ResourcePath = prefabName.Substring(0, prefabName.LastIndexOf("."));
                }
                // 如果外部有,则地址为外部地址.
                if (File.Exists(ViewDefine.DataPathSync + assetBundleName))
                {
                    ppm.IsHasBundle = true;
                }
                else
                {
                    // 外部没有,则地址为StreamingAssets地址
                    ppm.IsHasBundle = false;
                }
                m_PrefabPathMap.Add(prefabName, ppm);
                return ppm;
            }
            return m_PrefabPathMap[prefabName];
        }

        private void AddBundleToUnloadList(string bundleName, bool isCache)
        {
            if (isCache)
            {
                if (!m_AddBundleList.Contains(bundleName))
                {
                    m_AddBundleList.Add(bundleName);
                }
            }
            else
            {
                if (!m_WaitForUnloadDict.ContainsKey(bundleName))
                {
                    m_WaitForUnloadDict.Add(bundleName, 2); // 等两帧卸载 
                }
            }
        }

        private void UnloadBundles()
        {
            for (int i = 0; i < m_AddBundleList.Count; i++)
            {
                //            LogSystem.LogWarning("Unload: " + m_AddBundleList[i]);
                AssetBundleManager.UnloadAssetBundle(m_AddBundleList[i]);
            }
            m_AddBundleList.Clear();
        }

        /// <summary>
        /// 加载GameObect,并且缓存
        /// </summary>
        /// <param name="assetPath">资源路径</param>
        /// <param name="cachePool">缓存池</param>
        /// <param name="isCache">是否为缓存</param>
        /// <returns></returns>
        private Object Inner_LoadGameObjectSync(string assetPath, SpawnPool cachePool, bool isCache = false)
        {
            //        LogSystem.Debug("Inner_LoadObjectSync: " + assetPath);
            Object uObj;
            PrefabPathMap ppm = CheckPrefabBundle(assetPath);
            if (ppm.IsHasBundle)
            {
                AssetBundle _Bundle = AssetBundleManager.LoadAssetBundleFromFile(ppm.BundleName);
                uObj = _Bundle.LoadAsset(ppm.AssetPath, typeof(GameObject));

                if (null != uObj) { AddBundleToUnloadList(ppm.BundleName, isCache); }
            }
            else
            {
#if UNITY_EDITOR
                uObj = AssetDatabase.LoadAssetAtPath(ppm.AssetPath, typeof(GameObject));
#else
            uObj = Resources.Load(ppm.ResourcePath, typeof(GameObject));
#endif
            }
            if (null != uObj)
            {
                CacheGameObject(uObj, assetPath, cachePool);
                if (!isCache)
                {
                    uObj = cachePool.Spawn(assetPath).gameObject;
                }
            }
            return uObj;
        }

        /// <summary>
        /// 同步加载Texture2D素材,不缓存,也不需要缓存
        /// </summary>
        /// <param name="assetPath">素材地址</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        private Object Inner_LoadObjectSync(string assetPath, Type type)
        {
            Object uObj;
            PrefabPathMap ppm = CheckPrefabBundle(assetPath);
            if (ppm.IsHasBundle)
            {
                AssetBundle _Bundle = AssetBundleManager.LoadAssetBundleFromFile(ppm.BundleName);
                uObj = _Bundle.LoadAsset(ppm.AssetPath, type);

                AddBundleToUnloadList(ppm.BundleName, false);
            }
            else
            {
#if UNITY_EDITOR
                uObj = AssetDatabase.LoadAssetAtPath(ppm.AssetPath, type);
#else
                uObj = Resources.Load(ppm.ResourcePath, type);
#endif
            }
            if (type == typeof(RuntimeAnimatorController))
            {
                CacheController(uObj, assetPath);
            }
            return uObj;
        }

//        /// <summary>
//        /// 加载Controller,并且缓存
//        /// </summary>
//        /// <param name="assetPath">资源路径</param>
//        /// <returns></returns>
//        private Object LoadControllerSync(string assetPath)
//        {
//            //        LogSystem.Debug("LoadControllerSync: " + assetPath);
//            Object uObj;
//            PrefabPathMap ppm = CheckPrefabBundle(assetPath);
//            if (ppm.IsHasBundle)
//            {
//                AssetBundle _Bundle = AssetBundleManager.LoadAssetBundleFromFile(ppm.BundleName);
//                uObj = _Bundle.LoadAsset(ppm.AssetPath, typeof(RuntimeAnimatorController));
//
//                AddBundleToUnloadList(ppm.BundleName, false);
//            }
//            else
//            {
//#if UNITY_EDITOR
//                uObj = AssetDatabase.LoadAssetAtPath(ppm.AssetPath, typeof(RuntimeAnimatorController));
//#else
//            uObj = Resources.Load(ppm.ResourcePath, typeof(RuntimeAnimatorController));
//#endif
//            }
//            CacheController(uObj, assetPath);
//            return uObj;
//        }

        /// <summary>
        /// 异步加载GameObject资源
        /// </summary>
        /// <param name="assetPath">资源路径</param>
        /// <param name="cachePool">缓存池</param>
        /// <param name="finishCall">完成回调</param>
        /// <param name="isCache">是否只是缓存</param>
        /// <returns></returns>
        private IEnumerator Inner_LoadGameObjectAsync(string assetPath, SpawnPool cachePool, Action<Object> finishCall, bool isCache = false)
        {
            //        LogSystem.Debug("Inner_LoadObjectAsync: " + assetPath);
            Object uObj;
            PrefabPathMap ppm = CheckPrefabBundle(assetPath);
            if (ppm.IsHasBundle)
            {
                AssetBundleLoadAssetOperation request = AssetBundleManager.LoadAssetAsync(ppm.BundleName, ppm.AssetPath,
                    typeof(GameObject));
                yield return StartCoroutine(request);
                uObj = request.GetAsset<GameObject>();
                AddBundleToUnloadList(ppm.BundleName, isCache);
            }
            else
            {
#if UNITY_EDITOR
                uObj = AssetDatabase.LoadAssetAtPath(ppm.AssetPath, typeof(GameObject));
                yield return null;
#else
            ResourceRequest request = Resources.LoadAsync(ppm.ResourcePath, typeof(GameObject));
            //yield return request;        //0415 王国庆  必须要判断isDone，否者可能就变成同步调用了
        while (!request.isDone) {
            yield return 0;
        }
            uObj = request.asset;
#endif
            }
            CacheGameObject(uObj, assetPath, cachePool);
            if (!isCache)
            {
                uObj = cachePool.Spawn(assetPath).gameObject;
            }
            if (finishCall != null)
            {
                finishCall(uObj);
            }
        }

        /// <summary>
        /// 异步加载Texture2D资源
        /// </summary>
        /// <param name="assetPath">资源地址</param>
        /// <param name="type">类型</param>
        /// <param name="finishCall">加载完成回调</param>
        /// <returns></returns>
        private IEnumerator Inner_LoadObjectAsync(string assetPath, Type type, Action<Object> finishCall)
        {
            Object uObj;
            PrefabPathMap ppm = CheckPrefabBundle(assetPath);
            if (ppm.IsHasBundle)
            {
                AssetBundleLoadAssetOperation request = AssetBundleManager.LoadAssetAsync(ppm.BundleName, ppm.AssetPath, type);
                yield return StartCoroutine(request);
                uObj = request.GetAsset<Object>();
                AddBundleToUnloadList(ppm.BundleName, false);
            }
            else
            {
#if UNITY_EDITOR
                uObj = AssetDatabase.LoadAssetAtPath(ppm.AssetPath, type);
                yield return null;
#else
            ResourceRequest request = Resources.LoadAsync(ppm.ResourcePath, type);
            //yield return request;        //0415 王国庆  必须要判断isDone，否者可能就变成同步调用了
        while (!request.isDone) {
            yield return 0;
        }
            uObj = request.asset;
#endif
            }
            if (type == typeof(RuntimeAnimatorController))
            {
                CacheController(uObj, assetPath);
            }
            if (finishCall != null)
            {
                finishCall(uObj);
            }
        }

//        /// <summary>
//        /// 异步加载AnimatorController资源
//        /// </summary>
//        /// <param name="assetPath">资源路径</param>
//        /// <param name="finishCall">完成回调</param>
//        /// <returns></returns>
//        private IEnumerator LoadControllerAsync(string assetPath, Action<Object> finishCall)
//        {
//            //        LogSystem.Debug("LoadControllerAsync: " + assetPath);
//            Object uObj;
//            PrefabPathMap ppm = CheckPrefabBundle(assetPath);
//            if (ppm.IsHasBundle)
//            {
//                AssetBundleLoadAssetOperation request = AssetBundleManager.LoadAssetAsync(ppm.BundleName, ppm.AssetPath,
//                    typeof(RuntimeAnimatorController));
//                yield return StartCoroutine(request);
//                uObj = request.GetAsset<RuntimeAnimatorController>();
//                AddBundleToUnloadList(ppm.BundleName, false);
//            }
//            else
//            {
//#if UNITY_EDITOR
//                uObj = AssetDatabase.LoadAssetAtPath(ppm.AssetPath, typeof(RuntimeAnimatorController));
//                yield return null;
//#else
//            ResourceRequest request = Resources.LoadAsync(ppm.ResourcePath, typeof(RuntimeAnimatorController));
//            yield return request;
//            uObj = request.asset;
//#endif
//            }
//            CacheController(uObj, assetPath);
//            if (finishCall != null)
//            {
//                finishCall(uObj);
//            }
//        }

        /// <summary>
        /// 加载Text资源
        /// 以资源路径为"Data"为开头的是静态元数据
        /// 其他为lua
        /// </summary>
        /// <param name="assetPath">Text路径</param>
        /// <returns></returns>
        private Object LoadTextSync(string assetPath)
        {
            Object uObj;
#if UNITY_EDITOR
            uObj = AssetDatabase.LoadAssetAtPath("Assets/" + assetPath, typeof(TextAsset));
#else
        if (assetPath.StartsWith("Data/"))
        {
            uObj = StaticDataBundle.LoadAsset("Assets/" + assetPath, typeof(TextAsset));
        }
        else
        {
            uObj = ScriptsBundle.LoadAsset("LuaScripts/" + assetPath, typeof(TextAsset));
        }
#endif
            return uObj;
        }




        /// <summary>
        /// 将Object加入缓存
        /// Prefab会加入GlobalCache或SceneCache.
        /// </summary>
        /// <param name="cacheObj">需要缓存的Obj</param>
        /// <param name="assetPath">路径</param>
        /// <param name="cachePool">缓存池</param>
        private void CacheGameObject(Object cacheObj, string assetPath, SpawnPool cachePool)
        {
            if (cachePool && !cachePool.prefabs.ContainsKey(assetPath))
            {
                GameObject obj = cacheObj as GameObject;
                if (obj)
                {
                    PrefabPool prefabPool = new PrefabPool(obj.transform);
                    prefabPool.prefabPath = assetPath;
                    prefabPool.preloadAmount = 1;
                    prefabPool.cullDespawned = false;
                    prefabPool.cullAbove = 1;
                    prefabPool.cullDelay = 1;
                    prefabPool.cullMaxPerPass = 5;
                    //                prefabPool.limitInstances = true;
                    //                prefabPool.limitAmount = 10;
                    //                prefabPool.limitFIFO = false;
                    cachePool.CreatePrefabPool(prefabPool);
                }
            }
        }

        /// <summary>
        /// 将Object加入缓存
        /// Controller会加入SceneAnimPool.
        /// </summary>
        /// <param name="cacheObj">需要缓存的Obj</param>
        /// <param name="assetPath">路径</param>
        private void CacheController(Object cacheObj, string assetPath)
        {
            if (!m_SceneAnimPool.controllers.ContainsKey(assetPath))
            {
                RuntimeAnimatorController controller = cacheObj as RuntimeAnimatorController;
                if (controller)
                {
                    m_SceneAnimPool.controllers._Add(assetPath, controller);
                }
            }
        }

        /// <summary>
        /// 缓存资源,对于Editor,直接AssetDatabse缓存.
        /// 对于没有更新资源的,Resource.LoadAsync.
        /// 对于有更新资源的,AssetBundleManager加载.
        /// </summary>
        /// <param name="assetPath">资源地址</param>
        /// <param name="finishCall">回调</param>
        /// <param name="cachePool">缓存池</param>
        /// <returns></returns>
        private void CacheAsync(string assetPath, Action finishCall, SpawnPool cachePool)
        {
            CheckScenePool();

            string extName = assetPath.Substring(assetPath.LastIndexOf(".")).ToLower();

            Action<Object> asyncRetAction = obj =>
            {
                //            Debug.Log("CacheAsync: " + assetPath);
                if (null != finishCall) { finishCall(); }
            };

            if (extName.Equals(".prefab"))
            {
                if (!m_ScenePool.prefabs.ContainsKey(assetPath) && !m_GlobalPool.prefabs.ContainsKey(assetPath))
                {
                    StartCoroutine(Inner_LoadGameObjectAsync(assetPath, cachePool, asyncRetAction, true));
                }
                else
                {
                    asyncRetAction(null);
                }
            }
            else if (extName.Equals(".controller"))
            {
                if (!m_SceneAnimPool.controllers.ContainsKey(assetPath))
                {
                    StartCoroutine(Inner_LoadObjectAsync(assetPath, typeof(RuntimeAnimatorController), asyncRetAction));
                }
                else
                {
                    asyncRetAction(null);
                }
            }
        }

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <param name="assetPath">资源路径</param>
        /// <param name="funcAfter">加载回调</param>
        /// <returns></returns>
        private void LoadAsync(string assetPath, Action<ILoadAsset> funcAfter)
        {
//        LogSystem.Log("Start to load " + assetPath + " at frame " + Time.frameCount);
            // 如果有正在加载中的,则把回调扔进去.
            if (!m_SameAssetDict.ContainsKey(assetPath))
            {
                m_SameAssetDict.Add(assetPath, new SameAssetInfo());
            }
            if (m_SameAssetDict[assetPath].CallbackAction != null && m_SameAssetDict[assetPath].CallbackAction.GetInvocationList().Length > 0)
            {
                m_SameAssetDict[assetPath].CallbackAction += funcAfter;
                return;
            }
            m_SameAssetDict[assetPath].CallbackAction = funcAfter;
            CheckScenePool();

            int indexOf = assetPath.LastIndexOf(".");
            if (indexOf < 0) {
                LogSystem.Debug("LoadAsync Failed ---->>>> " + assetPath);
                return; 
            }
            string extName = assetPath.Substring(indexOf).ToLower();
            Object uObj;

            Action<Object> asyncRetAction = obj =>
            {
                ILoadAsset _Node = new ILoadAsset();
                _Node.ObjectAsset = obj;
                int count = m_SameAssetDict[assetPath].CallbackAction.GetInvocationList().Length;
                
                if (m_SameAssetDict[assetPath].CallbackAction != null)
                {
                    if (extName.Equals(".prefab") && count>1)
                    {

                        Delegate[] events = m_SameAssetDict[assetPath].CallbackAction.GetInvocationList();
                        ((Action<ILoadAsset>)events[0])(_Node);
                        for (int i = 1; i < count; i++)
                        {
                            Object _uObj = m_ScenePool.Spawn(assetPath).gameObject;
                            ILoadAsset _Node1 = new ILoadAsset();
                            _Node1.ObjectAsset = _uObj;
                            ((Action<ILoadAsset>)events[i])(_Node1);
                        }
                        
                    }
                    else
                    {
                       m_SameAssetDict[assetPath].CallbackAction(_Node);
                    
                    }
                    m_SameAssetDict[assetPath].CallbackAction = null;
                }
            };

            if (extName.Equals(".prefab"))
            {
                if (m_ScenePool.prefabs.ContainsKey(assetPath))
                {
                    uObj = m_ScenePool.Spawn(assetPath).gameObject;
                    asyncRetAction(uObj);
                }
                else if (m_GlobalPool.prefabs.ContainsKey(assetPath))
                {
                    uObj = m_GlobalPool.Spawn(assetPath).gameObject;
                    asyncRetAction(uObj);
                }
                else
                {
                    //                LogSystem.Debug("LoadAsync: " + assetPath);
                    StartCoroutine(Inner_LoadGameObjectAsync(assetPath, m_ScenePool, asyncRetAction));
                }
            }
            else if (extName.Equals(".controller"))
            {
                if (m_SceneAnimPool.controllers.ContainsKey(assetPath))
                {
                    uObj = m_SceneAnimPool.controllers[assetPath];
                    asyncRetAction(uObj);
                }
                else
                {
                    StartCoroutine(Inner_LoadObjectAsync(assetPath, typeof(RuntimeAnimatorController), asyncRetAction));
                }
            }
            else if (extName.Equals(".exr"))
            {
                StartCoroutine(Inner_LoadObjectAsync(assetPath, typeof(Texture2D), asyncRetAction));
            }
            else if (extName.Equals(".mat"))
            {
                StartCoroutine(Inner_LoadObjectAsync(assetPath, typeof(Material), asyncRetAction));
            }
            else if (assetPath.StartsWith("Data/"))
            {
                uObj = LoadTextSync(assetPath);
                asyncRetAction(uObj);
            }
            else if (extName.Equals(".bytes"))
            {
                StartCoroutine(Inner_LoadObjectAsync(assetPath, typeof(TextAsset), asyncRetAction));
            }
        }

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="levelName">场景名称</param>
        /// <param name="afterCBHandler">加载回调</param>
        /// <param name="isAdditive">是否为叠加方式</param>
        public void LoadLevelASync(string levelName, Action<ILoadAsset> afterCBHandler, bool isAdditive = false)
        {
            StartCoroutine(LoadLevelAsync(levelName, isAdditive, afterCBHandler));
        }

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="levelName">场景名称</param>
        /// <param name="isAdditive">是否叠加</param>
        /// <param name="funcAfter">完成回调</param>
        /// <returns></returns>
        private IEnumerator LoadLevelAsync(string levelName, bool isAdditive, Action<ILoadAsset> funcAfter = null)
        {
            //        LogSystem.Log("Start to load scene " + levelName + " at frame " + Time.frameCount);
            ILoadAsset _Node = new ILoadAsset();
            PrefabPathMap ppm = CheckPrefabBundle(levelName, true);
            if (ppm.IsHasBundle)
            {
                AssetBundleLoadOperation request = AssetBundleManager.LoadLevelAsync(ppm.BundleName, levelName, isAdditive);
                yield return StartCoroutine(request);
                AddBundleToUnloadList(ppm.BundleName, false);
            }
            else
            {
                AsyncOperation operation = SceneManager.LoadSceneAsync(levelName, isAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single);
                yield return operation;
            }
            GC.Collect();
            //        LogSystem.Log("Finish loading scene " + levelName + " at frame " + Time.frameCount);

            if (funcAfter != null) { funcAfter(_Node); }
        }

        public void UnLoadLevel(ILoadAsset _assetNode)
        {
            //        if (StaticDataBundle)
            //        {
            //            StaticDataBundle.Unload(true);
            //            StaticDataBundle = null;
            //        }
            Resources.UnloadUnusedAssets();
        }

        public void UnLoadGameObject(ILoadAsset _assetNode)
        {
            if (_assetNode != null)
            {
                Object obj = _assetNode.ObjectAsset as Object;
                //            if (obj)
                //            {
                //                if (m_ScenePool.IsSpawned(obj.transform))
                //                {
                //                    m_ScenePool.Despawn(obj.transform);
                //                }
                //                else
                //                {
                //                    Destroy(obj);
                //                }
                //                _assetNode.AssetObject = null;
                //            }
                //            else 
                if (obj)
                {
                    //                LogSystem.Log("----------Destroy: " + _assetNode.AssetObject);
                    Destroy(obj);
                }
                //if (_assetNode.isInstance) { GameObject.Destroy(_assetNode.AssetObject as GameObject); }
            }
        }

        private void Update()
        {
            //        AutoFreeAssetBundle();
            m_WaitForUnloadDict.VisitKeys(key =>
            {
                m_WaitForUnloadDict[key]--;
                if (m_WaitForUnloadDict[key] <= 0)
                {
                    m_RemovedList.Add(key);
                    AssetBundleManager.UnloadAssetBundle(key);
                }
            });
            for (int i = 0; i < m_RemovedList.Count; i++)
            {
                m_WaitForUnloadDict.Remove(m_RemovedList[i]);
            }
            m_RemovedList.Clear();
        }
    }
}