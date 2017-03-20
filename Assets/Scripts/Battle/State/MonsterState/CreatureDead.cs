using System;
using System.Collections.Generic;
using UnityEngine;

public class CreatureDead : StateBase
{
    public MonsterInfo monsterInfo;
    public float monsterDeadTime;
    public float curTime;

    public CreatureDead(MonsterInfo _monsterInfo)
    {
        monsterInfo = _monsterInfo;
        curTime = 0;
        monsterDeadTime = AnimationCache.getInstance().getAnimation(monsterInfo.charName).getMeshAnimation("die").getAnimTime();
    }

    public void SetParam(params object[] args)
    {

    }

    public void EnterExcute()
    {
        Debug.Log("CreatureDead EnterExcute");
        monsterInfo.DoAction("die");
    }

    public void Excute()
    {
        curTime += Time.deltaTime;
        if (curTime >= monsterDeadTime)
        {
            EntityManager.getInstance().RemoveMonster(monsterInfo.Id);
        }
    }

    public void ExitExcute()
    {

    }
}

