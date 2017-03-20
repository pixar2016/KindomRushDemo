using UnityEngine;
using System.Collections.Generic;
using Hero;
public class UiManager
{
    private static UiManager _instance;
    public static UiManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new UiManager();
            }
            return _instance;
        }
    }

    public GameObject UIRoot;
    //点击其他区域直接关闭UI
    public UIDefine[] ClickPanels = { UIDefine.eSelectPanel};
    public void Init(GameObject uiRoot)
    {
        UIRoot = uiRoot;
    }

    public void CloseClickPanels()
    {
        foreach (UIDefine panel in ClickPanels)
        {
            if (_panels.ContainsKey(panel))
            {
                CloseUIById(panel);
            }
        }
    }

    private Dictionary<UIDefine, GameObject> _panels = new Dictionary<UIDefine, GameObject>();

    public bool HasOpenUI(UIDefine panelId)
    {
        if (_panels.ContainsKey(panelId))
        {
            return true;
        }
        return false;
    }

    public void OpenUI(UIDefine panelId, params object[] param)
    {
        if (_panels.ContainsKey(panelId))
        {
            Debug.Log("already open panelId:" + panelId);
            return;
        }
        UIData data = UIDataSetting.Instance.UIPrefabs[panelId];
        if (data == null)
        {
            Debug.Log("not find panelId:" + panelId);
            return;
        }
        ILoadAsset asset = GameLoader.Instance.LoadAssetSync(data.path);
        if (asset.GameObjectAsset == null)
        {
            Debug.Log("not load path:" + data.path);
            return;
        }
        GameObject node = asset.GameObjectAsset;
        UIComponent ui = node.AddComponent(data.className) as UIComponent;
        if (ui == null)
        {
            Debug.Log("not instantiate class:" + data.className.Name);
            return;
        }
        _panels.Add(panelId, node);
        node.transform.parent = UIRoot.transform;
        node.transform.localPosition = new Vector3(0,0,0);
        ui.panelId = panelId;
        ui.m_LoadAsset = asset;
        ui.OnInit(param);
    }
    public void CloseUIById(UIDefine panelId)
    {
        if (_panels.ContainsKey(panelId))
        {
            GameObject node = _panels[panelId];
            UIComponent ui = node.GetComponent<UIComponent>();
            ui.OnRelease();
            GameLoader.Instance.UnLoadGameObject(ui.m_LoadAsset);
            GameObject.Destroy(node);
            _panels.Remove(panelId);
        }
    }
}

