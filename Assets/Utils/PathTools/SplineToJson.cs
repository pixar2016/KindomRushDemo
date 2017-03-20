using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;

public class SplineToJson : EditorWindow
{
    /// <summary>
    /// 编辑器窗口实例
    /// </summary>
    private static SplineToJson instance;

    private static Vector2 scrollPos;

    private static List<GameObject> objs;

    private static string m_Path;
    public static GUIStyle TextFieldRoundEdge;
    public static GUIStyle TextFieldRoundEdgeCancelButton;
    public static GUIStyle TextFieldRoundEdgeCancelButtonEmpty;
    public static GUIStyle TransparentTextField;

    [MenuItem("Plugins/SplineTools/SplineToJson")]
    static void ShowPathTools()
    {
        Init();
        LoadPath();
    }

    private static void Init()
    {
        instance = EditorWindow.GetWindow<SplineToJson>();
        objs = new List<GameObject>();
        m_Path = "level";
        //GameObject[] temp = GameObject.FindGameObjectsWithTag("path");
        //foreach (GameObject obj in temp)
        //{
        //    objs.Add(obj);
        //}
    }
    void OnGUI()
    {
        DrawOptions();
        DrawExport();
    }

    private void DrawOptions()
    {
        if (objs == null) return;
        EditorGUILayout.LabelField("现有的路径为：");
        if (objs.Count < 1)
        {
            EditorGUILayout.LabelField("目前没有路径被选中哦!");
        }
        GUILayout.BeginVertical();
        scrollPos = GUILayout.BeginScrollView(scrollPos, false, true, GUILayout.Height(150));
        foreach (GameObject obj in objs)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Toggle(true, obj.name);
            GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();
        GUILayout.EndVertical();
    }

    private void DrawExport()
    {
        EditorGUILayout.LabelField("Path:");
        m_Path = RelativeAssetPathTextField(m_Path, ".json");
        if (GUILayout.Button("转换路径为json"))
        {
            Convert();
        }
    }

    private string RelativeAssetPathTextField(string path, string extension)
    {
        if (TextFieldRoundEdge == null)
        {
            TextFieldRoundEdge = new GUIStyle("SearchTextField");
            TextFieldRoundEdgeCancelButton = new GUIStyle("SearchCancelButton");
            TextFieldRoundEdgeCancelButtonEmpty = new GUIStyle("SearchCancelButtonEmpty");
            TransparentTextField = new GUIStyle(EditorStyles.whiteLabel);
            TransparentTextField.normal.textColor = EditorStyles.textField.normal.textColor;
        }

        Rect position = EditorGUILayout.GetControlRect();
        GUIStyle textFieldRoundEdge = TextFieldRoundEdge;
        GUIStyle transparentTextField = TransparentTextField;
        GUIStyle gUIStyle = (path != "") ? TextFieldRoundEdgeCancelButton : TextFieldRoundEdgeCancelButtonEmpty;
        position.width -= gUIStyle.fixedWidth;
        if (Event.current.type == EventType.Repaint)
        {
            GUI.contentColor = (EditorGUIUtility.isProSkin ? Color.black : new Color(0f, 0f, 0f, 0.5f));
            textFieldRoundEdge.Draw(position, new GUIContent("Assets/Data/Json/"), 0);
            GUI.contentColor = Color.white;
        }
        Rect rect = position;
        float num = textFieldRoundEdge.CalcSize(new GUIContent("Assets/Data/Json/")).x - 2f;
        rect.x += num;
        rect.y += 1f;
        rect.width -= num;
        EditorGUI.BeginChangeCheck();
        path = EditorGUI.TextField(rect, path, transparentTextField);
        if (EditorGUI.EndChangeCheck())
        {
            path = path.Replace('\\', '/');
        }
        if (Event.current.type == EventType.Repaint)
        {
            Rect position2 = rect;
            float num2 = transparentTextField.CalcSize(new GUIContent(path + ".")).x - EditorStyles.whiteLabel.CalcSize(new GUIContent(".")).x; ;
            position2.x += num2;
            position2.width -= num2;
            GUI.contentColor = (EditorGUIUtility.isProSkin ? Color.black : new Color(0f, 0f, 0f, 0.5f));
            EditorStyles.label.Draw(position2, extension, false, false, false, false);
            GUI.contentColor = Color.white;
        }
        position.x += position.width;
        position.width = gUIStyle.fixedWidth;
        position.height = gUIStyle.fixedHeight;
        if (GUI.Button(position, GUIContent.none, gUIStyle) && path != "")
        {
            path = "";
            GUI.changed = true;
            GUIUtility.keyboardControl = 0;
        }
        return path;
    }

    private void Convert()
    {
        if (objs == null)
            return;
        //List<object> table = new List<object>();
        Dictionary<string, object> table = new Dictionary<string, object>();
        foreach (GameObject obj in objs)
        {
            if (obj.GetComponent<Spline>() != null)
            {
                List<object> singlePath = new List<object>();
                Spline spline = obj.GetComponent<Spline>();
                for (float i = 0; i <= 1; i = i + 0.04f)
                {
                    Dictionary<string, float> pathPoint = new Dictionary<string, float>();
                    Vector3 pos = spline.GetPositionOnSpline(i);
                    pathPoint.Add("x", pos.x);
                    pathPoint.Add("y", pos.y);
                    singlePath.Add((object)pathPoint);
                }
                table.Add(obj.name, (object)singlePath);
            }
            else
            {
                List<object> singlePath = new List<object>();
                foreach (Transform trans in obj.transform)
                {
                    Dictionary<string, float> pathPoint = new Dictionary<string, float>();
                    Vector3 pos = trans.position;
                    pathPoint.Add("x", pos.x);
                    pathPoint.Add("y", pos.y);
                    singlePath.Add((object)pathPoint);
                }
                table.Add(obj.name, (object)singlePath);
            }        
        }
        //生成Json字符串
        string json = JsonConvert.SerializeObject(table, Newtonsoft.Json.Formatting.Indented);
        //Debug.Log(json);
        //Debug.Log(m_Path);
        FileUtils.SaveFile(Application.dataPath, "Data/Json/" + m_Path + ".json", json);
        Debug.Log("Convert Success!!");
    }

    void OnSelectionChange()
    {
        Show();
        LoadPath();
        Repaint();
    }
    static void LoadPath()
    {
        if (objs == null)
            objs = new List<GameObject>();
        objs.Clear();
        object[] selection = (object[])Selection.objects;
        if (selection.Length == 0)
        {
            return;
        }
        foreach (object obj in selection)
        {
            if (obj.GetType() == typeof(UnityEngine.GameObject))
            {
                objs.Add((GameObject)obj);
            }
        }
    }
}
