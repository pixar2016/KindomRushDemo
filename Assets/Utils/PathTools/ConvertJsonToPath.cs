using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Hero;

public class ConvertJsonToPath:EditorWindow
{
    private static ConvertJsonToPath instance;

    private static GameObject BaseObj;

    private static List<string> jsonList;

    private static Vector2 scrollPos;
    static void ReadJson()
    {
        Init();
        LoadJson();
        instance.Show();
    }

    private static void LoadJson()
    {
        if (jsonList == null) jsonList = new List<string>();
        jsonList.Clear();
        object[] selection = (object[])Selection.objects;
        if (selection.Length == 0)
            return;
        foreach (Object obj in selection)
        {
            string objPath = AssetDatabase.GetAssetPath(obj);
            if (objPath.EndsWith(".json"))
            {
                jsonList.Add(objPath);
            }
        }
    }

    private static void Init()
    {
        instance = EditorWindow.GetWindow<ConvertJsonToPath>();
        BaseObj = GameObject.Find("Base");
    }

    void OnGUI()
    {
        DrawOptions();
        DrawExport();
    }
    private void DrawOptions()
    {
        if (jsonList == null) return;
        EditorGUILayout.LabelField("现有的路径为：");
        if (jsonList.Count < 1)
        {
            EditorGUILayout.LabelField("目前没有路径被选中哦!");
        }
        GUILayout.BeginVertical();
        scrollPos = GUILayout.BeginScrollView(scrollPos, false, true, GUILayout.Height(150));
        foreach (string obj in jsonList)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Toggle(true, obj);
            GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();
        GUILayout.EndVertical();
    }
    private void DrawExport()
    {
        if (GUILayout.Button("转换json成路径"))
        {
            Convert();
        }
    }

    private void Convert()
    {
        int count = 1;
        foreach (string jsonPath in jsonList)
        {
            string pathRoot = Application.dataPath;
            pathRoot = pathRoot.Substring(0, pathRoot.LastIndexOf("/"));
            string info = FileUtils.LoadFile(pathRoot, jsonPath);
            ConvertJson(info, count);
            count = count + 1;
        }
    }

    private void ConvertJson(string info, int count)
    {
        if (BaseObj == null)
            BaseObj = GameObject.Find("Base");

        Dictionary<string, object> table = MiniJSON.Json.Deserialize(info) as Dictionary<string, object>;
        foreach(string key in table.Keys){
            List<object> singlePath = table[key] as List<object>;
            Object tempObj;
            GameObject towerObj;
            Transform towerTrans = BaseObj.transform.FindChild(key);
            if (towerTrans == null)
            {
                towerObj = new GameObject();
                towerObj.name = key;
                towerObj.transform.parent = BaseObj.transform;
            }
            else
            {
                towerObj = towerTrans.gameObject;
                towerObj.name = key;
            }
            if (key == "Towers")
            {
                tempObj = Resources.Load("Prefabs/tower", typeof(GameObject));
            }
            else
            {
                tempObj = Resources.Load("Prefabs/littlesheep", typeof(GameObject));
            }
            foreach (object point in singlePath)
            {
                Dictionary<string, object> pathPoint = point as Dictionary<string, object>;
                float x = float.Parse(pathPoint["x"].ToString());
                float y = float.Parse(pathPoint["y"].ToString());
                CreateTag(new Vector3(x, y, 0), towerObj.transform, tempObj);
            }
        }
    }

    private void CreateSheep(Vector3 pos, Transform parent, Object obj)
    {
        GameObject sheep = Instantiate(obj) as GameObject;
        sheep.transform.parent = parent;
        sheep.transform.position = pos;
        sheep.GetComponent<SpriteRenderer>().sortingOrder = 1;
    }

    private void CreateTag(Vector3 pos, Transform parent, Object obj)
    {
        GameObject sheep = Instantiate(obj) as GameObject;
        sheep.transform.parent = parent;
        sheep.transform.position = pos;
        sheep.GetComponent<SpriteRenderer>().sortingOrder = 1;
    }

    void OnSelectionChange()
    {
        Show();
        LoadJson();
        Repaint();
    }
}

