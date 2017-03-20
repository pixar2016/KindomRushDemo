using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public class DataPreLoader {

    private static DataPreLoader instance = null;

    public Dictionary<string, JsonData> JsonDict;

    private DataPreLoader()
    {
        JsonDict = new Dictionary<string, JsonData>();
    }

    public static DataPreLoader getInstance()
    {
        if (instance == null)
        {
            instance = new DataPreLoader();
        }
        return instance;
    }

    public void LoadData(string fileName)
    {
        string infos = FileUtils.LoadFile(Application.dataPath, "Resources/Data/" + fileName + ".json");
        JsonData json = JsonMapper.ToObject(infos);
        JsonDict.Add(fileName, json);
    }

    public JsonData GetData(string fileName)
    {
        if (JsonDict.ContainsKey(fileName))
        {
            return JsonDict[fileName];
        }
        return null;
    }

    public void Clear()
    {
        JsonDict.Clear();
    }
}
