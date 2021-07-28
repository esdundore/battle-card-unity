using UnityEngine;
public class UpdateMonsterCommand : Command
{

    private CardAreaVisual area;
    private int slotIndex;
    private Monster monster;

    public UpdateMonsterCommand(CardAreaVisual area, int slotIndex, Monster monster)
    {
        this.area = area;
        this.slotIndex = slotIndex;
        this.monster = monster;
    }

    public override void StartCommandExecution()
    {
        area.UpdateMonsterCard(slotIndex, monster);
        Command.CommandExecutionComplete();
    }
}
