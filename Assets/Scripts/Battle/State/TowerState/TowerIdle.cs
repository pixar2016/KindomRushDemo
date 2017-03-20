using System;
using System.Collections.Generic;
using UnityEngine;

public class TowerIdle : StateBase
{
    public AttackTowerInfo towerInfo;

    public float intervalTime;

    public float curTime;

    public TowerIdle(AttackTowerInfo _towerInfo)
    {
        towerInfo = _towerInfo;
        intervalTime = 1;
        curTime = 0;
    }

    public void SetParam(params object[] args)
    {

    }

    public void EnterExcute()
    {
        Debug.Log("TowerIdle");
        towerInfo.DoAction("idle");
    }

    public void Attack()
    {
        CharacterInfo targetInfo = towerInfo.RunAI();
        if (targetInfo != null)
        {
            towerInfo.SetAttackInfo(targetInfo);
            towerInfo.ChangeState("attack", targetInfo);
        }
    }

    public void Excute()
    {
        curTime += Time.deltaTime;
        if (curTime >= intervalTime)
        {
            curTime = 0;
            Attack();
        }
    }

    public void ExitExcute()
    {

    }
}

