using System;

[Serializable]
public class GameView
{
    public string currentPlayer;
    public string winner;
    public string phase;
    public string environmentCard;
    public PlayerArea playerArea;
    public PlayerArea opponentArea;
    public SkillArea skillArea;
    public Playable playable;
}