using UnityEngine;
public class CreateSkillCardCommand : Command
{
    private CardAreaVisual area;
    private int slotIndex;
    private SkillCard skillCard;
    private Vector3 origin;

    public CreateSkillCardCommand(CardAreaVisual area, int slotIndex, SkillCard skillCard, Vector3 origin)
    {
        this.area = area;
        this.slotIndex = slotIndex;
        this.skillCard = skillCard;
        this.origin = origin;
    }

    public override void StartCommandExecution()
    {
        if (slotIndex == -1)
        {
            GameObject card = area.CreateSkillCard(-1, skillCard, origin);
            area.SequenceMoveCard(card, new Vector3(0, 0, 0), true);
        }
        else 
        {
            area.CreateSkillCard(slotIndex, skillCard, origin);
            Command.CommandExecutionComplete();
        }

    }
}