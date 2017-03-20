//-----------------------------------------------------
//这段代码是工具生成，不要轻易修改！！！
//-----------------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;

public class D_Map
{
	public int _id;
	public string _name;
	public int _wave;
	public int _hp;
	public int _gold;
	public string _pathJson;
	public string _monsterJson;

}
public class J_Map
{
    private static Dictionary<int, D_Map> infoDict = new Dictionary<int, D_Map>();
    private static string tableName = "";
    public static void LoadConfig()
    {
        if (infoDict.Count > 0)
        {
            return;
        }
        tableName = "Map";
        string infos = FileUtils.LoadFile(Application.dataPath, "Data/Json/Map.json");
		Init(infos);
    }

    private static void Init(string _info)
    {
        List<object> jsonObjects = MiniJSON.Json.Deserialize(_info) as List<object>;
        for (int i = 0; i < jsonObjects.Count; i++)
        {
            D_Map info = new D_Map();
			Dictionary<string, object> jsonObject = jsonObjects[i] as Dictionary<string, object>;
			
			if(jsonObject["id"] != null){
				info._id = int.Parse(jsonObject["id"].ToString());
			}
			if(jsonObject["name"] != null){
				info._name = jsonObject["name"].ToString();
			}
			if(jsonObject["wave"] != null){
				info._wave = int.Parse(jsonObject["wave"].ToString());
			}
			if(jsonObject["hp"] != null){
				info._hp = int.Parse(jsonObject["hp"].ToString());
			}
			if(jsonObject["gold"] != null){
				info._gold = int.Parse(jsonObject["gold"].ToString());
			}
			if(jsonObject["pathJson"] != null){
				info._pathJson = jsonObject["pathJson"].ToString();
			}
			if(jsonObject["monsterJson"] != null){
				info._monsterJson = jsonObject["monsterJson"].ToString();
			}

            infoDict.Add(info._id, info);
        }
        /*
        foreach (KeyValuePair<int, D_Map> info in infoDict)
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
    public static D_Map GetData(int _id)
    {
        D_Map data = null;
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
    public static List<D_Map> ToList()
    {
        List<D_Map> list =  new List<D_Map>();
        foreach (KeyValuePair<int,D_Map> info in infoDict)
        {
            list.Add(info.Value);
        }
        return list;
    }

}
