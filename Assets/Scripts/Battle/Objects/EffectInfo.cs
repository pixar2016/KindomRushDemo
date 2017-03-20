using System;
using System.Collections.Generic;
using UnityEngine;

//特效基类
public class EffectInfo
{
    //索引ID
    public int Id;
    //特效ID
    public int effectId;
    //特效名称
    public string effectName;
    //特效位置
    public Vector3 pos;
    //特效已产生的时间
    public float effectTime;
    //特效播放一次时间
    public float effectMaxTime;
    //特效是否循环播放 true-是 false-否
    public bool loop;
    public EffectInfo(int effectIndexId, int effId)
    {
        Id = effectIndexId;
        effectId = effId;
        effectName = "mageTowerBullet";
        effectTime = 0;
        effectMaxTime = 1.0f;
        loop = false;
    }

    public Vector3 GetPosition()
    {
        return pos;
    }
    public void SetPosition(float x, float y, float z)
    {
        pos = new Vector3(x, y, z);
    }
    public void Update()
    {
        if (loop)
        {
            return;
        }
        effectTime += Time.deltaTime;
        if (effectTime >= effectMaxTime)
        {
            effectTime = 0;
            Debug.Log("RemoveEffect");
            EntityManager.getInstance().RemoveEffect(this.Id);
        }
    }

    public void Release()
    {

    }
}

