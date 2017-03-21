using System;
using System.Collections.Generic;
using UnityEngine;

//攻击类型塔 箭塔、魔塔、炮塔，因为状态机一致分配到一类中
public class AttackTowerInfo : TowerInfo
{

    public CharacterInfo attackCharInfo;
    public int attackDamage;
    public int attackSpeed;
    public List<SkillInfo> attackSkillList;
    public SkillInfo attackSkill;

    public StateMachine towerStateMachine;
    public TowerAtk towerAtk;
    public TowerIdle towerIdle;
    public AttackTowerInfo(int indexId, int towerId)
    {
        this.Id = indexId;
        this.charId = towerId;
        this.eventDispatcher = new MiniEventDispatcher();
        this.towerData = J_Tower.GetData(towerId);
        this.towerBase = towerData._towerBase;
        this.shooter = towerData._Shooter;
        this.towerType = towerData._towerType;

        towerStateMachine = new StateMachine();
        towerAtk = new TowerAtk(this);
        towerIdle = new TowerIdle(this);

        attackSkillList = new List<SkillInfo>();
        attackSkillList.Add(GetSpaceSkillInfo());
        attackSkill = SkillManager.getInstance().AddSkill(2, this);
        //若有弓手，攻击时长为弓手攻击动作
        if (this.shooter != null)
        {
            attackTime = AnimationCache.getInstance().getAnimation(this.shooter).getMeshAnimation("attack").getAnimTime();
        }
        //若没有弓手，攻击时长为塔身攻击动作
        else
        {
            attackTime = AnimationCache.getInstance().getAnimation(this.towerBase).getMeshAnimation("attack").getAnimTime();
        }
    }

    public SkillInfo GetSpaceSkillInfo()
    {
        foreach (SkillInfo skill in attackSkillList)
        {
            if (!skill.GetActive())
            {
                return skill;
            }
        }
        return SkillManager.getInstance().AddSkill(2, this);
    }

    public void SetPosition(float x, float y, float z)
    {
        position = new Vector3(x, y, z);
    }

    public Vector3 GetPosition()
    {
        return position;
    }

    public override void SetAttackInfo(CharacterInfo charInfo)
    {
        attackCharInfo = charInfo;
    }

    public override CharacterInfo GetAttackInfo()
    {
        return attackCharInfo;
    }

    public override void StartAttack()
    {
        SkillManager.getInstance().StartSkill(attackSkill);
    }

    public override void StartSkill(int skillId)
    {

    }

    public CharacterInfo RunAI()
    {
        return FindMonster();
    }

    MonsterInfo FindMonster()
    {
        List<MonsterInfo> monsterList = EntityManager.getInstance().GetMonsterInfo();
        foreach (MonsterInfo monster in monsterList)
        {
            if (Vector3.Distance(this.GetPosition(), monster.GetPosition()) <= 1250)
            {
                return monster;
            } 
        }
        return null;
    }

    public override void ChangeState(string stateName, params object[] args)
    {
        if (stateName == "attack")
        {
            towerStateMachine.ChangeState(towerAtk, args);
        }
        else if (stateName == "idle")
        {
            towerStateMachine.ChangeState(towerIdle, args);
        }
    }

    public override void Update()
    {
        towerStateMachine.Excute();
    }
}

