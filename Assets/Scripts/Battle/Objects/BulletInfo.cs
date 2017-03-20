using System;
using System.Collections.Generic;
using UnityEngine;

//子弹基类
public class BulletInfo
{
    //索引ID
    public int Id;
    //子弹ID
    public int bulletId;
    //子弹名称
    public string bulletName;
    //子弹初始位置
    public Vector3 startPos;
    //子弹目标位置
    public Vector3 endPos;
    //子弹速度
    public float speed;
    //子弹当前位置
    public Vector3 pos;
    //子弹当前角度
    public Quaternion quaternion;
    //发射子弹的塔或者兵种
    public CharacterInfo charInfo;

    public BulletInfo(int bulletIndexId, int _bulletId, CharacterInfo charInfo, Vector3 _startPos, Vector3 _endPos, float _speed)
    {
        Id = bulletIndexId;
        bulletId = _bulletId;
        bulletName = "arrow";
        startPos = _startPos;
        endPos = _endPos;
        //speed = _speed;
        speed = Vector3.Distance(startPos, endPos) / 0.5f;
        pos = startPos;
    }

    public Vector3 GetPosition()
    {
        return pos;
    }

    public Quaternion GetEulerAngles()
    {
        return quaternion;
    }

    public void SetPosition(float x, float y, float z)
    {
        pos = new Vector3(x, y, z);
    }

    public void Update()
    {
        if (Vector3.Distance(pos, endPos) < 0.1)
        {
            EntityManager.getInstance().RemoveBullet(this.Id);
        }
        else
        {
            pos = Vector3.MoveTowards(pos, endPos, speed * Time.deltaTime);
        }
    }
    public void Release()
    {

    }
}

