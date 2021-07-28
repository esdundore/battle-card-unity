using System;
using System.Collections.Generic;

[Serializable]
public class PlayerArea
{
    public Deck deck;
    public List<SkillCard> hand;
    public List<string> discards;
    public List<Monster> monsters;
    public List<Monster> subMonsters;
    public Breeder breeder;
}