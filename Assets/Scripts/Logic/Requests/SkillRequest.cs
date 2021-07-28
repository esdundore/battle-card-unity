using System;

[Serializable]
public class SkillRequest : PlayersRequest
{
    public int user;
    public int handIndex;
    public SkillRequest() { }
    public SkillRequest(PlayersRequest playersRequest)
    {
        this.player1 = playersRequest.player1;
        this.player2 = playersRequest.player2;
    }
}