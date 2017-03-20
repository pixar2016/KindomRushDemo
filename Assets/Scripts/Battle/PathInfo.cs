using System;
using System.Collections.Generic;
using UnityEngine;

public class PathInfo
{
    private List<Vector3> pos;
    
    public PathInfo()
    {
        pos = new List<Vector3>();
    }

    public void AddPoint(Vector3 point)
    {
        pos.Add(point);
    }

    public Vector3 GetPoint(int num)
    {
        return pos[num];
    }

    public int GetCount()
    {
        return pos.Count;
    }

    public void PrintAllPoint()
    {
        for (int i = 0; i < pos.Count - 1; i++)
        {
            Debug.Log(Vector3.Distance(pos[i+1], pos[i]));
        }
            //foreach (Vector3 p in pos)
            //{
            //    Debug.Log(p);
            //}
    }
}
