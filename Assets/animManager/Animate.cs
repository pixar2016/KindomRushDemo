using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animate: MonoBehaviour 
{
    //动画名称，可以通过外部赋值
    public string animName;

    //动画组件 存储不同Mesh动画 用于提取不同的Mesh动画
	private MyAnimation anim;
    //通过动画帧信息 构造的动画Mesh
	private Mesh mesh;
    //当前动画播放到第几帧
	private int FrameNum;
    //动画帧时间间隔
	private float delay;
    //当前的Mesh动画
	private MeshAnimation curAnimation;
    //当前的动画名字
	private string curActionName;
    //动画是否循环
    private bool isLoop;
    //动画是否激活
	private bool active;
    //动画运行时间，用于控制动画帧时间间隔
	private float curTime;
    //当前Mesh动画总帧数
    private int curAnimationFrameNum;

	public void OnInit(MyAnimation myanim)
	{
		this.anim = myanim;
        this.mesh = new Mesh();
        ResetAnimateVariable("", false);
        //mesh关联
        gameObject.GetComponent<MeshFilter>().sharedMesh = this.mesh;
        //根据图片名字加载使用的材质
        gameObject.GetComponent<MeshRenderer>().sharedMaterial = SpriteFrameCache.getInstance().getSpriteTexture(anim.pictName);
    }

    public void OnInit(string animName = "")
    {
        if (animName == null || animName == "")
        {
            return;
        }
        this.animName = animName;
        this.anim = AnimationCache.getInstance().getAnimation(this.animName);
        this.mesh = new Mesh();
        ResetAnimateVariable("", false);
        //mesh关联
        gameObject.GetComponent<MeshFilter>().sharedMesh = this.mesh;
        //根据图片名字加载使用的材质
        gameObject.GetComponent<MeshRenderer>().sharedMaterial = SpriteFrameCache.getInstance().getSpriteTexture(anim.pictName);
    }
    public void ResetAnimateVariable(string actionName, bool active)
    {
        this.curActionName = actionName;
        this.curAnimation = anim.getMeshAnimation(curActionName);
        if (this.curAnimation == null)
        {
            this.delay = 0.1f;
            this.isLoop = true;
            this.curAnimationFrameNum = 1;
        }
        else
        {
            this.delay = curAnimation.delay;
            this.isLoop = curAnimation.loop;
            this.curAnimationFrameNum = curAnimation.frameList.Count;
        }
        this.curTime = 0;
        this.FrameNum = 0;
        this.active = active;
    }
    //重新设置某个动作的时长
    public void SetAnimateTime(string actionName, float time)
    {
        MeshAnimation temp = anim.getMeshAnimation(actionName);
        temp.setAnimTime(time);
    }
	public void Update()
	{
		if (!active)
		{
			return;
		}
		if (curAnimation != null)
		{
			curTime += Time.deltaTime;
			if (curTime < delay)
			{
				return;
			}
			curTime = 0;
            if (FrameNum >= this.curAnimationFrameNum)
			{
                if (isLoop)
                {
                    FrameNum = 0;
                }    
                else
                {
                    ResetAnimateVariable("", false);
                    active = false;
                    return;
                }
                    
			}
			GetMesh(curAnimation.frameList[FrameNum]);
			FrameNum++;
		}
	}

    public int GetFrameMaxNum()
    {
        return this.curAnimationFrameNum;
    }
    //开始一个动画，包含多个动作
    public void startAnimation(string actionName)
    {
        //若是相同的动作，不需要改变
        if (curActionName != actionName)
        {
            ResetAnimateVariable(actionName, true);
        }
    }
    //开始一个动画，只有一个动作
    public void startAnimation()
    {
        if (curActionName != "normal")
        {
            ResetAnimateVariable("normal", true);
        }
    }
    //停止动画
    public void stopAnimation()
    {
        ResetAnimateVariable("", false);
    }
    /// <summary>
    /// 利用动画帧信息，构造Mesh
    /// </summary>
    /// <param name="frame"></param>
    /// <returns></returns>
    public void GetMesh(SpriteFrame frame)
    {
        this.mesh.vertices = frame.vertices;
        this.mesh.triangles = frame.triangles;
        this.mesh.uv = frame.uv;
    }
}

