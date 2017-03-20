using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerCreator : MonoBehaviour
{
    public GameObject baseSplines;
    private Vector3 up;
    private Vector3 down;
    private Vector3 left;
    private Vector3 right;

    public GameObject towerPos;
    public List<GameObject> towerPosList;
    void Start()
    {
        up = new Vector3(0, 1, 0);
        down = new Vector3(0, -1, 0);
        left = new Vector3(-1, 0, 0);
        right = new Vector3(1, 0, 0);
        towerPosList = new List<GameObject>();
        if (baseSplines == null)
        {
            Debug.Log("BaseSplines is NULL!");
            return;
        }
        Transform temp = baseSplines.transform.Find("Tower");
        if (temp == null)
        {
            Debug.Log("Can not Find TowerObj under BaseSplines");
            towerPos = new GameObject();
            towerPos.name = "Tower";
            towerPos.transform.parent = baseSplines.transform;
            return;
        }
        else
        {
            towerPos = temp.gameObject;
        }
    }

    void Update()
    {
        Move();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddTowerPos();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            RemoveTowerPos();
        }
    }


    public void AddTowerPos()
    {
        GameObject obj = (GameObject)Instantiate(Resources.Load("Prefabs/tool/Tower"));
        obj.transform.parent = towerPos.transform;
        towerPosList.Add(obj);
    }

    public void RemoveTowerPos()
    {
        int count = towerPosList.Count;
        if (count > 0)
        {
            towerPosList.Remove(towerPosList[count-1]);
        }
    }

    private void Move()
    {
        if (Input.GetKey(KeyCode.W))
            transform.position = transform.position + up * Time.deltaTime * 60f;
        if (Input.GetKey(KeyCode.S))
            transform.position = transform.position + down * Time.deltaTime * 60f;

        if (Input.GetKey(KeyCode.A))
            transform.position = transform.position + left * Time.deltaTime * 60f;
        if (Input.GetKey(KeyCode.D))
            transform.position = transform.position + right * Time.deltaTime * 60f;
    }

}
