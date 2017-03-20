using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager
{
    private static SkillManager instance = null;

    public Dictionary<int, SkillInfo> skills;

    public int skillIndexId;

    public static SkillManager getInstance()
    {
        if (instance == null)
        {
            instance = new SkillManager();
        }
        return instance;
    }

    private SkillManager()
    {
        skills = new Dictionary<int, SkillInfo>();
        skillIndexId = 0;
    }

    public SkillInfo AddSkill(int skillId, CharacterInfo charInfo)
    {
        //Debug.Log("AddSkill");
        skillIndexId++;
        SkillInfo skillInfo = new SkillInfo(skillIndexId, skillId);
        skillInfo.charInfo = charInfo;
        skillInfo.LoadData();
        skills.Add(skillIndexId, skillInfo);
        return skillInfo;
    }

    public void RemoveSkill(int skillIndexId)
    {
        if (skillIndexId <= 0)
        {
            return;
        }
        foreach (int key in skills.Keys)
        {
            if (skills[key].Id == skillIndexId)
            {
                skills.Remove(key);
                break;
            }
        }
    }

    public void StartSkill(SkillInfo skillInfo)
    {
        skillInfo.Start();
    }

    public void EndSkill(SkillInfo skillInfo)
    {
        skillInfo.End();
    }

    public void Update()
    {
        foreach (int key in skills.Keys)
        {
            skills[key].Update();
        }
    }

    public void Release()
    {
        skills.Clear();
        skillIndexId = 0;
        instance = null;
    }
}
