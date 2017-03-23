using System;
using System.Collections.Generic;
using UnityEngine;
public class TriggerEffectLogic_SkillEvent : TriggerEffectBase
{
    public TriggerEffectLogic_SkillEvent()
    {

    }
    public override void ExcuteAction(TriggerInfo triggerInfo, TriggerEffectInfo effectInfo)
    {
        //Debug.Log("TriggerGroupId = " + triggerInfo.triggerGroup.Id + "TriggerEffectLogic_SkillEvent");
        CharacterInfo charInfo = triggerInfo.charInfo;
        if (effectInfo.paramList != null && effectInfo.paramList[0] == "end")
        {
            SkillManager.getInstance().EndSkill(triggerInfo.skillInfo, triggerInfo.triggerGroup);
        }
    }
}

