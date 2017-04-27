using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCache
{
    private static AnimationCache instance = null;

    public Dictionary<string, MyAnimation> _animations;
    private AnimationCache()
    {
        _animations = new Dictionary<string, MyAnimation>();
    }

    public static AnimationCache getInstance()
    {
        if (instance == null)
        {
            instance = new AnimationCache();
        }
        return instance;
    }
    //根据动画名得到动画
    public MyAnimation getAnimation(string name)
    {
        //Debug.Log("name = " + name);
        //foreach (string s in _animations.Keys)
        //{
        //    Debug.Log(s);
        //}
        if (_animations.ContainsKey(name))
        {
            return _animations[name];
        }
        //Debug.Log("null");
        return null;
    }
    //创建一个包含多个动作的动画
    public void addAnimation(MeshAnimation animation, string name, string actionName)
    {

        //若该生物已创建，则只需要在对应动作增加
        if (_animations.ContainsKey(name))
        {
            MyAnimation anim = _animations[name];
            anim.AddAnimation(animation, actionName);
        }
        //若该生物未创建，创建该生物，并增加对应动作
        else
        {
            MyAnimation anim = new MyAnimation();
            anim.AddAnimation(animation, actionName);
            _animations.Add(name, anim);
        }
    }
    //创建一个不包含动作的动画
    public void addAnimation(MeshAnimation animation, string name)
    {
        if (_animations.ContainsKey(name))
        {
            MyAnimation anim = _animations[name];
            anim.AddAnimation(animation);
        }
        else
        {
            MyAnimation anim = new MyAnimation();
            anim.AddAnimation(animation);
            _animations.Add(name, anim);
        }
    }
    public MeshAnimation createAnimation(string imageName, int start, int end, float delay, bool isLoop = true)
    {
        List<SpriteFrame> animFrames = new List<SpriteFrame>();
        if (start <= 0 && end <= 0)
        {
            SpriteFrame frame = SpriteFrameCache.getInstance().getSpriteFrame(imageName + ".png");
            if (frame != null)
            {
                animFrames.Add(frame);
            }
        }
        else
        {
            for (int i = start; i <= end; i++)
            {
                SpriteFrame frame = SpriteFrameCache.getInstance().getSpriteFrame(imageName + i + ".png");
                if (frame != null)
                {
                    animFrames.Add(frame);
                }
            }
        }
        MeshAnimation anim = new MeshAnimation();
        anim.createWithSpriteFrames(animFrames, delay, isLoop);
        return anim;
    }
}
