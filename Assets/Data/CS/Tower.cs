//-----------------------------------------------------
//这段代码是工具生成，不要轻易修改！！！
//-----------------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;

public class D_Tower
{
	public int _id;
	public string _name;
	public int _modelId;
	public int _towerType;
	public string _towerBase;
	public string _Shooter;
	public int _level;
	public int _attackSpeed;
	public int _attackDamage;
	public int _attackRange;
	public string _description;
	public int _attackId;
	public int _skill1;
	public int _skill2;
	public int _skill3;
	public int _skill4;

}
public class J_Tower
{
    private static Dictionary<int, D_Tower> infoDict = new Dictionary<int, D_Tower>();
    private static string tableName = "";
    public static void LoadConfig()
    {
        if (infoDict.Count > 0)
        {
            return;
        }
        tableName = "Tower";
        string infos = FileUtils.LoadFile(Application.dataPath, "Data/Json/Tower.json");
		Init(infos);
    }

    private static void Init(string _info)
    {
        List<object> jsonObjects = MiniJSON.Json.Deserialize(_info) as List<object>;
        for (int i = 0; i < jsonObjects.Count; i++)
        {
            D_Tower info = new D_Tower();
			Dictionary<string, object> jsonObject = jsonObjects[i] as Dictionary<string, object>;
			
			if(jsonObject["id"] != null){
				info._id = int.Parse(jsonObject["id"].ToString());
			}
			if(jsonObject["name"] != null){
				info._name = jsonObject["name"].ToString();
			}
			if(jsonObject["modelId"] != null){
				info._modelId = int.Parse(jsonObject["modelId"].ToString());
			}
			if(jsonObject["towerType"] != null){
				info._towerType = int.Parse(jsonObject["towerType"].ToString());
			}
			if(jsonObject["towerBase"] != null){
				info._towerBase = jsonObject["towerBase"].ToString();
			}
			if(jsonObject["Shooter"] != null){
				info._Shooter = jsonObject["Shooter"].ToString();
			}
			if(jsonObject["level"] != null){
				info._level = int.Parse(jsonObject["level"].ToString());
			}
			if(jsonObject["attackSpeed"] != null){
				info._attackSpeed = int.Parse(jsonObject["attackSpeed"].ToString());
			}
			if(jsonObject["attackDamage"] != null){
				info._attackDamage = int.Parse(jsonObject["attackDamage"].ToString());
			}
			if(jsonObject["attackRange"] != null){
				info._attackRange = int.Parse(jsonObject["attackRange"].ToString());
			}
			if(jsonObject["description"] != null){
				info._description = jsonObject["description"].ToString();
			}
			if(jsonObject["attackId"] != null){
				info._attackId = int.Parse(jsonObject["attackId"].ToString());
			}
			if(jsonObject["skill1"] != null){
				info._skill1 = int.Parse(jsonObject["skill1"].ToString());
			}
			if(jsonObject["skill2"] != null){
				info._skill2 = int.Parse(jsonObject["skill2"].ToString());
			}
			if(jsonObject["skill3"] != null){
				info._skill3 = int.Parse(jsonObject["skill3"].ToString());
			}
			if(jsonObject["skill4"] != null){
				info._skill4 = int.Parse(jsonObject["skill4"].ToString());
			}

            infoDict.Add(info._id, info);
        }
        /*
        foreach (KeyValuePair<int, D_Tower> info in infoDict)
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
    public static D_Tower GetData(int _id)
    {
        D_Tower data = null;
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
    public static List<D_Tower> ToList()
    {
        List<D_Tower> list =  new List<D_Tower>();
        foreach (KeyValuePair<int,D_Tower> info in infoDict)
        {
            list.Add(info.Value);
        }
        return list;
    }

}
