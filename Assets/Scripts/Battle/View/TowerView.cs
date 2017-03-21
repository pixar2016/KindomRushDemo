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
        ClickInfo preClick = GameManager.getInstance().curClickInfo;
        //如果上次点击 点中可交互物体，并且类型相同，Id不同，即点中其他塔，立刻关闭UI
        if (preClick != null && preClick.clickType == curClick.clickType && preClick.Id != curClick.Id)
        {
            UiManager.Instance.CloseUIById(UIDefine.eSelectPanel);
            UiManager.Instance.OpenUI(UIDefine.eSelectPanel, towerInfo);
        }
        else if (UiManager.Instance.HasOpenUI(UIDefine.eSelectPanel))
        {
            UiManager.Instance.CloseUIById(UIDefine.eSelectPanel);
        }
        else
        {
            UiManager.Instance.OpenUI(UIDefine.eSelectPanel, towerInfo);
        }
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

