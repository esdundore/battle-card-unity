using System;

[Serializable]
public class PlayersRequest
{
    public string player1;
    public string player2;
    public PlayersRequest() { }
    public PlayersRequest(String player, String opponent)
    {
        this.player1 = player;
        this.player2 = opponent;
    }
}