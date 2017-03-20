using UnityEngine;
using System.Collections.Generic;


public class MonsterInfo : CharacterInfo
{
    //攻击目标
    private CharacterInfo attackCharInfo;
    //攻击需要移动的位置
    private Vector3 attackMovePos;

    //属性
    public D_Creature creatureData;
    public int hp;
    public int hpMax;
    public int attackSpeed;
    public int attackDamage;
    public int defenceType;

    public StateMachine creatureStateMachine;
    public CreatureAtk creatureAtk;
    public CreatureDead creatureDead;
    public CreatureIdle creatureIdle;
    public CreatureMove creatureMove;
    
    public SkillInfo attackSkill;
    //当前怪物行动路径
    public PathInfo pathInfo;
    //当前走到路径第几个点
    public int curPathNum;
    public MonsterInfo(int creatureIndexId, int creatureId, PathInfo _pathInfo)
    {
        Id = creatureIndexId;
        charId = creatureId;
        creatureData = J_Creature.GetData(charId);
        charName = creatureData._modelName;
        
        creatureStateMachine = new StateMachine();
        creatureAtk = new CreatureAtk(this);
        creatureDead = new CreatureDead(this);
        creatureIdle = new CreatureIdle(this);
        creatureMove = new CreatureMove(this);

        hpMax = creatureData._hp;
        hp = hpMax;
        attackSpeed = creatureData._attackSpeed;
        attackDamage = creatureData._attackDamage;
        defenceType = creatureData._defenceType;
        attackSkill = SkillManager.getInstance().AddSkill(1, this);
        pathInfo = _pathInfo;
        curPathNum = 0;
        //设置初始位置
        position = pathInfo.GetPoint(curPathNum);

        attackTime = AnimationCache.getInstance().getAnimation(charName).getMeshAnimation("attack").getAnimTime();

    }
    public bool ReachNextPoint()
    {
        if (curPathNum + 1 >= pathInfo.GetCount())
        {
            return false;
        }
        curPathNum++;
        return true;
    }
    public Vector3 GetNextPoint()
    {
        if (curPathNum + 1 >= pathInfo.GetCount())
        {
            return this.GetPosition();
        }
        return pathInfo.GetPoint(curPathNum + 1);
    }
    //设置攻击目标等信息
    public override void SetAttackInfo(CharacterInfo charInfo)
    {
        attackCharInfo = charInfo;
    }

    public override CharacterInfo GetAttackInfo()
    {
        return attackCharInfo;
    }

    public override void ChangeState(string _state, params object[] args)
    {
        if (_state == "attack")
        {
            creatureStateMachine.ChangeState(creatureAtk, args);
        }
        else if (_state == "die")
        {
            creatureStateMachine.ChangeState(creatureDead, args);
        }
        else if (_state == "idle")
        {
            creatureStateMachine.ChangeState(creatureIdle, args);
        }
        else if (_state == "move")
        {
            creatureStateMachine.ChangeState(creatureMove, args);
        }
    }

    public override void SetState(string _state)
    {
        if (_state == "attack")
        {
            creatureStateMachine.SetCurrentState(creatureAtk);
        }
        else if (_state == "die")
        {
            creatureStateMachine.SetCurrentState(creatureDead);
        }
        else if (_state == "idle")
        {
            creatureStateMachine.SetCurrentState(creatureIdle);
        }
        else if (_state == "move")
        {
            creatureStateMachine.SetCurrentState(creatureMove);
        }
    }

    //得到当前状态类的名字
    public string GetCurrentState()
    {
        if (creatureStateMachine.GetCurrentState() == null)
        {
            return "";
        }
        return creatureStateMachine.GetCurrentState().ToString();
    }

    public override void StartAttack()
    {
        SkillManager.getInstance().StartSkill(attackSkill);
    }

    public override void StartSkill(int skillId)
    {
        
    }
    //向上走
    public override void RunUp()
    {
        SetRotation(0, 0, 0);
        DoAction("run2");
    }
    //向下走
    public override void RunDown()
    {
        SetRotation(0, 0, 180);
        DoAction("run2");
    }
    //向右走
    public override void RunRight()
    {
        SetRotation(0, 0, 0);
        DoAction("run1");
    }
    //向左走
    public override void RunLeft()
    {
        SetRotation(0, 180, 0);
        DoAction("run1");
    }
    public float GetSpeed()
    {
        return 60;
    }
    //对目标造成普通攻击伤害
    public override void Hurt()
    {
        if (attackCharInfo != null)
        {
            attackCharInfo.ReduceHP(attackDamage);
            //Debug.Log(attackCharInfo.charName + "  " + attackCharInfo.hp);
        }
    }

    public override void ReduceHP(int losehp)
    {
        //Debug.Log("losehp = "+losehp);
        hp -= losehp;
        Debug.Log(this.charName + " hp = " + hp);
    }

    public override bool IsDead()
    {
        return hp <= 0;
    }

    public void Update()
    {
        creatureStateMachine.Excute();
    }
}
