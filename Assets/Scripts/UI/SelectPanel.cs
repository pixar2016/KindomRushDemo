using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//建造选择界面

public class SelectPanel : UIComponent
{
    int panelType;
    TowerInfo towerInfo;

    public GameObject BuildTowerObj;
    public Button BtnArrowTower;
    public Button BtnMageTower;
    public Button BtnSoliderTower;
    public Button BtnArtileryTower;

    public GameObject UpgradeTowerObj;
    public Button BtnUpgrade;
    public Button BtnSell;

    public GameObject UpgradeSkillObj;
    public Button BtnSkill1;
    public Button BtnSkill2;
    public Button BtnSkill3;
    //public Button button;
    public override void OnInit(object[] data)
    {
        towerInfo = (TowerInfo)data[0];
        this.gameObject.transform.position = Camera.main.WorldToScreenPoint(towerInfo.GetPosition());
        BuildTowerObj = transform.Find("BuildTower").gameObject;
        BtnArrowTower = transform.Find("BuildTower/BtnArrow").GetComponent<Button>();
        BtnMageTower = transform.Find("BuildTower/BtnMage").GetComponent<Button>();
        BtnSoliderTower = transform.Find("BuildTower/BtnSolider").GetComponent<Button>();
        BtnArtileryTower = transform.Find("BuildTower/BtnArtilery").GetComponent<Button>();

        UpgradeTowerObj = transform.Find("UpgradeTower").gameObject;
        BtnUpgrade = transform.Find("UpgradeTower/BtnUpgrade").GetComponent<Button>();
        BtnSell = transform.Find("UpgradeTower/BtnSell").GetComponent<Button>();

        UpgradeSkillObj = transform.Find("UpgradeSkill").gameObject;
        BtnSkill1 = transform.Find("UpgradeSkill/BtnSkill1").GetComponent<Button>();
        BtnSkill2 = transform.Find("UpgradeSkill/BtnSkill2").GetComponent<Button>();
        BtnSkill3 = transform.Find("UpgradeSkill/BtnSkill3").GetComponent<Button>();
        SetEventListener();

        ShowPanel(towerInfo);
    }
    //1-建造塔  2-升级塔  3-升级塔的技能
    public void ShowPanel(TowerInfo towerInfo)
    {
        int towerType = towerInfo.towerType;
        int towerLevel = towerInfo.towerData._level;
        BuildTowerObj.SetActive(false);
        UpgradeTowerObj.SetActive(false);
        UpgradeSkillObj.SetActive(false);
        if (towerType == 5)
        {
            BuildTowerObj.SetActive(true);
        }
        else if (towerLevel <= 3)
        {
            UpgradeTowerObj.SetActive(true);
        }
        else
        {
            UpgradeSkillObj.SetActive(true);
        }
    }

    public void SetEventListener()
    {
        UIEventListener.Get(BtnArrowTower.gameObject).onClick = OnBtnArrowTowerClick;
        UIEventListener.Get(BtnMageTower.gameObject).onClick = OnBtnMageTowerClick;
        UIEventListener.Get(BtnSoliderTower.gameObject).onClick = OnBtnSoliderTowerClick;
        UIEventListener.Get(BtnArtileryTower.gameObject).onClick = OnBtnArtileryTowerClick;

        UIEventListener.Get(BtnUpgrade.gameObject).onClick = OnBtnUpgradeClick;
        UIEventListener.Get(BtnSell.gameObject).onClick = OnBtnSellClick;

        UIEventListener.Get(BtnSkill1.gameObject).onClick = OnBtnSkill1Click;
        UIEventListener.Get(BtnSkill2.gameObject).onClick = OnBtnSkill2Click;
        UIEventListener.Get(BtnSkill3.gameObject).onClick = OnBtnSkill3Click;
    }

    public void OnBtnArrowTowerClick(GameObject go)
    {
        Debug.Log("OnBtnArrowTowerClick");
        //UiManager.Instance.CloseUIById(UIDefine.eSelectPanel);
        EntityManager.getInstance().RemoveTower(towerInfo.Id);
        EntityManager.getInstance().AddTower(2);
        UiManager.Instance.CloseUIById(UIDefine.eSelectPanel);
    }

    public void OnBtnMageTowerClick(GameObject go)
    {
        //UiManager.Instance.CloseUIById(UIDefine.eSelectPanel);
    }

    public void OnBtnSoliderTowerClick(GameObject go)
    {
        //UiManager.Instance.CloseUIById(UIDefine.eSelectPanel);
    }

    public void OnBtnArtileryTowerClick(GameObject go)
    {
        //UiManager.Instance.CloseUIById(UIDefine.eSelectPanel);
    }

    public void OnBtnUpgradeClick(GameObject go)
    {

    }

    public void OnBtnSellClick(GameObject go)
    {

    }

    public void OnBtnSkill1Click(GameObject go)
    {

    }

    public void OnBtnSkill2Click(GameObject go)
    {

    }

    public void OnBtnSkill3Click(GameObject go)
    {

    }

    public override void OnRelease()
    {
        
    }
}

