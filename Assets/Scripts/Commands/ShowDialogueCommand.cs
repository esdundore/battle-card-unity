using UnityEngine;
public class ShowDialogueCommand : Command
{
    private Vector3 position;
    private int msgId;


    public ShowDialogueCommand(Vector3 position, int msgId)
    {
        this.position = position;
        this.msgId = msgId;
    }

    public override void StartCommandExecution()
    {
        DialogueEffect.CreateTextBubble(position, msgId);
    }
}
