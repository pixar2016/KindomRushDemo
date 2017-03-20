using System;
using System.Collections.Generic;
using UnityEngine;

public class SoliderAtk : StateBase
{
    public SoliderInfo soliderInfo;
    //public float attackTime;
    //public float curTime;
    //普通攻击放到状态机里做，技能用SkillManager做，普通攻击不作为技能处理
    public SoliderAtk(SoliderInfo _soliderInfo)
    {
        soliderInfo = _soliderInfo;
        //attackTime = 0;
        //curTime = 0;
    }

    public void SetParam(params object[] args)
    {

    }

    public void EnterExcute()
    {
        soliderInfo.StartAttack();
        soliderInfo.attackSkill.eventDispatcher.Register("SkillEnd", SkillEnd);
        //attackTime = soliderInfo.attackTime;
        //curTime = 0;
    }

    public void SkillEnd(object[] param)
    {
        //Debug.Log("SkillEnd");
        CharacterInfo attackCharInfo = soliderInfo.GetAttackInfo();
        //如果目标死亡，回归空闲状态
        if (attackCharInfo.IsDead())
        {
            attackCharInfo.ChangeState("die");
            soliderInfo.ChangeState("ready");
        }
        //如果目标未死亡，继续攻击状态
        else
        {
            //Debug.Log("continue attack");
            soliderInfo.ChangeState("attack");
        }
    }

    public void Excute()
    {
        //curTime += Time.deltaTime;
        ////若完成一次攻击
        //if (curTime >= attackTime)
        //{
        //    //执行普通攻击计算伤害方法
        //    //若目标为空或者已死亡，返回停留位置
        //    //否则，继续攻击
        //    //可以在这里添加攻击后被动技能
        //    soliderInfo.SetState("attack");
        //}
    }

    public void ExitExcute()
    {
        soliderInfo.attackSkill.eventDispatcher.Remove("SkillEnd", SkillEnd);
    }
}

