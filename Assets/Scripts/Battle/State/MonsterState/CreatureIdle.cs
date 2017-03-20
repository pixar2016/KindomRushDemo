using System;
using System.Collections.Generic;
using UnityEngine;

public class CreatureIdle : StateBase
{
    public MonsterInfo monsterInfo;
    public string name;

    public CreatureIdle(MonsterInfo _monsterInfo)
    {
        name = "CreatureIdle";
        monsterInfo = _monsterInfo;
    }

    public void SetParam(params object[] args)
    {

    }

    public void EnterExcute()
    {
        //Debug.Log("CreatureIdle EnterExcute");
        monsterInfo.DoAction("idle");
    }

    public void Excute()
    {

    }

    public void ExitExcute()
    {

    }
}

