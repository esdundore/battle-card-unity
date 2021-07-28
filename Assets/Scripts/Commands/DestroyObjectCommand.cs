using UnityEngine;
using System.Collections.Generic;
public class DestroyObjectCommand : Command
{
    private CardAreaVisual area;
    private List<int> slotIndexes;

    public DestroyObjectCommand(CardAreaVisual area, List<int> slotIndexes)
    {
        this.area = area;
        this.slotIndexes = slotIndexes;
    }

    public override void StartCommandExecution()
    {
        area.CompleteSequenceAbsorbCards(slotIndexes);
    }
}