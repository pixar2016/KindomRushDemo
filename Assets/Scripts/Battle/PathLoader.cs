using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//预加载路径信息
//路径信息包括：
public class PathLoader
{
    private Dictionary<string, PathInfo> pathDict;
    public PathLoader()
    {
        pathDict = new Dictionary<string, PathInfo>();
    }

    //从json文件中加载路径信息
    //path为json文件路径
    public void LoadPath(string path)
    {
        string infos = FileUtils.LoadFile(Application.dataPath, "Data/Json/level/" + path + ".json");
        Dictionary<string, object> table = MiniJSON.Json.Deserialize(infos) as Dictionary<string, object>;
        foreach (string key in table.Keys)
        {
            List<object> singlePath = table[key] as List<object>;
            PathInfo pathInfo = new PathInfo();
            foreach (object point in singlePath)
            {
                Dictionary<string, object> pathPoint = point as Dictionary<string, object>;
                float x = float.Parse(pathPoint["x"].ToString());
                float y = float.Parse(pathPoint["y"].ToString());
                pathInfo.AddPoint(new Vector3(x, y, 0));
            }
            pathDict.Add(key, pathInfo);
        }
    }

    //根据路径名得到路径信息
    public PathInfo GetPath(string pathName)
    {
        if (pathDict.ContainsKey(pathName))
        {
            return pathDict[pathName];
        }
        return null;
    }

    //释放所有存储的路径信息
    public void Release()
    {
        pathDict.Clear();
    }
}

