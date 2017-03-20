using System;
using System.Collections.Generic;
using UnityEngine;
public class EntityViewManager
{
    private static EntityViewManager instance = null;

    public static EntityViewManager getInstance()
    {
        if (instance == null)
        {
            instance = new EntityViewManager();
        }
        return instance;
    }

    public Dictionary<int, CharacterView> monsters;
    public Dictionary<int, CharacterView> soliders;
    public Dictionary<int, EffectView> effects;
    public Dictionary<int, BulletView> bullets;
    public Dictionary<int, TowerView> towers;

    private EntityViewManager()
    {
        monsters = new Dictionary<int, CharacterView>();
        soliders = new Dictionary<int, CharacterView>();
        effects = new Dictionary<int, EffectView>();
        bullets = new Dictionary<int, BulletView>();
        towers = new Dictionary<int, TowerView>();
        EntityManager.getInstance().eventDispatcher.Register("AddMonster", AddMonster);
        EntityManager.getInstance().eventDispatcher.Register("AddSolider", AddSolider);
        EntityManager.getInstance().eventDispatcher.Register("AddEffect", AddEffect);
        EntityManager.getInstance().eventDispatcher.Register("AddBullet", AddBullet);
        EntityManager.getInstance().eventDispatcher.Register("AddTower", AddTower);

        EntityManager.getInstance().eventDispatcher.Register("RemoveMonster", RemoveMonster);
        EntityManager.getInstance().eventDispatcher.Register("RemoveSolider", RemoveSolider);
        EntityManager.getInstance().eventDispatcher.Register("RemoveEffect", RemoveEffect);
        EntityManager.getInstance().eventDispatcher.Register("RemoveBullet", RemoveBullet);
        EntityManager.getInstance().eventDispatcher.Register("RemoveTower", RemoveTower);
    }

    public void AddMonster(object[] data)
    {
        CharacterInfo charInfo = (CharacterInfo)data[0];
        CharacterView charView = new CharacterView(charInfo);
        charView.LoadModel();
        if (monsters.ContainsKey(charInfo.Id))
        {
            monsters[charInfo.Id] = charView;
        }
        else
        {
            monsters.Add(charInfo.Id, charView);
        }
    }

    public void AddSolider(object[] data)
    {
        CharacterInfo charInfo = (CharacterInfo)data[0];
        CharacterView charView = new CharacterView(charInfo);
        charView.LoadModel();
        if (soliders.ContainsKey(charInfo.Id))
        {
            soliders[charInfo.Id] = charView;
        }
        else
        {
            soliders.Add(charInfo.Id, charView);
        }
    }

    public void AddEffect(object[] data)
    {
        EffectInfo effectInfo = (EffectInfo)data[0];
        EffectView effectView = new EffectView(effectInfo);
        effectView.LoadModel();
        if (effects.ContainsKey(effectInfo.Id))
        {
            effects[effectInfo.Id] = effectView;
        }
        else
        {
            effects.Add(effectInfo.Id, effectView);
        }
    }

    public void AddBullet(object[] data)
    {
        BulletInfo bulletInfo = (BulletInfo)data[0];
        BulletView bulletView = new BulletView(bulletInfo);
        bulletView.LoadModel();
        if (bullets.ContainsKey(bulletInfo.Id))
        {
            bullets[bulletInfo.Id] = bulletView;
        }
        else
        {
            bullets.Add(bulletInfo.Id, bulletView);
        }
    }

    public void AddTower(object[] data)
    {
        TowerInfo tempInfo = (TowerInfo)data[0];
        int towerId;
        TowerView towerView;
        //若为兵营
        if (tempInfo.towerType == 4)
        {
            BarrackTowerInfo towerInfo = (BarrackTowerInfo)tempInfo;
            towerId = towerInfo.Id;
            towerView = new BarrackTowerView(towerInfo);
        }
        //若为空地
        else if (tempInfo.towerType == 5)
        {
            OpenSpaceInfo spaceInfo = (OpenSpaceInfo)tempInfo;
            towerId = spaceInfo.Id;
            towerView = new OpenSpaceView(spaceInfo);
        }
        else
        {
            AttackTowerInfo towerInfo = (AttackTowerInfo)tempInfo;
            towerId = towerInfo.Id;
            towerView = new ArrowTowerView(towerInfo);
        }
        towerView.LoadModel();
        if (towers.ContainsKey(towerId))
        {
            towers[towerId] = towerView;
        }
        else
        {
            towers.Add(towerId, towerView);
        }
    }
    public void RemoveMonster(object[] data)
    {
        int monsterIndexId = (int)data[0];
        int delId = -1;
        foreach (int key in monsters.Keys)
        {
            if (monsters[key].charInfo.Id == monsterIndexId)
            {
                delId = key;
                monsters[key].Release();
            }
        }
        if (delId != -1)
        {
            monsters.Remove(delId);
        }
    }
    public void RemoveSolider(object[] data)
    {
        int soliderIndexId = (int)data[0];
        int delId = -1;
        foreach (int key in soliders.Keys)
        {
            if (soliders[key].charInfo.Id == soliderIndexId)
            {
                delId = key;
                soliders[key].Release();
            }
        }
        if (delId != -1)
        {
            soliders.Remove(delId);
        }
    }
    public void RemoveEffect(object[] data)
    {
        int effectIndexId = (int)data[0];
        int delId = -1;
        foreach (int key in effects.Keys)
        {
            if (effects[key].effectInfo.Id == effectIndexId)
            {
                delId = key;
                effects[key].Release();
            }
        }
        if (delId != -1)
        {
            effects.Remove(delId);
        }
    }
    public void RemoveBullet(object[] data)
    {
        int bulletIndexId = (int)data[0];
        int delId = -1;
        foreach (int key in bullets.Keys)
        {
            if (bullets[key].bulletInfo.Id == bulletIndexId)
            {
                delId = key;
                bullets[key].Release();
            }
        }
        if (delId != -1)
        {
            bullets.Remove(delId);
        }
    }
    public void RemoveTower(object[] data)
    {
        int towerIndexId = (int)data[0];
        int delId = -1;
        foreach (int key in towers.Keys)
        {
            if (towers[key].towerInfo.Id == towerIndexId)
            {
                delId = key;
                towers[key].Release();
            }
        }
        if (delId != -1)
        {
            towers.Remove(delId);
        }
    }
    public void Update()
    {
        foreach (int key in soliders.Keys)
        {
            soliders[key].Update();
        }
        foreach (int key in monsters.Keys)
        {
            monsters[key].Update();
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
    }
}
