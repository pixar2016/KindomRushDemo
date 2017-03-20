using System;
using System.Collections.Generic;

public class TowerAtk : StateBase
{
    public AttackTowerInfo towerInfo;

    public TowerAtk(AttackTowerInfo _towerInfo)
    {
        towerInfo = _towerInfo;
    }

    public void SetParam(params object[] args)
    {

    }

    public void EnterExcute()
    {
        towerInfo.StartAttack();
        towerInfo.attackSkill.eventDispatcher.Register("SkillEnd", SkillEnd);
    }

    public void SkillEnd(object[] param)
    {
        CharacterInfo attackCharInfo = towerInfo.GetAttackInfo();
        if (attackCharInfo.IsDead())
        {
            attackCharInfo.ChangeState("die");
            towerInfo.ChangeState("idle");
        }
        else
        {
            towerInfo.ChangeState("attack");
        }
    }

    public void Excute()
    {

    }

    public void ExitExcute()
    {
        towerInfo.attackSkill.eventDispatcher.Remove("SkillEnd", SkillEnd);
    }
}

