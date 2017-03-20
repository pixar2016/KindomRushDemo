using System;
using System.Collections.Generic;
using UnityEngine;

//主要是组成单独的动画，将spriteFrameCache中缓存的相关的帧放到一起
public class MeshAnimation
{
    public List<SpriteFrame> frameList;
    //动作名，若没有，则为空
    public string actionName;
    public float delay;
    public bool active;
    //是否循环
    public bool loop;
    //动画帧所属图片
    public string texture;
    //动画帧数
    public int count;
    //动画总时间
    public float animTime;
    public MeshAnimation()
    {
        frameList = new List<SpriteFrame>();
    }
    //创建MeshAnimation
    public void createWithSpriteFrames(List<SpriteFrame> animFrames, float delays, bool isLoop)
    {
        delay = delays;
        loop = isLoop;
        if (animFrames.Count > 0)
        {
            texture = animFrames[0].textureName;
        }
        foreach (SpriteFrame frame in animFrames)
        {
            frameList.Add(frame);
        }
        count = animFrames.Count;
        animTime = count * delay;
    }
    //得到动画总时间
    public float getAnimTime()
    {
        return animTime;
    }
    //设置动画时间，保持帧数不变，更改帧间隔时间
    public void setAnimTime(float time)
    {
        animTime = time;
        delay = animTime / count;
    }
}

