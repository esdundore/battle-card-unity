using UnityEngine;
public class MoveCardCommand : Command {

    private CardAreaVisual area;
    private int slotIndex;
    private Vector3 destination;
    private bool rotate;

    public MoveCardCommand(CardAreaVisual area, int slotIndex, Vector3 destination, bool rotate)
    {
        this.area = area;
        this.slotIndex = slotIndex;
        this.destination = destination;
        this.rotate = rotate;
    }

    public override void StartCommandExecution()
    {
        area.SequenceMoveCard(slotIndex, destination, rotate);
    }
}