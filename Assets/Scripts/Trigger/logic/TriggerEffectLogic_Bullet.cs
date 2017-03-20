using System;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEffectLogic_Bullet : TriggerEffectBase
{
    public TriggerEffectLogic_Bullet()
    {

    }
    public override void ExcuteAction(TriggerInfo triggerInfo, TriggerEffectInfo effectInfo)
    {
        CharacterInfo charInfo = triggerInfo.charInfo;
        CharacterInfo targetInfo = charInfo.GetAttackInfo();
        EntityManager.getInstance().AddBullet(1, charInfo, charInfo.GetPosition(), targetInfo.GetPosition(), 500f);
        //EntityManager.getInstance().AddMoveEffect(1, charInfo.GetPosition(), targetInfo.GetPosition(), 2.5f);
    }
}

