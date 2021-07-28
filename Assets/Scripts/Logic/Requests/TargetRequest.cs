using System;

[Serializable]
public class TargetRequest : PlayersRequest
{
    public int target;
    public TargetRequest() { }
    public TargetRequest(PlayersRequest playersRequest)
    {
        this.player1 = playersRequest.player1;
        this.player2 = playersRequest.player2;
    }
}