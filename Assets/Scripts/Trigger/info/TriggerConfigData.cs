using System;
using System.Collections.Generic;
public class ConfigData
{
    public string infoName;
    public List<string> paramList;
    public ConfigData()
    {
        paramList = new List<string>();
    }
}

public class TriggerConfigData
{

    public List<ConfigData> conditions;
    public List<ConfigData> effects;
    public TriggerConfigData()
    {
        conditions = new List<ConfigData>();
        effects = new List<ConfigData>();
    }

    public void SetInfo(D_SkillEvent info)
    {
        if (info._condName1 != null)
        {
            ConfigData data = new ConfigData();
            data.infoName = info._condName1;
            if (info._condParam1 != null)
            {
                string[] strArray = info._condParam1.Split('|');
                foreach (string i in strArray)
                {
                    data.paramList.Add(i);
                }
            }
            conditions.Add(data);
        }
        if (info._condName2 != null)
        {
            ConfigData data = new ConfigData();
            data.infoName = info._condName2;
            if (info._condParam2 != null)
            {
                string[] strArray = info._condParam2.Split('|');
                foreach (string i in strArray)
                {
                    data.paramList.Add(i);
                }
            }
            conditions.Add(data);
        }
        if (info._effName1 != null)
        {
            ConfigData data = new ConfigData();
            data.infoName = info._effName1;
            if (info._effParam1 != null)
            {
                string[] strArray = info._effParam1.Split('|');
                foreach (string i in strArray)
                {
                    data.paramList.Add(i);
                }
            }
            effects.Add(data);
        }
        if (info._effName2 != null)
        {
            ConfigData data = new ConfigData();
            data.infoName = info._effName2;
            if (info._effParam2 != null)
            {
                string[] strArray = info._effParam2.Split('|');
                foreach (string i in strArray)
                {
                    data.paramList.Add(i);
                }
            }
            effects.Add(data);
        }
        if (info._effName3 != null)
        {
            ConfigData data = new ConfigData();
            data.infoName = info._effName3;
            if (info._effParam3 != null)
            {
                string[] strArray = info._effParam3.Split('|');
                foreach (string i in strArray)
                {
                    data.paramList.Add(i);
                }
            }
            effects.Add(data);
        }
        if (info._effName4 != null)
        {
            ConfigData data = new ConfigData();
            data.infoName = info._effName4;
            if (info._effParam4 != null)
            {
                string[] strArray = info._effParam4.Split('|');
                foreach (string i in strArray)
                {
                    data.paramList.Add(i);
                }
            }
            effects.Add(data);
        }
        if (info._effName5 != null)
        {
            ConfigData data = new ConfigData();
            data.infoName = info._effName5;
            if (info._effParam5 != null)
            {
                string[] strArray = info._effParam5.Split('|');
                foreach (string i in strArray)
                {
                    data.paramList.Add(i);
                }
            }
            effects.Add(data);
        }
    }
}

