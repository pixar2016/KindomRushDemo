using System;
using System.Collections.Generic;
using UnityEngine;
public class TriggerCondLogic_ActionEnd : TriggerCondBase
{
    public TriggerCondLogic_ActionEnd()
    {

    }
    public override bool CheckCondition(TriggerInfo triggerInfo, TriggerCondInfo condInfo)
    {
        CharacterInfo charInfo = triggerInfo.charInfo;
        float animTime = charInfo.attackTime;
        //float animTime = AnimationCache.getInstance().getAnimation(charInfo.charName).getMeshAnimation(charInfo.actionName).getAnimTime();
        if (animTime == null)
        {
            Debug.Log("Error:Can not get correct animTime");
            return false;
        }
        //Debug.Log("EffectTime = " + triggerInfo.EffectTime);
        condInfo.isConditionMatch = triggerInfo.EffectTime >= animTime;
        return condInfo.isConditionMatch;
    }
}

