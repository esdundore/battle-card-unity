using System;
using System.Collections.Generic;

[Serializable]
public class Monster
{
    public string name;
    public string monsterType;
    public int currentLife;
    public int maxLife;
    public int tempDamage;
    public bool canAttack;
    public List<string> statuses;
}
