using System;
using System.Collections.Generic;
using UnityEngine;
public class SkillInfo
{
    public int Id;
    public int skillId;
    public CharacterInfo charInfo;
    public int triggerId;
    public TriggerGroup triggerGroup;
    public bool isActive;
    //用于广播事件
    public MiniEventDispatcher eventDispatcher;
    public SkillInfo(int indexId, int skillId)
    {
        this.Id = indexId;
        this.skillId = skillId;
        this.triggerId = J_Skill.GetData(skillId)._triggerId;
        this.eventDispatcher = new MiniEventDispatcher();
    }

    public void Start()
    {
        //Debug.Log("SkillEvent_Start");
        Reset(true);
        //Debug.Log("triggerGroup--------------" + triggerGroup.Id + "-----" + triggerGroup.GetActive());
        eventDispatcher.Broadcast("SkillEvent_start");
    }

    public void End()
    {
        //Debug.Log("SkillEvent_End");
        Reset(false);
        eventDispatcher.Broadcast("SkillEnd");
    }

    public void LoadData()
    {
        triggerGroup = TriggerManager.getInstance().AddSkillTrigger(this);
    }

    public void Reset(bool isActive)
    {
        this.isActive = isActive;
        this.triggerGroup.Reset(isActive);
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

