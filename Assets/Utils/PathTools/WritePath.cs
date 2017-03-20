using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class WritePath : MonoBehaviour {

    public static List<GameObject> curSelections;

    public static List<SavePath> savePaths;
	// Use this for initialization
	void Start () {
	
	}
	[MenuItem("Plugins/WriteObj/littlesheep")]
    static void Writelittlesheep()
    {
        LoadCurSelection();
        foreach (GameObject obj in curSelections)
        {
            if (obj.GetComponent<SavePath>() == null)
            {
                obj.AddComponent<SavePath>();
            }
            SavePath path = obj.GetComponent<SavePath>();
            path.PaintPath();
            path.ClearOtherSavePath();
            path.enabled = true;
        }
    }
    [MenuItem("Plugins/WriteObj/tower")]
    static void WriteTower()
    {
        LoadCurSelection();
        foreach (GameObject obj in curSelections)
        {
            if (obj.GetComponent<SavePath>() == null)
            {
                obj.AddComponent<SavePath>();
            }
            
            SavePath path = obj.GetComponent<SavePath>();
            path.PaintTower();
            path.ClearOtherSavePath();
            path.enabled = true;
        }
    }

    static void LoadCurSelection()
    {
        if (curSelections == null) curSelections = new List<GameObject>();
        curSelections.Clear();
        object[] selection = (object[])Selection.objects;
        if (selection.Length == 0)
        {
            return;
        }
        foreach (object obj in selection)
        {
            if (obj.GetType() == typeof(UnityEngine.GameObject))
            {
                curSelections.Add((GameObject)obj);
            }
        }
    }

    
	// Update is called once per frame
	void Update () {
	
	}
}
