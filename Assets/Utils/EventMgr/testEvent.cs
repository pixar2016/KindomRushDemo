using UnityEngine;
using System.Collections;

public class testEvent : MonoBehaviour
{


    void Start()
    {

        EventsMgr.GetInstance().Init();
        Debug.Log("RabbitJump JumpHeight:");
        attachEvents();

        TriggerEvents();

        MiniEventDispatcher ee = new MiniEventDispatcher();
        ee.Register("ee", ff);
        ee.Register("ee", ff);
        ee.Remove("ee", ff);
        ee.Broadcast("ee",32,"sfs",76);
        //detachrEvents();
    }

    void attachEvents()
    {
        EventsMgr.GetInstance().AttachEvent(eEventsKey.控制系统_兔子跳, RabbitJump);
        EventsMgr.GetInstance().AttachEvent(eEventsKey.控制系统_兔子向右走, RabbitRight);
        EventsMgr.GetInstance().AttachEvent(eEventsKey.控制系统_兔子停止向右走, RabbitStopRight);
    }

    void TriggerEvents()
    {
        EventsMgr.GetInstance().TriigerEvent(eEventsKey.控制系统_兔子跳, 10.0f);
        EventsMgr.GetInstance().TriigerEvent(eEventsKey.控制系统_兔子向右走, 5.0f);
        EventsMgr.GetInstance().TriigerEvent(eEventsKey.控制系统_兔子停止向右走, 5.0f);
    }

    void detachrEvents()
    {
        EventsMgr.GetInstance().DetachEvent(eEventsKey.控制系统_兔子跳, RabbitJump);
        EventsMgr.GetInstance().DetachEvent(eEventsKey.控制系统_兔子向右走, RabbitRight);
        EventsMgr.GetInstance().DetachEvent(eEventsKey.控制系统_兔子停止向右走, RabbitStopRight);
    }

    //void RabbitJump(object data)
    //{
    //    Debug.Log("RabbitJump JumpHeight:" + (float)data);
    //}

    void ff(object[] data)
    {
        foreach (object obj in data)
        {
            Debug.Log(obj);
        }
        Debug.Log("eeeeeeeeeeeeeee");
    }
    void RabbitJump(object[] data)
    {
        Debug.Log("RabbitJump JumpHeight:" + data[0]);
    }

    void RabbitRight(object[] data)
    {
        Debug.Log("RabbitRight Speed:" + data[0]);
    }

    void RabbitStopRight(object[] data)
    {
        Debug.Log("RabbitStop" +data[0]);
    }

}

