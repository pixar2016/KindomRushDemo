using System;
using System.Collections.Generic;
using UnityEngine;
public class TriggerInfo
{
    public int Id;
    public int TickCount;//帧计数
    public int EffectCount;//生效计数
    public float EffectTime;//生效时间
    public bool isActive;//是否生效

    public CharacterInfo charInfo;
    public SkillInfo skillInfo;
    public TriggerGroup triggerGroup;

    public List<TriggerCondInfo> condInfos;
    public List<TriggerEffectInfo> effectInfos;
    public TriggerInfo(int id)
    {
        Id = id;
        TickCount = 0;
        EffectCount = 0;
        EffectTime = 0;
        isActive = true;
        condInfos = new List<TriggerCondInfo>();
        effectInfos = new List<TriggerEffectInfo>();
    }

    public void LoadDataWithConfig(TriggerConfigData data)
    {
        int index = 0;
        foreach (ConfigData condition in data.conditions)
        {
            index++;
            TriggerCondInfo condInfo = new TriggerCondInfo(index);
            condInfo.triggerInfo = this;
            condInfo.LoadDataWithConfig(condition);
            condInfos.Add(condInfo);
        }
        index = 0;
        foreach (ConfigData effect in data.effects)
        {
            index++;
            TriggerEffectInfo effectInfo = new TriggerEffectInfo(index);
            effectInfo.triggerInfo = this;
            effectInfo.LoadDataWithConfig(effect);
            effectInfos.Add(effectInfo);
        }
    }

    public void Reset()
    {
        isActive = true;
        TickCount = 0;
        EffectCount = 0;
        //Debug.Log("Reset TriggerInfo"+Id+" EffectCount" + EffectCount+"\n");
        EffectTime = 0;
        foreach (TriggerCondInfo condInfo in condInfos)
        {
            condInfo.Reset();
        }
        foreach (TriggerEffectInfo effectInfo in effectInfos)
        {
            effectInfo.Reset();
        }
    }

    public bool GetActive()
    {
        if (this.isActive)
        {
            if (triggerGroup != null && triggerGroup.GetActive())
            {
                return true;
            }
        }
        return false;
    }

}

