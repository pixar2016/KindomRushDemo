﻿using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using LitJson;
using Hero;
using CoreLib.Timer;
public class Test1 : MonoBehaviour {

    Animate animate;

    MonsterInfo charInfo;
    SoliderInfo soliderInfo;
    List<SoliderInfo> soliderList;

    CTimerSystem m_TimerSystem = null;
	// Use this for initialization
    void Awake()
    {
        GameLoader loader = GameObject.Find("GameLoader").GetComponent<GameLoader>();
        loader.Initialize();
    }
	void Start () {
        EntityManager.getInstance();
        EntityViewManager.getInstance();
        
        //DataPreLoader.getInstance().LoadData("Monsters");
        //根据图片信息txt将图片里帧信息分离出来
        //SpriteFrameCache.getInstance().addSpriteFrameFromFile("Resources/Config/monster1.txt");
        SpriteFrameCache.getInstance().addSpriteFrameFromFile("Resources/Config/Monster.txt");
        SpriteFrameCache.getInstance().addSpriteFrameFromFile("Resources/Config/Solider.txt");
        SpriteFrameCache.getInstance().addSpriteFrameFromFile("Resources/Config/Tower1.txt");
        SpriteFrameCache.getInstance().addSpriteFrameFromFile("Resources/Config/Tower2.txt");
        AnimationCache animCache = AnimationCache.getInstance();
        //第一个参数 createAnimation（参数1-图片名字 参数2-图片开始序号 参数3-图片结束序号 参数4-动画间隔 参数5-是否循环）
        //第二个参数 动画名称
        //第三个参数 动作名称
        animCache.addAnimation(animCache.createAnimation("wulf_", 19, 19, 0.1f, true), "wulf", "idle");
        animCache.addAnimation(animCache.createAnimation("wulf_", 1, 8, 0.1f, true), "wulf", "run1");
        animCache.addAnimation(animCache.createAnimation("wulf_", 9, 18, 0.1f, true), "wulf", "run2");
        animCache.addAnimation(animCache.createAnimation("wulf_", 19, 28, 0.1f, false), "wulf", "attack");
        animCache.addAnimation(animCache.createAnimation("wulf_", 29, 35, 0.1f, false), "wulf", "stuck_start");
        animCache.addAnimation(animCache.createAnimation("wulf_", 35, 40, 0.1f, false), "wulf", "stuck_end");
        animCache.addAnimation(animCache.createAnimation("wulf_", 41, 49, 0.1f, false), "wulf", "die");

        animCache.addAnimation(animCache.createAnimation("knight_", 1, 1, 0.1f, true), "knight", "idle");
        animCache.addAnimation(animCache.createAnimation("knight_", 2, 6, 0.1f, true), "knight", "run1");
        animCache.addAnimation(animCache.createAnimation("knight_", 7, 10, 0.1f, false), "knight", "attack");
        animCache.addAnimation(animCache.createAnimation("knight_", 11, 15, 0.1f, false), "knight", "attack1");
        animCache.addAnimation(animCache.createAnimation("knight_", 16, 33, 0.1f, false), "knight", "skill1");
        animCache.addAnimation(animCache.createAnimation("knight_", 34, 62, 0.1f, false), "knight", "skill2");
        animCache.addAnimation(animCache.createAnimation("knight_", 63, 69, 0.1f, false), "knight", "die");
        //箭塔
        animCache.addAnimation(animCache.createAnimation("arrowTowerlv1Shooter_", 1, 1, 0.1f, false), "arrowTowerlv1Shooter", "idle");
        animCache.addAnimation(animCache.createAnimation("arrowTowerlv1Shooter_", 1, 5, 0.1f, false), "arrowTowerlv1Shooter", "attack");
        animCache.addAnimation(animCache.createAnimation("arrowTowerlv1Shooter_", 6, 10, 0.1f, false), "arrowTowerlv1Shooter", "attack1");
        animCache.addAnimation(animCache.createAnimation("arrowTowerlv2Shooter_", 1, 1, 0.1f, false), "arrowTowerlv2Shooter", "idle");
        animCache.addAnimation(animCache.createAnimation("arrowTowerlv2Shooter_", 1, 5, 0.1f, false), "arrowTowerlv2Shooter", "attack");
        animCache.addAnimation(animCache.createAnimation("arrowTowerlv2Shooter_", 6, 10, 0.1f, false), "arrowTowerlv2Shooter", "attack1");
        animCache.addAnimation(animCache.createAnimation("arrowTowerlv3Shooter_", 1, 1, 0.1f, false), "arrowTowerlv3Shooter", "idle");
        animCache.addAnimation(animCache.createAnimation("arrowTowerlv3Shooter_", 1, 5, 0.1f, false), "arrowTowerlv3Shooter", "attack");
        animCache.addAnimation(animCache.createAnimation("arrowTowerlv3Shooter_", 6, 10, 0.1f, false), "arrowTowerlv3Shooter", "attack1");
        //兵营
        animCache.addAnimation(animCache.createAnimation("barrackTowerlv1_", 1, 1, 0.1f, false), "barrackTowerlv1", "idle");
        animCache.addAnimation(animCache.createAnimation("barrackTowerlv1_", 2, 4, 0.1f, false), "barrackTowerlv1", "start");
        animCache.addAnimation(animCache.createAnimation("barrackTowerlv2_", 1, 1, 0.1f, false), "barrackTowerlv2", "idle");
        animCache.addAnimation(animCache.createAnimation("barrackTowerlv2_", 2, 4, 0.1f, false), "barrackTowerlv2", "start");
        animCache.addAnimation(animCache.createAnimation("barrackTowerlv3_", 1, 1, 0.1f, false), "barrackTowerlv3", "idle");
        animCache.addAnimation(animCache.createAnimation("barrackTowerlv3_", 2, 4, 0.1f, false), "barrackTowerlv3", "start");
        //魔法塔
        animCache.addAnimation(animCache.createAnimation("mageTowerlv1_", 1, 1, 0.1f, false), "mageTowerlv1", "idle");
        animCache.addAnimation(animCache.createAnimation("mageTowerlv1_", 2, 6, 0.1f, false), "mageTowerlv1", "attack");
        animCache.addAnimation(animCache.createAnimation("mageTowerlv2_", 1, 1, 0.1f, false), "mageTowerlv2", "idle");
        animCache.addAnimation(animCache.createAnimation("mageTowerlv2_", 2, 6, 0.1f, false), "mageTowerlv2", "attack");
        animCache.addAnimation(animCache.createAnimation("mageTowerlv3_", 1, 1, 0.1f, false), "mageTowerlv3", "idle");
        animCache.addAnimation(animCache.createAnimation("mageTowerlv3_", 2, 6, 0.1f, false), "mageTowerlv3", "attack");
        animCache.addAnimation(animCache.createAnimation("mageTowerShooter_", 1, 1, 0.1f, false), "mageTowerShooter", "idle");
        animCache.addAnimation(animCache.createAnimation("mageTowerShooter_", 2, 2, 0.1f, false), "mageTowerShooter", "idle1");
        animCache.addAnimation(animCache.createAnimation("mageTowerShooter_", 3, 11, 0.1f, false), "mageTowerShooter", "attack");
        animCache.addAnimation(animCache.createAnimation("mageTowerShooter_", 12, 21, 0.1f, false), "mageTowerShooter", "attack1");
        animCache.addAnimation(animCache.createAnimation("mageTowerNecroShooter_", 1, 1, 0.1f, false), "mageTowerNecroShooter", "idle");
        animCache.addAnimation(animCache.createAnimation("mageTowerNecroShooter_", 2, 2, 0.1f, false), "mageTowerNecroShooter", "idle1");
        animCache.addAnimation(animCache.createAnimation("mageTowerNecroShooter_", 3, 15, 0.1f, false), "mageTowerNecroShooter", "attack");
        animCache.addAnimation(animCache.createAnimation("mageTowerNecroShooter_", 16, 28, 0.1f, false), "mageTowerNecroShooter", "attack1");
        animCache.addAnimation(animCache.createAnimation("mageTowerNecroShooter_", 29, 31, 0.1f, false), "mageTowerNecroShooter", "skill");
        animCache.addAnimation(animCache.createAnimation("mageTowerNecroShooter_", 32, 34, 0.1f, false), "mageTowerNecroShooter", "skill1");
        animCache.addAnimation(animCache.createAnimation("mageTowerMasterShooter_", 1, 1, 0.1f, false), "mageTowerMasterShooter", "idle");
        animCache.addAnimation(animCache.createAnimation("mageTowerMasterShooter_", 2, 2, 0.1f, false), "mageTowerMasterShooter", "idle1");
        animCache.addAnimation(animCache.createAnimation("mageTowerMasterShooter_", 3, 4, 0.1f, false), "mageTowerMasterShooter", "attack");
        animCache.addAnimation(animCache.createAnimation("mageTowerMasterShooter_", 5, 6, 0.1f, false), "mageTowerMasterShooter", "attack1");
        //弓箭特效
        animCache.addAnimation(animCache.createAnimation("arrow_", 1, 1, 0.1f, true), "arrow");

        J_Map.LoadConfig();
        J_Creature.LoadConfig();
        J_SkillEvent.LoadConfig();
        J_Skill.LoadConfig();
        J_Tower.LoadConfig();

        GameObject uiroot = GameObject.Find("Canvas").gameObject;
        UiManager.Instance.Init(uiroot);

        //PathLoader.getInstance().LoadPath("level0");
        //PathInfo path = PathLoader.getInstance().GetPath("0");
        //path.PrintAllPoint();

        //charInfo = EntityManager.getInstance().AddMonster(10001);
        //charInfo.SetPosition(-1, 1, 0);
        //charInfo.ChangeState("move");
        BattleFingerEvent.getInstance();
        //GameManager.getInstance().LoadLevel(1);
        //GameManager.getInstance().StartGame();

        TowerInfo tower = EntityManager.getInstance().AddTower(1);
        tower.SetPosition(-180, -220, 0);
        TowerInfo tower1 = EntityManager.getInstance().AddTower(100);
        tower1.SetPosition(-180, -140, 0);
        //tower.ChangeState("idle");
        
        //UiManager.Instance.OpenUI(UIDefine.eSelectPanel);
        //TowerInfo tower = EntityManager.getInstance().AddTower(16);
        //tower.ChangeState("start");
        //CharacterInfo cc = new SoliderInfo(1, 10001);
        //if (cc.GetType().ToString() == "SoliderInfo")
        //{
        //    Debug.Log("111111111111111111");
        //}

        //Animate charAnim;
        //GameObject charObj = GameLoader.Instance.LoadAssetSync("Resources/Prefabs/fly.prefab").GameObjectAsset;
        //if (charObj.GetComponent<Animate>() != null)
        //{
        //    charAnim = charObj.GetComponent<Animate>();
        //}
        //else
        //{
        //    charAnim = charObj.AddComponent<Animate>();
        //}
        //charAnim.OnInit(AnimationCache.getInstance().getAnimation("mageTowerBullet"));
        //charAnim.startAnimation();
    }

    public void StartAI()
    {
        Debug.Log("StartAI");
        if (null == m_TimerSystem)
        {
            m_TimerSystem = new CTimerSystem();
            m_TimerSystem.Create();

        }
        m_TimerSystem.CreateTimer(1, 1000, UpdateMonsterAI);
    }

    public void UpdateMonsterAI(uint nTimeID)
    {
        Debug.Log("UpdateMonsterAI");
        foreach (SoliderInfo solider in soliderList)
        {
            if (!solider.IsDead())
            {
                solider.RunAI();
            }
        }
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.W))
        {
            charInfo.DoAction("stuck_start");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            charInfo.DoAction("stuck_end");
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            charInfo.ChangeState("attack");
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            charInfo.ChangeState("move");
        }
        EntityManager.getInstance().Update();
        EntityViewManager.getInstance().Update();
        TriggerManager.getInstance().Update(Time.deltaTime);
        if (null != m_TimerSystem)
        {
            m_TimerSystem.UpdateTimer();
        }
        GameManager.getInstance().Update();
//        animate.OnUpdate(Time.deltaTime);
	}
}
