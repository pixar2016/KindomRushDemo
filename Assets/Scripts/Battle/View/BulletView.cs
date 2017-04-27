using System;
using System.Collections.Generic;
using UnityEngine;
using Hero;
public class BulletView
{
    public BulletInfo bulletInfo;
    public ILoadAsset bulletAsset;
    public GameObject bulletObj;

    public BulletView(BulletInfo _bulletInfo)
    {
        bulletInfo = _bulletInfo;
    }

    public void LoadModel()
    {
        Animate bulletAnim;
        //SpriteImage sprite;
        bulletAsset = GameLoader.Instance.LoadAssetSync("Resources/Prefabs/fly.prefab");
        bulletObj = bulletAsset.GameObjectAsset;
        if (bulletObj.GetComponent<Animate>() != null)
        {
            bulletAnim = bulletObj.GetComponent<Animate>();
        }
        else
        {
            bulletAnim = bulletObj.AddComponent<Animate>();
        }
        Debug.Log(bulletInfo.bulletName);
        bulletAnim.OnInit(AnimationCache.getInstance().getAnimation(bulletInfo.bulletName));
        bulletAnim.startAnimation();
    }

    public void Release()
    {
        GameLoader.Instance.UnLoadGameObject(bulletAsset);
    }

    public void Update()
    {
        bulletObj.transform.position = bulletInfo.GetPosition();
    }
}

