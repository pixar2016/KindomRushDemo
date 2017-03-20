using System;
using System.Collections.Generic;
using UnityEngine;

public class SoliderMove : StateBase
{
    public SoliderInfo soliderInfo;
    public Vector3 curPos;
    public Vector3 targetPos;
    public float speed;
    public SoliderMove(SoliderInfo _soliderInfo)
    {
        soliderInfo = _soliderInfo;
    }

    public void SetParam(params object[] args)
    {

    }

    public void EnterExcute()
    {
        curPos = soliderInfo.GetPosition();
        targetPos = soliderInfo.GetAttackMovePos();
        speed = soliderInfo.GetSpeed();
        if (targetPos.y > curPos.y && Math.Abs(targetPos.y - curPos.y) > Math.Abs(targetPos.x - curPos.x))
        {
            soliderInfo.RunUp();
        }
        else if (targetPos.x >= curPos.x)
        {
            soliderInfo.RunRight();
        }
        else
        {
            soliderInfo.RunLeft();
        }
    }

    public void Excute()
    {
        Vector3 pos = soliderInfo.GetPosition();
        float dis = Vector3.Distance(pos, targetPos);
        if (dis < speed * Time.deltaTime)
        {
            soliderInfo.SetPosition(targetPos.x, targetPos.y, targetPos.z);
            CharacterInfo attackInfo = soliderInfo.GetAttackInfo();
            soliderInfo.SetAttackInfo(attackInfo);
            attackInfo.SetAttackInfo(soliderInfo);
            soliderInfo.ChangeState("attack", attackInfo);
            attackInfo.ChangeState("attack", soliderInfo);
        }
        else
        {
            pos = Vector3.MoveTowards(pos, targetPos, Time.deltaTime * speed);
            soliderInfo.SetPosition(pos.x, pos.y, pos.z);
        }
    }

    public void ExitExcute()
    {

    }
}
