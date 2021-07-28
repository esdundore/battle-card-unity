using UnityEngine.UI;

public class EnableButtonCommand : Command
{
    private PlayerAreaVisual playerArea;
    private bool isOn;

    public EnableButtonCommand(PlayerAreaVisual playerArea, bool isOn)
    {
        this.playerArea = playerArea;
        this.isOn = isOn;
    }

    public override void StartCommandExecution()
    {
        playerArea.button.GetComponent<Button>().interactable = isOn;
        Command.CommandExecutionComplete();
    }
}
