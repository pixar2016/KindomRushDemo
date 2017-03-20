using System;
using System.Collections.Generic;

public class TriggerCondLogic_SkillEvent : TriggerCondBase
{
    public TriggerCondLogic_SkillEvent()
    {

    }
    public override bool CheckCondition(TriggerInfo triggerInfo, TriggerCondInfo condInfo)
    {
        return condInfo.isConditionMatch;
    }
}

