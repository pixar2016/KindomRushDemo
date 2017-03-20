using UnityEngine;
using System.Collections;
using EventDispatcherSpace;

public class CharacterInfo
{
    //序列ID
    public int Id;
    public int charId;
    public string charName;
    public Vector3 position;
    public Vector3 rotation;
    public string actionName;

    //攻击间隔，兵营为出兵间隔
    public float attackTime;
    //用于广播事件
    public MiniEventDispatcher eventDispatcher;
    

    public CharacterInfo()
    {
        eventDispatcher = new MiniEventDispatcher();
        position = new Vector3(0, 0, 0);
        rotation = new Vector3(0, 0, 0);
    }

    public void SetPosition(float x, float y, float z)
    {
        position = new Vector3(x, y, z);
    }
    public Vector3 GetPosition()
    {
        return position;
    }

    public void SetRotation(float x, float y, float z)
    {
        rotation = new Vector3(x, y, z);
    }
    public Vector3 GetRotation()
    {
        return rotation;
    }

    public virtual bool IsDead()
    {
        return false;
    }

    public void DoAction(string actionName)
    {
        //Debug.Log("Dispatching: SampleEvent " + sampleEvent);
        this.actionName = actionName;
        this.eventDispatcher.Broadcast("DoAction", actionName);
    }

    public virtual void ChangeState(string _state, params object[] args)
    {

    }

    public virtual void SetState(string _state)
    {

    }
    //设置攻击目标等信息
    public virtual void SetAttackInfo(CharacterInfo charInfo)
    {
    }

    public virtual CharacterInfo GetAttackInfo()
    {
        return null;
    }

    //开始普通攻击
    public virtual void StartAttack()
    {

    }
    //开始主动技能或被动技能
    public virtual void StartSkill(int skillId)
    {

    }
    //向上走
    public virtual void RunUp()
    {
        DoAction("run1");
    }
    //向下走
    public virtual void RunDown()
    {
        DoAction("run1");
    }
    //向右走
    public virtual void RunRight()
    {
        DoAction("run1");
    }
    //向左走
    public virtual void RunLeft()
    {
        DoAction("run1");
    }
    //对目标造成普通攻击伤害
    public virtual void Hurt()
    {
    }

    public virtual void ReduceHP(int losehp)
    {
    }

    public virtual void Release()
    {

    }
}
