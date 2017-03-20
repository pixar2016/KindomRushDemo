using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class BattleFingerEvent
{
    private static BattleFingerEvent instance;
    public static BattleFingerEvent getInstance()
    {
        if (instance == null)
        {
            instance = new BattleFingerEvent();
        }
        return instance;
    }
    public MiniEventDispatcher eventDispatcher;
    private BattleFingerEvent()
    {
        eventDispatcher = new MiniEventDispatcher();
        FingerGestures.OnFingerDown += OnFingerDown;
    }

    //在这里接受点击事件，只需发射一次射线识别物体，其他需要识别点击的物体接收广播即可。
    public void OnFingerDown(int fingerIndex, Vector2 fingerPos)
    {
#if IPHONE || ANDROID
        if(EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)){
#else
        if (EventSystem.current.IsPointerOverGameObject())
        {
#endif
            Debug.Log("点击到UI");
            return;
        }
        //未点击到UI
        GameObject obj = PickObject(fingerPos);
        //点击到物体
        if (obj != null && obj.GetComponent<ClickInfo>() != null)
        {
            Debug.Log("Click Obj");
            ClickInfo temp = obj.GetComponent<ClickInfo>();
            temp.fingerDown(temp);
            GameManager.getInstance().curClickInfo = temp;
        }
        //未点击到任何物体
        else
        {
            Debug.Log("Not Click Obj");
            UiManager.Instance.CloseClickPanels();
            GameManager.getInstance().curClickInfo = null;
        }
    }

    public static GameObject PickObject(Vector2 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
            return hit.collider.gameObject;

        return null;
    }
    public void Release()
    {
        FingerGestures.OnFingerDown -= OnFingerDown;
    }
}

