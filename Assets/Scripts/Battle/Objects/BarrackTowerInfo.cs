using System;
using System.Collections.Generic;
using UnityEngine;

//兵营
public class BarrackTowerInfo : TowerInfo
{

    public Dictionary<int, SoliderInfo> soliderDict;
    //出兵标记点
    public Vector3 signPos;
    //兵营门开启动画时间
    public float startTime;

    public StateMachine towerStateMachine;
    public BarrackStart barrackStart;
    public BarrackIdle barrackIdle;

    public BarrackTowerInfo(int indexId, int barrackId)
    {
        this.Id = indexId;
        this.charId = barrackId;
        this.eventDispatcher = new MiniEventDispatcher();
        this.towerData = J_Tower.GetData(barrackId);
        this.towerBase = towerData._towerBase;
        this.shooter = towerData._Shooter;
        this.towerType = towerData._towerType;

        towerStateMachine = new StateMachine();
        barrackStart = new BarrackStart(this);
        barrackIdle = new BarrackIdle(this);
        soliderDict = new Dictionary<int, SoliderInfo>();

        signPos = new Vector3(0,0,0);
        startTime = AnimationCache.getInstance().getAnimation(this.towerBase).getMeshAnimation("start").getAnimTime();
        InitSoliderDict();
    }

    public void InitSoliderDict()
    {
        Vector3 []pos = new Vector3[4];
        pos[1] = new Vector3(signPos.x + 30, signPos.y + 40, signPos.z);
        pos[2] = new Vector3(signPos.x - 30, signPos.y + 40, signPos.z);
        pos[3] = new Vector3(signPos.x, signPos.y + 40, signPos.z);
        for (int i = 1; i <= 3; i++)
        {
            SoliderInfo solider = EntityManager.getInstance().AddSolider(50001);
            solider.SetTowerInfo(this, i, pos[i]);
            solider.ChangeState("ready");
            soliderDict.Add(i, solider);
        }
    }

    public void SetPosition(float x, float y, float z)
    {
        position = new Vector3(x, y, z);
    }

    public Vector3 GetPosition()
    {
        return position;
    }

    public override void ChangeState(string stateName, params object[] args)
    {
        if (stateName == "start")
        {
            towerStateMachine.ChangeState(barrackStart, args);
        }
        else if (stateName == "idle")
        {
            towerStateMachine.ChangeState(barrackIdle, args);
        }
    }

    public void AddSolider()
    {
        //若兵营现在兵种数量小于3，则补充到3个
        foreach (int key in soliderDict.Keys)
        {
            if (soliderDict[key].IsDead())
            {
                soliderDict[key].Reset();
                soliderDict[key].ChangeState("idle");
            }
        }
    }

    //是否兵营已经全部出阵
    public bool IsSoliderFull()
    {
        foreach (int key in soliderDict.Keys)
        {
            if (soliderDict[key].IsDead())
            {
                return false;
            }
        }
        return true;
    }

    public override void Update()
    {
        towerStateMachine.Excute();
    }
}

