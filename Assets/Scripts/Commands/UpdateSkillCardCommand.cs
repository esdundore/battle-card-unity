public class UpdateSkillCardCommand : Command
{

    private CardAreaVisual area;
    private int slotIndex;
    private SkillCard skillCard;

    public UpdateSkillCardCommand(CardAreaVisual area, int slotIndex, SkillCard skillCard)
    {
        this.area = area;
        this.slotIndex = slotIndex;
        this.skillCard = skillCard;
    }

    public override void StartCommandExecution()
    {
        area.UpdateSkillCard(slotIndex, skillCard);
        Command.CommandExecutionComplete();
    }
}
