using System;
using System.Collections.Generic;
using UnityEngine;

public class TowerView
{
    public TowerInfo towerInfo;
    public ILoadAsset towerAsset;
    public GameObject towerObj;
    public TowerView()
    {
    }
    //被点击
    public virtual void FingerDown(ClickInfo curClick)
    {

    }

    public virtual void LoadModel()
    {

    }

    public virtual void DoAction(object[] data)
    {

    }

    public virtual void Update()
    {

    }

    public virtual void Release()
    {

    }
}

