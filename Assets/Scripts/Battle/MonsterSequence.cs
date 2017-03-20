using System;
using System.Collections.Generic;
using UnityEngine;
using MiniJSON;

//怪物序列
public class MonsterSequence
{
    //存储怪物出场顺序
    //key1-第几波  key2-路徑ID  value-怪物列表
    private Dictionary<int, Dictionary<int, List<int>>> monsterSeq;

    public MonsterSequence()
    {
        monsterSeq = new Dictionary<int, Dictionary<int, List<int>>>();
    }

    //加载怪物出场顺序
    public void LoadData(string name)
    {
        string infos = FileUtils.LoadFile(Application.dataPath, "Data/Json/" + name + ".json");
        List<object> jsonObjects = MiniJSON.Json.Deserialize(infos) as List<object>;
        for (int i = 0; i < jsonObjects.Count; i++)
        {
            Dictionary<string, object> jsonObject = jsonObjects[i] as Dictionary<string, object>;
            int wave = int.Parse(jsonObject["wave"].ToString());
            int monsterId = int.Parse(jsonObject["monsterId"].ToString());
            int pathId = int.Parse(jsonObject["pathId"].ToString());
            AddSequence(wave, monsterId, pathId);
        }
    }

    public void AddSequence(int wave, int monsterId, int pathId)
    {
        if (!monsterSeq.ContainsKey(wave))
        {
            monsterSeq[wave] = new Dictionary<int, List<int>>();
        }
        if (!monsterSeq[wave].ContainsKey(pathId))
        {
            monsterSeq[wave][pathId] = new List<int>();
        }
        monsterSeq[wave][pathId].Add(monsterId);
    }
    //得到某一波的所有怪物Id
    public Dictionary<int, List<int>> GetMonsterByWave(int wave)
    {
        if (monsterSeq.ContainsKey(wave))
        {
            return monsterSeq[wave];
        }
        return null;
    }
    //得到某一波某一條路徑的所有怪物Id
    public List<int> GetMonsterByPath(int wave, int pathid)
    {
        if (!monsterSeq.ContainsKey(wave))
        {
            return null;
        }
        if (!monsterSeq[wave].ContainsKey(pathid))
        {
            return null;
        }
        return monsterSeq[wave][pathid];
    }
}

