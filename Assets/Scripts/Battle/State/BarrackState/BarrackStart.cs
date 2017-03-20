using System;
using System.Collections.Generic;
using UnityEngine;

//兵营刷新兵种状态
public class BarrackStart : StateBase
{
    public BarrackTowerInfo barrackInfo;
    float curTime;
    public BarrackStart(BarrackTowerInfo _barrackInfo)
    {
        barrackInfo = _barrackInfo;
        curTime = 0;
    }

    public void SetParam(params object[] args)
    {

    }

    public void EnterExcute()
    {
        Debug.Log("BarrackInfo DoAction Start");
        barrackInfo.DoAction("start");
        curTime = 0;
        //补齐兵种
        barrackInfo.AddSolider();
    }

    public void Excute()
    {
        curTime += Time.deltaTime;
        //动画播放结束，转入空闲状态
        if (curTime >= barrackInfo.startTime)
        {
            curTime = 0;
            barrackInfo.ChangeState("idle");
        }
    }

    public void ExitExcute()
    {

    }
}

