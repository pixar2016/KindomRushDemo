using System;
using System.Collections.Generic;
using Hero;
using UnityEngine;
public class EffectView
{
    public EffectInfo effectInfo;
    public ILoadAsset effectAsset;
    public GameObject effectObj;
    public EffectView(EffectInfo effectInfo)
    {
        this.effectInfo = effectInfo;
    }

    public void LoadModel()
    {
        Animate effectAnim;
        //SpriteImage sprite;
        effectAsset = GameLoader.Instance.LoadAssetSync("Resources/Prefabs/fly.prefab");
        effectObj = effectAsset.GameObjectAsset;
        //if (effectObj.GetComponent<SpriteImage>() != null)
        //{
        //    sprite = effectObj.GetComponent<SpriteImage>();
        //}
        //else
        //{
        //    sprite = effectObj.AddComponent<SpriteImage>();
        //}
        //sprite.OnInit("knight_3.png");
        if (effectObj.GetComponent<Animate>() != null)
        {
            effectAnim = effectObj.GetComponent<Animate>();
        }
        else
        {
            effectAnim = effectObj.AddComponent<Animate>();
        }
        effectAnim.OnInit(AnimationCache.getInstance().getAnimation(effectInfo.effectName));
        effectAnim.startAnimation();
    }

    public void Release()
    {
        GameLoader.Instance.UnLoadGameObject(effectAsset);
    }

    public void Update()
    {
        effectObj.transform.position = effectInfo.GetPosition();
    }
}

