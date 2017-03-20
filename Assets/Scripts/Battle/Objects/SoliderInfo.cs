using UnityEngine;
using System.Collections.Generic;


public class SoliderInfo : CharacterInfo
{
    //攻击目标
    private CharacterInfo attackCharInfo;
    //关联的兵营
    public BarrackTowerInfo barrackTower;
    //兵营中的Id
    public int barrackSoliderId;
    //兵营停留位置
    public Vector3 barrackSoliderPos;

    //属性
    public D_Creature creatureData;
    public int hp;
    public int hpMax;
    public int attackSpeed;
    public int attackDamage;
    public int defenceType;
    //宽度
    public float width;
    //高度
    public float height;

    public StateMachine soliderStateMachine;
    public SoliderAtk soliderAtk;
    public SoliderDead soliderDead;
    public SoliderIdle soliderIdle;
    public SoliderMove soliderMove;
    public SoliderReady soliderReady;

    public SkillInfo attackSkill;
    public SoliderInfo(int soliderIndexId, int soliderId)
    {
        Id = soliderIndexId;
        charId = soliderId;
        creatureData = J_Creature.GetData(charId);
        charName = creatureData._modelName;

        soliderStateMachine = new StateMachine();
        soliderAtk = new SoliderAtk(this);
        soliderDead = new SoliderDead(this);
        soliderIdle = new SoliderIdle(this);
        soliderMove = new SoliderMove(this);
        soliderReady = new SoliderReady(this);

        hpMax = creatureData._hp;
        hp = hpMax;
        attackSpeed = creatureData._attackSpeed;
        attackDamage = creatureData._attackDamage;
        defenceType = creatureData._defenceType;
        attackSkill = SkillManager.getInstance().AddSkill(1, this);

        attackTime = AnimationCache.getInstance().getAnimation(charName).getMeshAnimation("attack").getAnimTime();
    }

    //关联兵营、兵营Id和停留位置
    public void SetTowerInfo(BarrackTowerInfo tower, int id, Vector3 pos)
    {
        barrackTower = tower;
        barrackSoliderId = id;
        barrackSoliderPos = pos;
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
    public Vector3 GetAttackMovePos()
    {
        Vector3 atkPos = attackCharInfo.GetPosition();
        Vector3 movePos;
        Vector3 atkRot = attackCharInfo.GetRotation();
        //攻击目标向左
        if (atkRot.y > 0)
        {
            movePos = atkPos - new Vector3(30f, 0, 0); 
        }
        //攻击目标向右
        else
        {
            movePos = atkPos + new Vector3(30f, 0, 0);
        }
        return movePos;
    }
    public Vector3 GetBarrackPos()
    {
        return barrackSoliderPos;
    }

    //执行AI
    public CharacterInfo RunAI()
    {
        //Debug.Log("RunAI");
        return FindMonster();
    }
    //寻找怪物
    MonsterInfo FindMonster()
    {
        List<MonsterInfo> monsterList = EntityManager.getInstance().GetMonsterInfo();
        foreach (MonsterInfo monster in monsterList)
        {
            if (monster.GetAttackInfo() == null && Vector3.Distance(this.GetPosition(), monster.GetPosition()) <= 150)
            {
                return monster;
            }
        }
        return null;
    }

    public override void ChangeState(string _state, params object[] args)
    {
        if (_state == "attack")
        {
            soliderStateMachine.ChangeState(soliderAtk, args);
        }
        else if (_state == "die")
        {
            soliderStateMachine.ChangeState(soliderDead, args);
        }
        else if (_state == "idle")
        {
            soliderStateMachine.ChangeState(soliderIdle, args);
        }
        else if (_state == "move")
        {
            soliderStateMachine.ChangeState(soliderMove, args);
        }
        else if(_state == "ready")
        {
            soliderStateMachine.ChangeState(soliderReady, args);
        }
    }

    public override void SetState(string _state)
    {
        if (_state == "attack")
        {
            soliderStateMachine.SetCurrentState(soliderAtk);
        }
        else if (_state == "die")
        {
            soliderStateMachine.SetCurrentState(soliderDead);
        }
        else if (_state == "idle")
        {
            soliderStateMachine.SetCurrentState(soliderIdle);
        }
        else if (_state == "move")
        {
            soliderStateMachine.SetCurrentState(soliderMove);
        }
        else if (_state == "ready")
        {
            soliderStateMachine.SetCurrentState(soliderReady);
        }
    }

    public override void StartAttack()
    {
        SkillManager.getInstance().StartSkill(attackSkill);
    }

    public override void StartSkill(int skillId)
    {

    }

    //对目标造成普通攻击伤害
    public override void Hurt()
    {
        if (attackCharInfo != null)
        {
            attackCharInfo.ReduceHP(attackDamage);
        }
    }

    public override void ReduceHP(int losehp)
    {
        //Debug.Log("losehp = "+losehp);
        hp -= losehp;
        //Debug.Log(this.charName + " hp = " + hp);
    }

    //向上走
    public override void RunUp()
    {
        SetRotation(0, 0, 0);
        DoAction("run1");
    }
    //向下走
    public override void RunDown()
    {
        SetRotation(0, 0, 180);
        DoAction("run1");
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

    public override bool IsDead()
    {
        return hp <= 0;
    }

    public void Update()
    {
        soliderStateMachine.Excute();
    }

    public override void Release()
    {
        
    }
    //重置信息，重置血量等信息
    public void Reset()
    {

    }
}
