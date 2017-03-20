using System;
using System.Collections.Generic;
using UnityEngine;

public class CreatureMove : StateBase
{
    public MonsterInfo monsterInfo;
    public string name;
    public Vector3 curPos;
    public Vector3 targetPos;
    public float speed;
    public CreatureMove(MonsterInfo _monsterInfo)
    {
        name = "CreatureMove";
        monsterInfo = _monsterInfo;
    }

    public void SetParam(params object[] args)
    {

    }

    public void EnterExcute()
    {
        //Debug.Log("CreatureMove EnterExcute");
        curPos = monsterInfo.GetPosition();
        targetPos = monsterInfo.GetNextPoint();
        speed = monsterInfo.GetSpeed();
        if (targetPos.y > curPos.y && Math.Abs(targetPos.y - curPos.y) > Math.Abs(targetPos.x - curPos.x))
        {
            monsterInfo.RunUp();
        }
        else if (targetPos.x >= curPos.x)
        {
            monsterInfo.RunRight();
        }
        else
        {
            monsterInfo.RunLeft();
        }
    }

    public void Excute()
    {
        Vector3 pos = monsterInfo.GetPosition();
        float dis = Vector3.Distance(pos, targetPos);
        if (dis < speed * Time.deltaTime)
        {
            monsterInfo.SetPosition(targetPos.x, targetPos.y, targetPos.z);
            if (monsterInfo.ReachNextPoint())
            {
                monsterInfo.SetState("move");
            }
            else
            {
                monsterInfo.SetState("idle");
            }
            
        }
        else
        {
            pos = Vector3.MoveTowards(pos, targetPos, Time.deltaTime * speed);
            monsterInfo.SetPosition(pos.x, pos.y, pos.z);
        }
    }

    public void ExitExcute()
    {

    }
}
