using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;

public class DispatcherEventInvok
{
    private static DispatcherEventInvok instance;
    private List<InvokBase> lEvents = new List<InvokBase>();
    private Dictionary<int, List<pair>> RegisterDict = new Dictionary<int, List<pair>>();

    public void Clear()
    {
        this.lEvents.Clear();
    }

    public void DispatchEvent(int key, params object[] param)
    {
        lock (this)
        {
            if (this.RegisterDict.ContainsKey(key))
            {
                this.lEvents.Add(new InvokBase(key, false, param));
            }
        }
    }

    public void DispatchEventSave(int key, params object[] param)
    {
        lock (this)
        {
            this.lEvents.Add(new InvokBase(key, true, param));
        }
    }

    public static DispatcherEventInvok Instance()
    {
        if (instance == null)
        {
            instance = new DispatcherEventInvok();
        }
        return instance;
    }

    public void OnTick()
    {
        int num2;
        List<InvokBase> list = new List<InvokBase>();
        for (int i = 0; i < this.lEvents.Count; i++)
        {
            InvokBase item = this.lEvents[i];
            if (this.RegisterDict.ContainsKey(item.key))
            {
                num2 = 0;
                while (num2 < this.RegisterDict[item.key].Count)
                {
                    DispatcherEventInvok.pair pair;
                    try
                    {
                        pair = this.RegisterDict[item.key][num2];
                        Debug.Log(pair.methodInfo);
                        Debug.Log(pair.obj);
                        Debug.Log(item.param);
                        pair.methodInfo.Invoke(pair.obj, item.param);
                    }
                    catch (Exception exception)
                    {
                        pair = this.RegisterDict[item.key][num2];
                        Debug.Log(">>>DispatchEvent Error:  " + exception.Message + "  method name ==  " + pair.methodName + "<<<\n");
                    }
                    num2++;
                }
                list.Add(item);
            }
            else if (!item.save)
            {
                list.Add(item);
            }
        }
        for (num2 = 0; num2 < list.Count; num2++)
        {
            this.lEvents.Remove(list[num2]);
        }
    }

    public void Register(int key, object obj, string methodName)
    {
        List<DispatcherEventInvok.pair> list = null;
        DispatcherEventInvok.pair pair;
        if (this.RegisterDict.ContainsKey(key))
        {
            list = this.RegisterDict[key];
        }
        else
        {
            list = new List<DispatcherEventInvok.pair>();
        }
        pair.obj = obj;
        pair.methodInfo = obj.GetType().GetMethod(methodName);
        pair.methodName = methodName;
        if (pair.methodInfo == null)
        {
            Debug.Log(">>>method: " + methodName + " no found!");
        }
        list.Add(pair);
        this.RegisterDict[key] = list;
    }

    public void UnRegister(int key, object obj, string methodName)
    {
        if (this.RegisterDict.ContainsKey(key))
        {
            List<pair> list = this.RegisterDict[key];
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if ((list[i].methodInfo.Name == methodName) && (list[i].obj == obj))
                {
                    list.RemoveAt(i);
                }
            }
            if (list.Count == 0)
            {
                this.RegisterDict.Remove(key);
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct pair
    {
        public object obj;
        public MethodInfo methodInfo;
        public string methodName;
    }
}

