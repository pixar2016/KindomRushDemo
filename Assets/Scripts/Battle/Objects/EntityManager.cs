using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
//using EventDispatcherSpace;
//管理所有实体，包括怪物，士兵，塔，特效动画等
public class EntityManager {

	private static EntityManager instance = null;
    //用于广播事件
    public MiniEventDispatcher eventDispatcher;
	public static EntityManager getInstance()
	{
		if (instance == null)
		{
			instance = new EntityManager();
		}
		return instance;
	}

    public Dictionary<int, MonsterInfo> monsters;
    public Dictionary<int, SoliderInfo> soliders;
    public Dictionary<int, EffectInfo> effects;
    public Dictionary<int, BulletInfo> bullets;
    public Dictionary<int, TowerInfo> towers;
    //回收list
    public List<int> monsterDelList;
    public List<int> soliderDelList;
    public List<int> effectDelList;
    public List<int> bulletDelList;
    public List<int> towerDelList;
    //兵种序列ID
    public int monsterIndexId;
    public int soliderIndexId;
    public int effectIndexId;
    public int bulletIndexId;
    public int towerIndexId;
	private EntityManager()
	{
        monsters = new Dictionary<int, MonsterInfo>();
        soliders = new Dictionary<int, SoliderInfo>();
        effects = new Dictionary<int, EffectInfo>();
        bullets = new Dictionary<int, BulletInfo>();
        towers = new Dictionary<int, TowerInfo>();
        eventDispatcher = new MiniEventDispatcher();
        monsterDelList = new List<int>();
        soliderDelList = new List<int>();
        effectDelList = new List<int>();
        bulletDelList = new List<int>();
        towerDelList = new List<int>();
        monsterIndexId = 0;
        soliderIndexId = 0;
        effectIndexId = 0;
        bulletIndexId = 0;
        towerIndexId = 0;
	}

    public MonsterInfo AddMonster(int monsterId, PathInfo pathInfo)
    {
        monsterIndexId += 1;
        MonsterInfo charInfo = new MonsterInfo(monsterIndexId, monsterId, pathInfo);
        monsters.Add(monsterIndexId, charInfo);
        //Debug.Log(DataPreLoader.getInstance().GetData("Monsters")[creatureId]["AttackValue"]);
        this.eventDispatcher.Broadcast("AddMonster", charInfo);
        //EntityViewManager.getInstance().AddCreature(charInfo);
        return charInfo;
    }

    public SoliderInfo AddSolider(int soliderId)
    {
        soliderIndexId += 1;
        SoliderInfo charInfo = new SoliderInfo(soliderIndexId, soliderId);
        soliders.Add(soliderIndexId, charInfo);
        this.eventDispatcher.Broadcast("AddSolider", charInfo);
        return charInfo;
    }
    //添加静态特效
    public EffectInfo AddEffect(int effectId, int effectType)
    {
        effectIndexId += 1;
        EffectInfo effectInfo = new EffectInfo(effectIndexId, effectId);      
        effects.Add(effectIndexId, effectInfo);
        this.eventDispatcher.Broadcast("AddEffect", effectInfo);
        return effectInfo;
    }
    //添加子弹
    public BulletInfo AddBullet(int effectId, CharacterInfo charInfo, Vector3 startPos, Vector3 endPos, float speed, int triggerGroupId = 0)
    {
        bulletIndexId += 1;
        BulletInfo effectInfo = new BulletInfo(bulletIndexId, effectId, charInfo, startPos, endPos, speed, triggerGroupId);
        bullets.Add(bulletIndexId, effectInfo);
        this.eventDispatcher.Broadcast("AddBullet", effectInfo);
        return effectInfo;
    }
    //添加防御塔
    public TowerInfo AddTower(int towerId)
    {
        towerIndexId += 1;
        int towerType = J_Tower.GetData(towerId)._towerType;
        //增加兵营
        if (towerType == 4)
        {
            BarrackTowerInfo towerInfo = new BarrackTowerInfo(towerIndexId, towerId);
            this.eventDispatcher.Broadcast("AddTower", towerInfo);
            towers.Add(towerIndexId, towerInfo);
            return towerInfo;
        }
        //增加空地
        else if (towerType == 5)
        {
            OpenSpaceInfo spaceInfo = new OpenSpaceInfo(towerIndexId, towerId);
            this.eventDispatcher.Broadcast("AddTower", spaceInfo);
            towers.Add(towerIndexId, spaceInfo);
            return spaceInfo;
        }
        //增加攻击塔
        else
        {
            AttackTowerInfo towerInfo = new AttackTowerInfo(towerIndexId, towerId);
            this.eventDispatcher.Broadcast("AddTower", towerInfo);
            towers.Add(towerIndexId, towerInfo);
            return towerInfo;
        }
    }
    public void RemoveTower(int towerId)
    {
        foreach (int key in towers.Keys)
        {
            if (towers[key].Id == towerId)
            {
                towerDelList.Add(key);
                this.eventDispatcher.Broadcast("RemoveTower", towerId);
            }
        }
    }
    public void RemoveMonster(int monsterId)
    {
        Debug.Log("RemoveMonster!");
        foreach (int key in monsters.Keys)
        {
            if (monsters[key].Id == monsterId)
            {
                monsterDelList.Add(key);
                this.eventDispatcher.Broadcast("RemoveMonster", monsterId);
            }
        }
    }

