using UnityEngine;

public class SkillCardAsset : ScriptableObject 
{
    public MonsterCardAsset userAsset;
    public Sprite image;
    public int gutsCost;
    public int currentGutsCost;
    public int damage;
    public int currentDamage;
    public string title;
    public string type;
    [TextArea(2, 3)]
    public string description;
}
