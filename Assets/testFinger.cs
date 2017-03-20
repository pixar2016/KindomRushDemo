using UnityEngine;
using System.Collections;

public class testFinger : MonoBehaviour {


	// Use this for initialization
	void Start () {
	
	}

    void OnEnable()
    {
        Debug.Log("OnEnable");
        FingerGestures.OnFingerDown += OnFingerDown;
    }

    void OnDisable()
    {
        FingerGestures.OnFingerDown -= OnFingerDown;
    }
    void OnFingerDown(int fingerIndex, Vector2 fingerPos)
    {
        RaycastHit2D[] ray;
        //int hitNumber = Physics2D.LinecastNonAlloc(GetWorldPos(fingerPos))
    }

    //把Unity屏幕坐标换算成3D坐标
    Vector3 GetWorldPos(Vector2 screenPos)
    {
        Camera mainCamera = Camera.main;
        return mainCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, Mathf.Abs(transform.position.z - mainCamera.transform.position.z)));
    }

    Ray _ray;
    RaycastHit objhit;
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.Log("ButtonDown");
            if(Physics.Raycast(_ray, out objhit, 100)){
                GameObject gameObj = objhit.collider.gameObject;
                Debug.Log("Raycast"+gameObj.name);
                Debug.DrawLine(_ray.origin, objhit.point, Color.red, 2);
            }
            Debug.DrawLine(_ray.origin, _ray.GetPoint(100), Color.red, 2);
        }
	}
}
