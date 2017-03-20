using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class ExcelTools : EditorWindow
{
	/// <summary>
	/// 当前编辑器窗口实例
	/// </summary>
	private static ExcelTools instance;

	/// <summary>
	/// Excel文件列表
	/// </summary>
	private static List<string> excelList;

	/// <summary>
	/// 项目根路径	
	/// </summary>
	private static string pathRoot;

	/// <summary>
	/// 滚动窗口初始位置
	/// </summary>
	private static Vector2 scrollPos;

	/// <summary>
	/// 输出格式索引
	/// </summary>
	private static int indexOfFormat=0;

	/// <summary>
	/// 输出格式
	/// </summary>
	private static string[] formatOption=new string[]{"JSON","CSV","XML","LUA","CS"};

	/// <summary>
	/// 编码索引
	/// </summary>
	private static int indexOfEncoding=0;

	/// <summary>
	/// 编码选项
	/// </summary>
	private static string[] encodingOption=new string[]{"UTF-8","GB2312"};

	/// <summary>
	/// 是否保留原始文件
	/// </summary>
	private static bool keepSource=true;
    
    /// <summary>
    /// 输出路径
    /// </summary>
    private static string m_Path;

    public static GUIStyle TextFieldRoundEdge;
    public static GUIStyle TextFieldRoundEdgeCancelButton;
    public static GUIStyle TextFieldRoundEdgeCancelButtonEmpty;
    public static GUIStyle TransparentTextField;

	/// <summary>
	/// 显示当前窗口	
	/// </summary>
    [MenuItem("Plugins/ExcelTools")]
	static void ShowExcelTools()
	{
		Init();
		//加载Excel文件
		LoadExcel();
		instance.Show();
	}

	void OnGUI()
	{
		DrawOptions();
		DrawExport();
	}

	/// <summary>
	/// 绘制插件界面配置项
	/// </summary>
	private void DrawOptions()
	{
		GUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("请选择格式类型:",GUILayout.Width(85));
		indexOfFormat=EditorGUILayout.Popup(indexOfFormat,formatOption,GUILayout.Width(125));
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("请选择编码类型:",GUILayout.Width(85));
		indexOfEncoding=EditorGUILayout.Popup(indexOfEncoding,encodingOption,GUILayout.Width(125));
		GUILayout.EndHorizontal();

		keepSource=GUILayout.Toggle(keepSource,"保留Excel源文件");
	}

	/// <summary>
	/// 绘制插件界面输出项
	/// </summary>
	private void DrawExport()
	{
		if(excelList==null) return;
		if(excelList.Count<1)
		{
			EditorGUILayout.LabelField("目前没有Excel文件被选中哦!");
		}
		else
		{
			EditorGUILayout.LabelField("下列项目将被转换为" + formatOption[indexOfFormat] + ":");
			GUILayout.BeginVertical();
			scrollPos=GUILayout.BeginScrollView(scrollPos,false,true,GUILayout.Height(150));
			foreach(string s in excelList)
			{
				GUILayout.BeginHorizontal();
				GUILayout.Toggle(true,s);
				GUILayout.EndHorizontal();
			}
			GUILayout.EndScrollView();
			GUILayout.EndVertical();

            EditorGUILayout.LabelField("Path:");
            if (indexOfFormat == 0 || indexOfFormat == 1 || indexOfFormat == 2 || indexOfFormat == 3)
            {
                m_Path = "Json/";
                m_Path = RelativeAssetPathTextField(m_Path, "");
            }
            else if (indexOfFormat == 4)
            {
                m_Path = "CS/";
                m_Path = RelativeAssetPathTextField(m_Path, "");
            }
            
			//输出
			if(GUILayout.Button("转换"))
			{
				Convert();
			}
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
            textFieldRoundEdge.Draw(position, new GUIContent("Assets/Data/"), 0);
            GUI.contentColor = Color.white;
        }
        Rect rect = position;
        float num = textFieldRoundEdge.CalcSize(new GUIContent("Assets/Data/")).x - 2f;
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
        return "Assets/Data/" + path;
    }

	/// <summary>
	/// 转换Excel文件
	/// </summary>
	private static void Convert()
	{
		foreach(string assetsPath in excelList)
		{
            int index = assetsPath.LastIndexOf("/");
            string tempPath = assetsPath.Substring(index + 1);
            //Debug.Log(pathRoot + "/" + m_Path + tempPath);
            string outputPath = pathRoot + "/" + m_Path + tempPath;
			//获取Excel文件的绝对路径
            string excelPath = pathRoot + "/" + assetsPath;
			//构造Excel工具类
			ExcelUtility excel=new ExcelUtility(excelPath);

			//判断编码类型
			Encoding encoding=null;
			if(indexOfEncoding==0 || indexOfEncoding==3)
            {
				encoding=Encoding.GetEncoding("utf-8");
			}else if(indexOfEncoding==1){
				encoding=Encoding.GetEncoding("gb2312");
			}

			//判断输出类型
			string output="";
			if(indexOfFormat==0){
                output = outputPath.Replace(".xlsx", ".json");
                excel.ConvertToJson(output, encoding);
                //excel.Test(output, encoding);
			}else if(indexOfFormat==1){
                output = outputPath.Replace(".xlsx", ".csv");
				excel.ConvertToCSV(output,encoding);
			}else if(indexOfFormat==2){
                output = outputPath.Replace(".xlsx", ".xml");
				excel.ConvertToXml(output);
			}else if (indexOfFormat == 3)
            {
                output = outputPath.Replace(".xlsx", ".lua");
                excel.ConvertToLua(output, encoding);
            }
            else if (indexOfFormat == 4)
            {
                output = outputPath.Replace(".xlsx", ".cs");
                excel.ConvertToCS(output, encoding);
            }

			//判断是否保留源文件
			if(!keepSource)
			{
				FileUtil.DeleteFileOrDirectory(excelPath);
			}

			//刷新本地资源
			AssetDatabase.Refresh();
		}

		//转换完后关闭插件
		//这样做是为了解决窗口
		//再次点击时路径错误的Bug
		instance.Close();

	}

	/// <summary>
	/// 加载Excel
	/// </summary>
	private static void LoadExcel()
	{
		if(excelList==null) excelList=new List<string>();
		excelList.Clear();
		//获取选中的对象
		object[] selection=(object[])Selection.objects;
		//判断是否有对象被选中
		if(selection.Length==0)
			return;
		//遍历每一个对象判断不是Excel文件
		foreach(Object obj in selection)
		{
			string objPath=AssetDatabase.GetAssetPath(obj);
			if(objPath.EndsWith(".xlsx"))
			{
				excelList.Add(objPath);
			}
		}
	}

	private static void Init()
	{
		//获取当前实例
		instance=EditorWindow.GetWindow<ExcelTools>();
		//初始化
		pathRoot=Application.dataPath;
		//注意这里需要对路径进行处理
		//目的是去除Assets这部分字符以获取项目目录
		//我表示Windows的/符号一直没有搞懂
		pathRoot=pathRoot.Substring(0,pathRoot.LastIndexOf("/"));
		excelList=new List<string>();
		scrollPos=new Vector2(instance.position.x,instance.position.y+75);
	}

	void OnSelectionChange() 
	{
		//当选择发生变化时重绘窗体
		Show();
		LoadExcel();
		Repaint();
	}
}
