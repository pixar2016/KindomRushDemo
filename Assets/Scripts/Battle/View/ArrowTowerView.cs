using System;
using System.Collections.Generic;
using Hero;
using UnityEngine;

public class ArrowTowerView : TowerView
{
    public SpriteImage towerBase;
    public Animate shooter1;
    public Animate shooter2;
    //轮到哪个弓手
    public int shooterNum;
    public ArrowTowerView(AttackTowerInfo towerInfo)
    {
        this.towerInfo = towerInfo;
        this.towerInfo.eventDispatcher.Register("DoAction", DoAction);
        this.shooterNum = 1;
    }

    public override void LoadModel()
    {
        towerObj = GameLoader.Instance.LoadAssetSync("Resources/Prefabs/ArrowTower.prefab").GameObjectAsset;
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
        if (towerObj.GetComponent<SpriteImage>() != null)
        {
            towerBase = towerObj.GetComponent<SpriteImage>();
        }
        else
        {
            towerBase = towerObj.AddComponent<SpriteImage>();
        }
        towerBase.OnInit(towerInfo.towerBase);
        //加载射手1
        GameObject shooterObj1 = towerObj.transform.Find("ArrowShooter1").gameObject;
        if (shooterObj1.GetComponent<Animate>() != null)
        {
            shooter1 = shooterObj1.GetComponent<Animate>();
        }
        else
        {
            shooter1 = shooterObj1.AddComponent<Animate>();
        }
        shooter1.OnInit(towerInfo.shooter);
        shooter1.startAnimation("idle");
        //加载射手2
        GameObject shooterObj2 = towerObj.transform.Find("ArrowShooter2").gameObject;
        if (shooterObj2.GetComponent<Animate>() != null)
        {
            shooter2 = shooterObj2.GetComponent<Animate>();
        }
        else
        {
            shooter2 = shooterObj2.AddComponent<Animate>();
        }

        //根据塔基座大小增加碰撞盒
        BoxCollider collider;
        if (towerObj.GetComponent<BoxCollider>() == null)
        {
            collider = towerObj.AddComponent<BoxCollider>();
            collider.size = new Vector3(towerBase.width, towerBase.height, 0.2f);
        }
        else
        {
            collider = towerObj.GetComponent<BoxCollider>();
            collider.size = new Vector3(towerBase.width, towerBase.height, 0.2f);
        }
        shooter2.OnInit(towerInfo.shooter);
        shooter2.startAnimation("idle");
    }

    public override void DoAction(object[] data)
    {
        string actionName = data[0].ToString();
        if (actionName == "attack")
        {
            Attack();
        }
        else
        {
            shooter1.startAnimation(actionName);
            shooter2.startAnimation(actionName);
        }
    }

    private void Attack()
    {
        if (shooterNum == 1)
        {
            shooterNum = 2;
            shooter1.startAnimation("attack");
        }
        else
        {
            shooterNum = 1;
            shooter2.startAnimation("attack");
        }
    }
    //被点击
    public override void FingerDown(ClickInfo curClick)
    {
        Debug.Log("FingerDown");
        //ClickInfo preClick = GameManager.getInstance().curClickInfo;
        //如果上次点击 点中可交互物体，并且类型相同，Id不同，即点中其他塔，立刻关闭UI
        //if (preClick != null && preClick.clickType == curClick.clickType && preClick.Id != curClick.Id)
        //{
        //    UiManager.Instance.CloseUIById()
        //}
        if (UiManager.Instance.HasOpenUI(UIDefine.eSelectPanel))
        {
            UiManager.Instance.CloseUIById(UIDefine.eSelectPanel);
        }
        else
        {
            UiManager.Instance.OpenUI(UIDefine.eSelectPanel, towerInfo);
        }
    }

    public override void Update()
    {
        this.towerObj.transform.position = this.towerInfo.GetPosition();
    }
}

