using System;
using System.Collections.Generic;

[Serializable]
public class SkillArea
{
    public string attacker;
    public string defender;
    public int attackId;
    public bool resolved;
    public string targetArea;
    public List<Skill> attacks;
    public List<Skill> defenses;
}