public class ShowPhaseIconCommand : Command
{
    private PlayerAreaVisual area;
    private string phase;
    private bool isActive;

    public ShowPhaseIconCommand(PlayerAreaVisual area, string phase, bool isActive)
    {
        this.area = area;
        this.phase = phase;
        this.isActive = isActive;
    }

    public override void StartCommandExecution()
    {
        area.phaseIcon.ShowPhaseIcon(phase, isActive);
        Command.CommandExecutionComplete();
    }
}
