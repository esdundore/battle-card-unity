public class ShowMessageCommand : Command
{
    private string text;
    private float duration;

    public ShowMessageCommand(string text, float duration)
    {
        this.text = text;
        this.duration = duration;
    }

    public override void StartCommandExecution()
    {
        MessageManager.Instance.ShowMessage(text, duration);
    }
}
