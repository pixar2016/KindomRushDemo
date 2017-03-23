using System;
using System.Collections.Generic;
using UnityEngine;
public class TowerAtk : StateBase
{
    public AttackTowerInfo towerInfo;
    public float attackTime;
    public float curTime;
    public TowerAtk(AttackTowerInfo _towerInfo)
    {
        towerInfo = _towerInfo;
        attackTime = 0;
        curTime = 0;
    }

    public void SetParam(params object[] args)
    {

    }

    public void EnterExcute()
    {
        towerInfo.StartAttack();
        attackTime = towerInfo.attackTime;
        curTime = 0;
    }

    public void AttackEnd()
    {
        CharacterInfo attackCharInfo = towerInfo.GetAttackInfo();
        //若死亡或者超出了攻击范围，则回归待机重新寻找目标
        if (attackCharInfo.IsDead())
        {
            attackCharInfo.ChangeState("die");
            towerInfo.ChangeState("idle");
        }
        else if (!towerInfo.WithinRange(attackCharInfo))
        {
            towerInfo.ChangeState("idle");
        }
        else
        {
            towerInfo.ChangeState("attack");
        }
    }

    public void Excute()
    {
        curTime += Time.deltaTime;
        if (curTime >= attackTime)
        {
            AttackEnd();
        }
    }

    public void ExitExcute()
    {
    }
}

