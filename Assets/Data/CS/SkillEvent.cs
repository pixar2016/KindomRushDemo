//-----------------------------------------------------
//这段代码是工具生成，不要轻易修改！！！
//-----------------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;

public class D_SkillEvent
{
	public int _id;
	public int _triggerId;
	public string _condName1;
	public string _condParam1;
	public string _condName2;
	public string _condParam2;
	public string _effName1;
	public string _effParam1;
	public string _effName2;
	public string _effParam2;
	public string _effName3;
	public string _effParam3;
	public string _effName4;
	public string _effParam4;
	public string _effName5;
	public string _effParam5;

}
public class J_SkillEvent
{
    private static Dictionary<int, D_SkillEvent> infoDict = new Dictionary<int, D_SkillEvent>();
    private static string tableName = "";
    public static void LoadConfig()
    {
        if (infoDict.Count > 0)
        {
            return;
        }
        tableName = "SkillEvent";
        string infos = FileUtils.LoadFile(Application.dataPath, "Data/Json/SkillEvent.json");
		Init(infos);
    }

    private static void Init(string _info)
    {
        List<object> jsonObjects = MiniJSON.Json.Deserialize(_info) as List<object>;
        for (int i = 0; i < jsonObjects.Count; i++)
        {
            D_SkillEvent info = new D_SkillEvent();
			Dictionary<string, object> jsonObject = jsonObjects[i] as Dictionary<string, object>;
			
			if(jsonObject["id"] != null){
				info._id = int.Parse(jsonObject["id"].ToString());
			}
			if(jsonObject["triggerId"] != null){
				info._triggerId = int.Parse(jsonObject["triggerId"].ToString());
			}
			if(jsonObject["condName1"] != null){
				info._condName1 = jsonObject["condName1"].ToString();
			}
			if(jsonObject["condParam1"] != null){
				info._condParam1 = jsonObject["condParam1"].ToString();
			}
			if(jsonObject["condName2"] != null){
				info._condName2 = jsonObject["condName2"].ToString();
			}
			if(jsonObject["condParam2"] != null){
				info._condParam2 = jsonObject["condParam2"].ToString();
			}
			if(jsonObject["effName1"] != null){
				info._effName1 = jsonObject["effName1"].ToString();
			}
			if(jsonObject["effParam1"] != null){
				info._effParam1 = jsonObject["effParam1"].ToString();
			}
			if(jsonObject["effName2"] != null){
				info._effName2 = jsonObject["effName2"].ToString();
			}
			if(jsonObject["effParam2"] != null){
				info._effParam2 = jsonObject["effParam2"].ToString();
			}
			if(jsonObject["effName3"] != null){
				info._effName3 = jsonObject["effName3"].ToString();
			}
			if(jsonObject["effParam3"] != null){
				info._effParam3 = jsonObject["effParam3"].ToString();
			}
			if(jsonObject["effName4"] != null){
				info._effName4 = jsonObject["effName4"].ToString();
			}
			if(jsonObject["effParam4"] != null){
				info._effParam4 = jsonObject["effParam4"].ToString();
			}
			if(jsonObject["effName5"] != null){
				info._effName5 = jsonObject["effName5"].ToString();
			}
			if(jsonObject["effParam5"] != null){
				info._effParam5 = jsonObject["effParam5"].ToString();
			}

            infoDict.Add(info._id, info);
        }
        /*
        foreach (KeyValuePair<int, D_SkillEvent> info in infoDict)
        {
            Debug.Log(">>>>>"+info.Value._id+":"+info.Value._name+":"+info.Value._desc+":"+info.Value._point+":"+info.Value._label+":"+info.Value._type+":"+info.Value._number+":"+info.Value._function+":"+info.Value._para+":"+info.Value._reward+":"+"<<<<<\n");
        }
        */
    }

    /// <summary>
    /// 通过key获取数据
    /// </summary>
    /// <param name="_id">字典key</param>
    /// <returns></returns>
    public static D_SkillEvent GetData(int _id)
    {
        D_SkillEvent data = null;
        if (infoDict.ContainsKey(_id))
        {
            data = infoDict[_id];
        }
        else
        {
            Debug.Log(">>>>>table:" + tableName+" id:"+_id+" is null<<<<<\n");
        }
        return data;
    }
    /// <summary>
    /// 获取字典长度
    /// </summary>
    /// <returns></returns>
    public static int GetCount()
    {
        return infoDict.Count;
    }
    /// <summary>
    /// 把字典转换成List
    /// </summary>
    /// <returns></returns>
    public static List<D_SkillEvent> ToList()
    {
        List<D_SkillEvent> list =  new List<D_SkillEvent>();
        foreach (KeyValuePair<int,D_SkillEvent> info in infoDict)
        {
            list.Add(info.Value);
        }
        return list;
    }

}
