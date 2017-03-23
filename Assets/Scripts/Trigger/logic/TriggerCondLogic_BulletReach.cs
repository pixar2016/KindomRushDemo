using System;
using System.Collections.Generic;

public class TriggerCondLogic_BulletReach : TriggerCondBase
{
    public TriggerCondLogic_BulletReach()
    {

    }

    public override bool CheckCondition(TriggerInfo triggerInfo, TriggerCondInfo condInfo)
    {
        return condInfo.isConditionMatch;
    }
}

