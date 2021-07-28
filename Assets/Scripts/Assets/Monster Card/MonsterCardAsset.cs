using UnityEngine;
using System.Collections.Generic;

public class MonsterCardAsset : ScriptableObject
{
    public Sprite image;
    public Color32 cardColor;
    public Color32 iconColor;
    public int life;
    public int currentLife;
    public int tempDamage;
    public string title;
    public string type;
    public string currentType;
    public MonsterCardAsset mainLineage;
    public MonsterCardAsset subLineage;
    public List<string> statuses;
}