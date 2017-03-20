using UnityEngine;
using System.Collections.Generic;

public class MiniEventDispatcher
{

    public delegate void CommonEvent(params object[] data);

    public Dictionary<string, CommonEvent> m_dicEvents;

    public MiniEventDispatcher()
    {
        m_dicEvents = new Dictionary<string, CommonEvent>();
    }

    public void Register(string eventKey, CommonEvent attachEvent)
    {
        if (!m_dicEvents.ContainsKey(eventKey))
        {
            m_dicEvents.Add(eventKey, attachEvent);
        }
        else
        {
            m_dicEvents[eventKey] += attachEvent;
        }
    }

    public void Remove(string eventKey, CommonEvent attachEvent)
    {
        if (m_dicEvents.ContainsKey(eventKey))
        {
            m_dicEvents[eventKey] -= attachEvent;
            if (m_dicEvents[eventKey] == null)
            {
                m_dicEvents.Remove(eventKey);
            }
        }
        else
        {
            Debug.LogError("没有定义该事件" + eventKey);
        }
    }

    public void Broadcast(string eventKey, params object[] data)
    {
        if (m_dicEvents.ContainsKey(eventKey))
        {
            m_dicEvents[eventKey](data);
        }
        else
        {
            Debug.LogError("没有定义该事件" + eventKey);
        }
    }
}