    public void RemoveSolider(int soliderId)
    {
        foreach (int key in soliders.Keys)
        {
            if (soliders[key].Id == soliderId)
            {
                soliderDelList.Add(key);
                this.eventDispatcher.Broadcast("RemoveSolider", soliderId);
            }
        }
    }

    public void RemoveEffect(int effectId)
    {
        foreach (int key in effects.Keys)
        {
            if (effects[key].Id == effectId)
            {
                effectDelList.Add(key);
                this.eventDispatcher.Broadcast("RemoveEffect", effectId);
            }
        }
    }

    public void RemoveBullet(int bulletId)
    {
        foreach (int key in bullets.Keys)
        {
            if (bullets[key].Id == bulletId)
            {
                bulletDelList.Add(key);
                this.eventDispatcher.Broadcast("RemoveBullet", bulletId);
            }
        }
    }

    public void Update()
    {
        foreach (int key in monsters.Keys)
        {
            monsters[key].Update();
        }
        foreach (int key in soliders.Keys)
        {
            soliders[key].Update();
        }
        foreach (int key in effects.Keys)
        {
            effects[key].Update();
        }
        foreach (int key in bullets.Keys)
        {
            bullets[key].Update();
        }
        foreach (int key in towers.Keys)
        {
            towers[key].Update();
        }
        CollectDelInfo();
    }
    //回收要删除的实体
    public void CollectDelInfo()
    {
        if (monsterDelList.Count > 0)
        {
            foreach (int indexId in monsterDelList)
            {
                monsters.Remove(indexId);
            }
            monsterDelList.Clear();
        }
        if (soliderDelList.Count > 0)
        {
            foreach (int indexId in soliderDelList)
            {
                soliders.Remove(indexId);
            }
            soliderDelList.Clear();
        }
        if (effectDelList.Count > 0)
        {
            foreach (int indexId in effectDelList)
            {
                effects.Remove(indexId);
            }
            effectDelList.Clear();
        }
        if (bulletDelList.Count > 0)
        {
            foreach (int indexId in bulletDelList)
            {
                bullets.Remove(indexId);
            }
            bulletDelList.Clear();
        }
        if (towerDelList.Count > 0)
        {
            foreach (int indexId in towerDelList)
            {
                towers.Remove(indexId);
            }
            towerDelList.Clear();
        }
    }

    public List<SoliderInfo> GetSoliderInfo()
    {
        List<SoliderInfo> list = new List<SoliderInfo>();
        foreach (int key in soliders.Keys)
        {
            list.Add(soliders[key]);
        }
        return list;
    }

    public List<MonsterInfo> GetMonsterInfo()
    {
        List<MonsterInfo> list = new List<MonsterInfo>();
        foreach (int key in monsters.Keys)
        {
            list.Add(monsters[key]);
        }
        return list;
    }
}
