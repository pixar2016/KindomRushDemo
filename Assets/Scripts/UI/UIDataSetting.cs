using System;
using System.Collections.Generic;

public class UIData
{
    public UIData() { }
    public UIData(string path = "", Type className = null)
    {
        this.path = path;
        this.className = className;
    }
    public string path = "";
    public Type className = null;
}

public class UIDataSetting
{
    private static UIDataSetting _instance;
    public static UIDataSetting Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new UIDataSetting();
                _instance.SetupData();
            }
            return _instance;
        }
    }
    private Dictionary<UIDefine, UIData> _uiPrefabs = new Dictionary<UIDefine, UIData>();

    public Dictionary<UIDefine, UIData> UIPrefabs
    {
        get
        {
            return _uiPrefabs;
        }
    }

    private void SetupData()
    {
        _uiPrefabs.Add(UIDefine.eSelectPanel, new UIData("Resources/Prefabs/ui/SelectPanel.prefab", typeof(SelectPanel)));
    }
}

public enum UIDefine
{
    eStartPanel = 1,
    eSelectPanel = 2
}