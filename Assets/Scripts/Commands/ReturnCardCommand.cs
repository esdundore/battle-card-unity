public class ReturnCardCommand : Command {

    private CardAreaVisual area;
    private int slotIndex;
    private bool rotate;

    public ReturnCardCommand(CardAreaVisual area, int slotIndex, bool rotate)
    {
        this.area = area;
        this.slotIndex = slotIndex;
        this.rotate = rotate;
    }

    public override void StartCommandExecution()
    {
        area.SequenceMoveCard(slotIndex, rotate);
    }
}