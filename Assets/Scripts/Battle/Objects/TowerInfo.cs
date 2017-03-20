using System;
using System.Collections.Generic;
using UnityEngine;

public class TowerInfo : CharacterInfo
{
    public D_Tower towerData;
    public string towerBase;
    //没有就为空
    public string shooter;
    public int towerType;

    public TowerInfo()
    {

    }

    public virtual void ChangeState(string stateName, params object[] args)
    {

    }

    public virtual void Update()
    {

    }
}

