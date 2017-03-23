using System;
using System.Collections.Generic;
using UnityEngine;
public class SkillInfo
{
    public int Id;
    public int skillId;
    public CharacterInfo charInfo;
    public int triggerId;
    public List<TriggerGroup> triggerGroupList;
    public bool isActive;
    //用于广播事件
    public MiniEventDispatcher eventDispatcher;
    public SkillInfo(int indexId, int skillId)
    {
        this.Id = indexId;
        this.skillId = skillId;
        this.triggerId = J_Skill.GetData(skillId)._triggerId;
        this.triggerGroupList = new List<TriggerGroup>();
        this.eventDispatcher = new MiniEventDispatcher();
    }

    public void Start()
    {
        //Debug.Log("SkillEvent_Start");
        TriggerGroup trigger = GetSpareTriggerGroup();
        Reset(trigger, true);
        //Debug.Log("triggerGroup--------------" + triggerGroup.Id + "-----" + triggerGroup.GetActive());
        eventDispatcher.Broadcast("SkillEvent_start");
    }

    public TriggerGroup GetSpareTriggerGroup()
    {
        foreach (TriggerGroup trigger in triggerGroupList)
        {
            if (!trigger.GetActive())
            {
                return trigger;
            }
        }
        Debug.Log("AddNewTriggerGroup");
        TriggerGroup trigger1 = TriggerManager.getInstance().AddSkillTrigger(this);
        triggerGroupList.Add(trigger1);
        return trigger1;
    }

    public void End(TriggerGroup trigger)
    {
        trigger.isActive = false;
        foreach (TriggerGroup v in triggerGroupList)
        {
            if (v.GetActive())
            {
                return;
            }
        }
        Debug.Log("SkillEvent_End");
        this.isActive = false;
    }

    public void LoadData()
    {
        TriggerGroup triggerGroup = TriggerManager.getInstance().AddSkillTrigger(this);
        triggerGroupList.Add(triggerGroup);
    }

    public void Reset(TriggerGroup trigger, bool isActive)
    {
        this.isActive = isActive;
        trigger.Reset(isActive);
    }

    public bool GetActive()
    {
        if (this.isActive)
        {
            return true;
        }
        return false;

    }

    public void Update()
    {

    }
}

