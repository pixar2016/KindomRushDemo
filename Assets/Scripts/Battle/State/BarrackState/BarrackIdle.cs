using System;
using System.Collections.Generic;
using UnityEngine;

//兵营空闲状态
public class BarrackIdle : StateBase
{
    public BarrackTowerInfo barrackInfo;

    public float intervalTime;
    public float curTime;

    public BarrackIdle(BarrackTowerInfo _barrackInfo)
    {
        barrackInfo = _barrackInfo;
        intervalTime = 10;
        curTime = 0;
    }

    public void SetParam(params object[] args)
    {

    }

    public void EnterExcute()
    {
        barrackInfo.DoAction("idle");
    }

    public void RunAI()
    {
        if (!barrackInfo.IsSoliderFull())
        {
            barrackInfo.ChangeState("start");
        }
    }

    public void Excute()
    {
        curTime += Time.deltaTime;
        if (curTime >= intervalTime)
        {
            curTime = 0;
            RunAI();
        }
    }

    public void ExitExcute()
    {

    }
}

