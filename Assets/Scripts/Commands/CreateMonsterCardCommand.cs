using UnityEngine;
public class CreateMonsterCardCommand : Command
{
    private CardAreaVisual area;
    private int slotIndex;
    private Monster monster;
    private Vector3 origin;

    public CreateMonsterCardCommand(CardAreaVisual area, int slotIndex, Monster monster, Vector3 origin)
    {
        this.area = area;
        this.slotIndex = slotIndex;
        this.monster = monster;
        this.origin = origin;
    }

    public override void StartCommandExecution()
    {
        area.CreateMonsterCard(slotIndex, monster, origin);
        Command.CommandExecutionComplete();
    }
}