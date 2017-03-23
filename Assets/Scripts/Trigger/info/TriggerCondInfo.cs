using System;
using System.Collections.Generic;
using UnityEngine;
public class TriggerCondInfo
{
    public int Id;
    public string EventName;
    public List<string> paramList;
    public TriggerInfo triggerInfo;

    public bool isConditionMatch;

    public TriggerCondInfo(int id)
    {
        Id = id;
        EventName = "Frame";
        isConditionMatch = false;
    }

    public void LoadDataWithConfig(ConfigData config)
    {
        //事件名
        this.EventName = config.infoName;
        //参数列表
        this.paramList = config.paramList;
        //根据事件名添加监听
        if (this.EventName == "SkillEvent")
        {
            this.triggerInfo.skillInfo.eventDispatcher.Register("SkillEvent_" + this.paramList[0], SkillEvent_Start);
        }
        else if (this.EventName == "ActionEnd")
        {
            this.triggerInfo.charInfo.eventDispatcher.Register("ActionEnd", ActionEnd);
        }
        else if (this.EventName == "BulletReach")
        {
            this.triggerInfo.charInfo.eventDispatcher.Register("BulletReach", BulletReach);
        }
    }

    public void Reset()
    {
        isConditionMatch = false;
    }

    public bool GetActive()
    {
        if (triggerInfo != null && triggerInfo.GetActive())
        {
            return true;
        }
        return false;
    }

    public void SkillEvent_Start(object[] paramList)
    {
        //Debug.Log("Receive SkillEvent_Start");
        this.isConditionMatch = true;
    }

    public void ActionEnd(object[] paramList)
    {
        //Debug.Log("Receive ActionEnd");
        this.isConditionMatch = true;
    }

    public void BulletReach(object[] paramList)
    {
        int triggerGroupId = (int)paramList[0];
        if (this.triggerInfo.triggerGroup.Id == triggerGroupId)
        {
            this.isConditionMatch = true;
        }
    }
}
