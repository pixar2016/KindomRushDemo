using UnityEngine;
using System.Collections.Generic;


public class UIComponent : MonoBehaviour
{
    public UIDefine panelId;
    public ILoadAsset m_LoadAsset;

    public virtual void OnInit(object[] data)
    {
    }

    public virtual void OnRelease()
    {

    }
}

