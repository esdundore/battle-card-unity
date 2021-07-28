using UnityEngine;
using System.Collections.Generic;
public class AbsorbCardsCommand : Command {

    private CardAreaVisual area;
    private List<int> slotIndexes;
    private Vector3 destination;

    public AbsorbCardsCommand(CardAreaVisual area, List<int> slotIndexes, Vector3 destination)
    {
        this.area = area;
        this.slotIndexes = slotIndexes;
        this.destination = destination;
    }

    public override void StartCommandExecution()
    {
        area.SequenceAbsorbCards(slotIndexes, destination);
    }
}
