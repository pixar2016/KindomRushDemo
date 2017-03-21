using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hero;

public class OpenSpaceView : TowerView
{
    public SpriteImage towerBase;
    public OpenSpaceView(OpenSpaceInfo _openSpaceInfo)
    {
        this.towerInfo = _openSpaceInfo;
    }

    public override void LoadModel()
    {
        towerAsset = GameLoader.Instance.LoadAssetSync("Resources/Prefabs/OpenSpace.prefab");
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
        //加载空地图片
        if (towerObj.GetComponent<SpriteImage>() != null)
        {
            towerBase = towerObj.GetComponent<SpriteImage>();
        }
        else
        {
            towerBase = towerObj.AddComponent<SpriteImage>();
        }
        towerBase.OnInit(towerInfo.towerBase);
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

