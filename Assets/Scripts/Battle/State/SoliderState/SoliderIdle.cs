using System;
using System.Collections.Generic;
using UnityEngine;

public class SoliderIdle : StateBase
{
    public SoliderInfo soliderInfo;
    //运行AI的时间间隔
    public float intervalTime;
    //已运行多长时间
    public float curTime;
    public SoliderIdle(SoliderInfo _soliderInfo)
    {
        soliderInfo = _soliderInfo;
        intervalTime = 1;
        curTime = 0;
    }

    public void SetParam(params object[] args)
    {

    }

    public void EnterExcute()
    {
        soliderInfo.DoAction("idle");
    }

    public void RunAI()
    {
        CharacterInfo targetInfo = soliderInfo.RunAI();
        //若未找到目标，继续空闲
        if (targetInfo == null)
        {
            soliderInfo.ChangeState("idle");
        }
        //若找到目标，目标停住，并进入移动状态
        else
        {
            //双方都输入攻击目标，均被占用，不再接受其他人的约战
            targetInfo.SetAttackInfo(soliderInfo);
            targetInfo.ChangeState("idle", soliderInfo);
            soliderInfo.SetAttackInfo(targetInfo);
            soliderInfo.ChangeState("move", targetInfo);
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
