using System;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCondBase
{
    public TriggerCondBase()
    {

    }

    public virtual bool IsLifeOver(TriggerInfo triggerInfo, TriggerCondInfo condInfo)
    {
        //Debug.Log("triggerInfo Id = "+triggerInfo.Id+"IsLifeOver EffectCount = " + triggerInfo.EffectCount + "\n");
        return triggerInfo.EffectCount > 0;
    }

    public virtual bool CheckCondition(TriggerInfo triggerInfo, TriggerCondInfo condInfo)
    {
        return true;
    }

    public virtual void ResetCondition(TriggerInfo triggerInfo, TriggerCondInfo condInfo)
    {
        condInfo.isConditionMatch = false;
    }
}

