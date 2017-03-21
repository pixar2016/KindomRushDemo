using System.Collections;
using System.Collections.Generic;
using Hero;
using UnityEngine;
public class MageTowerView : TowerView
{
    public Animate towerBase;
    public Animate shooter;
    public MageTowerView(AttackTowerInfo towerInfo)
    {
        this.towerInfo = towerInfo;
    }

    public override void LoadModel()
    {
        towerAsset = GameLoader.Instance.LoadAssetSync("Resources/Prefabs/MageTower.prefab");
        towerObj = towerAsset.GameObjectAsset;
        towerObj.transform.position = this.towerInfo.GetPosition();
        if (towerObj.GetComponent<ClickInfo>() == null)
        {
            ClickInfo clickInfo = towerObj.AddComponent<ClickInfo>();
            clickInfo.OnInit(ClickType.Tower, this.towerInfo.Id, FingerDown);
        }
        else
        {
            ClickInfo clickInfo = towerObj.GetComponent<ClickInfo>();
            clickInfo.OnInit(ClickType.Tower, this.towerInfo.Id, FingerDown);
        }
        //加载塔身图片
        if (towerObj.GetComponent<Animate>() != null)
        {
            towerBase = towerObj.GetComponent<Animate>();
        }
        else
        {
            towerBase = towerObj.AddComponent<Animate>();
        }
        towerBase.OnInit(towerInfo.towerBase);
        towerBase.startAnimation("idle");
        //加载射手
        GameObject shooterObj = towerObj.transform.Find("MageShooter").gameObject;
        if (shooterObj.GetComponent<Animate>() != null)
        {
            shooter = shooterObj.GetComponent<Animate>();
        }
        else
        {
            shooter = shooterObj.AddComponent<Animate>();
        }
        shooter.OnInit(towerInfo.shooter);
        shooter.startAnimation("idle");
    }

    public override void DoAction(object[] data)
    {
        string actionName = data[0].ToString();
        towerBase.startAnimation(actionName);
        shooter.startAnimation(actionName);
    }

    public override void FingerDown(ClickInfo curClick)
    {
        if (UiManager.Instance.HasOpenUI(UIDefine.eSelectPanel))
        {
            UiManager.Instance.CloseUIById(UIDefine.eSelectPanel);
        }
        else
        {
            UiManager.Instance.OpenUI(UIDefine.eSelectPanel, towerInfo);
        }
    }

    public override void Release()
    {
        GameLoader.Instance.UnLoadGameObject(towerAsset);
    }

    public override void Update()
    {
        this.towerObj.transform.position = this.towerInfo.GetPosition();
    }
}

