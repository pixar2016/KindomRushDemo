using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class SplineCreator : MonoBehaviour
{
    void Start()
    {

    }

    [MenuItem("Plugins/SplineCreator/NodeCreator")]
    static void AddNodeCreator()
    {
        GameObject obj = (GameObject)Instantiate(Resources.Load("Prefabs/tool/NodeCreator"));
        obj.name = "NodeCreator";
        GameObject baseSpline = GameObject.Find("BaseSplines");
        obj.GetComponent<NodeCreator>().baseSplines = baseSpline;
    }

    [MenuItem("Plugins/SplineCreator/TowerCreator")]
    static void AddTowerCreator()
    {
        GameObject obj = (GameObject)Instantiate(Resources.Load("Prefabs/tool/TowerCreator"));
        obj.name = "TowerCreator";
        GameObject baseSpline = GameObject.Find("BaseSplines");
        obj.GetComponent<TowerCreator>().baseSplines = baseSpline;
    }
}

