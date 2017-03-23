using System;
using System.Collections.Generic;
using UnityEngine;
public class TriggerManager
{
    private static TriggerManager instance = null;
    public int triggerGroupId;
    public Dictionary<int, TriggerGroup> triggerGroups;
    public Dictionary<string, TriggerCondBase> condLogics;
    public Dictionary<string, TriggerEffectBase> effectLogics;

    public static TriggerManager getInstance()
    {
        if (instance == null)
        {
            instance = new TriggerManager();
        }
        return instance;
    }

    private TriggerManager()
    {
        triggerGroupId = 0;
        triggerGroups = new Dictionary<int, TriggerGroup>();
        condLogics = new Dictionary<string, TriggerCondBase>();
        effectLogics = new Dictionary<string, TriggerEffectBase>();
        AddAllLogic();
    }

    private void AddAllLogic()
    {
        RegisterCondLogic("SkillEvent", new TriggerCondLogic_SkillEvent());
        RegisterCondLogic("ActionEnd", new TriggerCondLogic_ActionEnd());
        RegisterCondLogic("BulletReach", new TriggerCondLogic_BulletReach());
        RegisterEffectLogic("Action", new TriggerEffectLogic_Action());
        RegisterEffectLogic("Hurt", new TriggerEffectLogic_Hurt());
        RegisterEffectLogic("Bullet", new TriggerEffectLogic_Bullet());
        RegisterEffectLogic("SkillEvent", new TriggerEffectLogic_SkillEvent());
    }

    private void RegisterCondLogic(string condName, TriggerCondBase logic)
    {
        if (condLogics.ContainsKey(condName))
        {
            condLogics[condName] = logic;
        }
        else
        {
            condLogics.Add(condName, logic);
        }
    }

    private void RegisterEffectLogic(string effectName, TriggerEffectBase logic)
    {
        if (effectLogics.ContainsKey(effectName))
        {
            effectLogics[effectName] = logic;
        }
        else
        {
            effectLogics.Add(effectName, logic);
        }
    }

    public void Update(float deltaTime)
    {
        foreach (int key in triggerGroups.Keys)
        {
            TriggerGroup group = triggerGroups[key];
            if (group.GetActive())
            {
                foreach (TriggerInfo trigger in group.triggerInfos)
                {
                    //Debug.Log("triggerInfo Id = " + trigger.Id + "Active = " + trigger.GetActive());
                    if (trigger.GetActive())
                    {
                        TickTrigger(trigger, deltaTime);
                    }
                }
            }
        }
    }

    private void TickTrigger(TriggerInfo triggerInfo, float deltaTime)
    {
        triggerInfo.TickCount++;
        triggerInfo.EffectTime += deltaTime;
        bool isAllCondMatch = true;
        foreach (TriggerCondInfo condInfo in triggerInfo.condInfos)
        {
            if (condLogics.ContainsKey(condInfo.EventName))
            {
                //Debug.Log("CondName:"+condInfo.EventName);
                condLogics[condInfo.EventName].CheckCondition(triggerInfo, condInfo);
            }
            if (!condInfo.isConditionMatch)
            {
                isAllCondMatch = false;
                break;
            }
        }
        if (isAllCondMatch)
        {
            foreach (TriggerEffectInfo effectInfo in triggerInfo.effectInfos)
            {
                if (effectLogics.ContainsKey(effectInfo.EventName))
                {
                    effectLogics[effectInfo.EventName].ExcuteAction(triggerInfo, effectInfo);
                    //若为技能结束事件，不需要执行下面代码
                    if (effectInfo.EventName == "SkillEvent" && effectInfo.paramList != null && effectInfo.paramList[0] == "end")
                    {
                        return;
                    }
                }
            }
            triggerInfo.EffectCount++;
        }
        bool isLifeOver = false;
        foreach (TriggerCondInfo condInfo in triggerInfo.condInfos)
        {
            if (condLogics[condInfo.EventName].IsLifeOver(triggerInfo, condInfo))
            {
                isLifeOver = true;
                break;
            }
        }

        if (isLifeOver)
        {
            StopTrigger(triggerInfo);
        }
    }

    void StopTrigger(TriggerInfo triggerInfo)
    {
        //Debug.Log("--------------------StopTrigger " + triggerInfo.Id);
        triggerInfo.isActive = false;
    }

    TriggerGroup AddTriggerGroup()
    {
        triggerGroupId++;
        TriggerGroup group = new TriggerGroup(triggerGroupId);
        if (triggerGroups.ContainsKey(triggerGroupId))
        {
            triggerGroups[triggerGroupId] = group;
        }
        else
        {
            triggerGroups.Add(triggerGroupId, group);
        }
        return group;
    }

    public TriggerGroup AddSkillTrigger(SkillInfo skillInfo)
    {
        //Debug.Log("AddSkillTrigger");
        TriggerGroup group = AddTriggerGroup();
        group.LoadDataWithSkill(skillInfo);
        return group;
    }

    public void ResetTriggerGroups()
    {
        foreach (int key in triggerGroups.Keys)
        {
            foreach (TriggerInfo trigger in triggerGroups[key].triggerInfos)
            {
                trigger.Reset();
            }
        }
    }

    public void Release()
    {
        triggerGroups.Clear();
        condLogics.Clear();
        effectLogics.Clear();
        instance = null;
    }
}

