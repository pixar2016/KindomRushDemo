using UnityEngine;
using System.Collections;
using Hero;
public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(Input.mousePosition);
            Debug.Log(GetWorldPos(Input.mousePosition));
            CreateSheep(GetWorldPos(Input.mousePosition));
        }
	}

    void CreateSheep(Vector3 pos)
    {
        GameObject sheep = GameLoader.Instance.LoadAssetSync("Resources/Prefabs/littlesheep.prefab").GameObjectAsset;
        sheep.transform.position = pos;
    }

    //把Unity屏幕坐标换算成3D坐标
    Vector3 GetWorldPos(Vector2 screenPos)
    {
        Camera mainCamera = Camera.main;
        return mainCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, Mathf.Abs(transform.position.z - mainCamera.transform.position.z)));
    }
}
