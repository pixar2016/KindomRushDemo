//-----------------------------------------------------
//这段代码是工具生成，不要轻易修改！！！
//-----------------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;

public class D_ModelResource
{
	public int _id;
	public string _modelName;
	public string _modelPath;

}
public class J_ModelResource
{
    private static Dictionary<int, D_ModelResource> infoDict = new Dictionary<int, D_ModelResource>();
    private static string tableName = "";
    public static void LoadConfig()
    {
        if (infoDict.Count > 0)
        {
            return;
        }
        tableName = "ModelResource";
        string infos = FileUtils.LoadFile(Application.dataPath, "Data/Json/ModelResource.json");
		Init(infos);
    }

    private static void Init(string _info)
    {
        List<object> jsonObjects = MiniJSON.Json.Deserialize(_info) as List<object>;
        for (int i = 0; i < jsonObjects.Count; i++)
        {
            D_ModelResource info = new D_ModelResource();
			Dictionary<string, object> jsonObject = jsonObjects[i] as Dictionary<string, object>;
			
			if(jsonObject["id"] != null){
				info._id = int.Parse(jsonObject["id"].ToString());
			}
			if(jsonObject["modelName"] != null){
				info._modelName = jsonObject["modelName"].ToString();
			}
			if(jsonObject["modelPath"] != null){
				info._modelPath = jsonObject["modelPath"].ToString();
			}

            infoDict.Add(info._id, info);
        }
        /*
        foreach (KeyValuePair<int, D_ModelResource> info in infoDict)
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
    public static D_ModelResource GetData(int _id)
    {
        D_ModelResource data = null;
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
    public static List<D_ModelResource> ToList()
    {
        List<D_ModelResource> list =  new List<D_ModelResource>();
        foreach (KeyValuePair<int,D_ModelResource> info in infoDict)
        {
            list.Add(info.Value);
        }
        return list;
    }

}
