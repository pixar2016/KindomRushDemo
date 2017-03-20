using System;
using System.Collections.Generic;

public class TriggerEffectInfo
{
    public int Id;
    public string EventName;
    public List<string> paramList;
    public TriggerInfo triggerInfo;

    public TriggerEffectInfo(int id)
    {
        Id = id;
    }

    public void LoadDataWithConfig(ConfigData config)
    {
        this.EventName = config.infoName;
        this.paramList = config.paramList;
    }

    public void Reset()
    {

    }

    public bool GetActive()
    {
        if (triggerInfo != null && triggerInfo.GetActive())
        {
            return true;
        }
        return false;
    }
}

