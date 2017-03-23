using System;
using System.Collections.Generic;
using UnityEngine;
//范围伤害
public class TriggerEffectLogic_AreaDamage : TriggerEffectBase
{
    public TriggerEffectLogic_AreaDamage()
    {

    }

    public override void ExcuteAction(TriggerInfo triggerInfo, TriggerEffectInfo effectInfo)
    {
        CharacterInfo charInfo = triggerInfo.charInfo;
        CharacterInfo targetInfo = charInfo.GetAttackInfo();
        Vector3 targetPos = targetInfo.GetPosition();
        List<MonsterInfo> monsterList = EntityManager.getInstance().GetMonsterInfo();
        foreach (MonsterInfo monster in monsterList)
        {
            if (Vector3.Distance(targetPos, monster.GetPosition()) <= 100)
            {
                //在范围内造成伤害
            }
        }
    }

    //在圆形范围内
    public bool WithinCircle(int radius)
    {
        return true;
    }
    //在方形范围内
    public bool WithinCube(int radius)
    {
        return true;
    }
    //在扇形范围内
    public bool WithinSector(float angle)
    {
        return true;
    }

}
