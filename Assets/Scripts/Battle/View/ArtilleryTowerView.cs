using System.Collections;
using System.Collections.Generic;
using Hero;
using UnityEngine;

public class ArtilleryTowerView : TowerView
{
    public Animate towerBase;

    public ArtilleryTowerView(AttackTowerInfo towerInfo)
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
    }

    public override void DoAction(object[] data)
    {
        string actionName = data[0].ToString();
        towerBase.startAnimation(actionName);
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

