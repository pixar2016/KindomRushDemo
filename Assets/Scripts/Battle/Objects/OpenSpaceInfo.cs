using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//空地类，可以建设所有塔
public class OpenSpaceInfo : TowerInfo
{

    public StateMachine stateMachine;
    public OpenSpaceConstructing openSpaceConstructing;

    public OpenSpaceInfo(int indexId, int spaceId)
    {
        this.Id = indexId;
        this.charId = spaceId;
        this.towerData = J_Tower.GetData(spaceId);
        this.towerBase = towerData._towerBase;
        this.towerType = towerData._towerType;
        stateMachine = new StateMachine();
        openSpaceConstructing = new OpenSpaceConstructing(this);
    }

    public override void ChangeState(string stateName, params object[] args)
    {
        if (stateName == "constructing")
        {
            stateMachine.ChangeState(openSpaceConstructing, args);
        }
    }
}

