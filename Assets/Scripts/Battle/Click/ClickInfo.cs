using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ClickType
{
    Tower = 1,
    Monster = 2,
    Solider = 3,
    Hero = 4
}

//保存点击信息
public class ClickInfo : MonoBehaviour
{
    public ClickType clickType;

    public int Id;

    public delegate void FingerDown(ClickInfo clickInfo);

    public FingerDown fingerDown;

    public void OnInit(ClickType _clickType, int _id, FingerDown _fingerDown)
    {
        clickType = _clickType;
        Id = _id;
        fingerDown = _fingerDown;
    }
}

