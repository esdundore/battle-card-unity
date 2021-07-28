using UnityEngine.UI;
using TMPro;

public class ChangeTextCommand : Command
{
    private TMP_Text oldText;
    private string newText;

    public ChangeTextCommand(TMP_Text oldText, string newText)
    {
        this.oldText = oldText;
        this.newText = newText;
    }

    public override void StartCommandExecution()
    {
        oldText.text = newText;
        Command.CommandExecutionComplete();
    }
}
