using UnityEditor;
using UnityEngine;

public class TestWindow : EditorWindow
{
    static void Init()
    {
        var window = EditorWindow.GetWindow<TestWindow>();
        window.titleContent.text = "路径设置";
        window.Show();
    }

    private string m_Path;

    void OnGUI()
    {
        EditorGUILayout.LabelField("Path:");
        m_Path = RelativeAssetPathTextField(m_Path, ".prefab");
    }

    public static GUIStyle TextFieldRoundEdge;
    public static GUIStyle TextFieldRoundEdgeCancelButton;
    public static GUIStyle TextFieldRoundEdgeCancelButtonEmpty;
    public static GUIStyle TransparentTextField;

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
            textFieldRoundEdge.Draw(position, new GUIContent("Assets/"), 0);
            GUI.contentColor = Color.white;
        }
        Rect rect = position;
        float num = textFieldRoundEdge.CalcSize(new GUIContent("Assets/")).x - 2f;
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
}
