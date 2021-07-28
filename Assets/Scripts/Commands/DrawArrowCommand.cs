public class DrawArrowCommand : Command
{
    private PlayerAreaVisual playerArea;
    private PlayerAreaVisual targetArea;
    private int user;
    private int target;

    public DrawArrowCommand(PlayerAreaVisual playerArea, PlayerAreaVisual targetArea, int user, int target)
    {
        this.playerArea = playerArea;
        this.targetArea = targetArea;
        this.user = user;
        this.target = target;
    }

    public override void StartCommandExecution()
    {
        //playerArea.usersVisual.DrawArrow(targetArea, user, target);
        Command.CommandExecutionComplete();
    }
}
