using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Hero;

//同时只能绘制一条路径，或者塔的位置，不能同时存在多个SavePath
public class SavePath : MonoBehaviour {

    public List<string> paths;
    public string path;

	// Use this for initialization
	void Start () {

	}

    public void ClearOtherSavePath()
    {
        GameObject obj = GameObject.Find("Base");
        SavePath[] paths = obj.GetComponentsInChildren<SavePath>();
        foreach (SavePath path in paths)
        {
            path.enabled = false;
        }
    }

    public void PaintPath()
    {
        path = "Resources/Prefabs/littlesheep.prefab";
    }
    public void PaintTower()
    {
        path = "Resources/Prefabs/Tower.prefab";
    }
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            CreateTag(GetWorldPos(Input.mousePosition));
        }
	}

    GameObject CreateTag(Vector3 pos)
    {
        GameObject sheep = GameLoader.Instance.LoadAssetSync(path).GameObjectAsset;
        sheep.transform.position = pos;
        sheep.transform.parent = transform;
        sheep.GetComponent<SpriteRenderer>().sortingOrder = 1;
        return sheep;
    }

    //把Unity屏幕坐标换算成3D坐标
    Vector3 GetWorldPos(Vector2 screenPos)
    {
        Camera mainCamera = Camera.main;
        return mainCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, Mathf.Abs(transform.position.z - mainCamera.transform.position.z)));
    }
}
