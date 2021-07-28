using UnityEngine;
using System.Collections.Generic;

public class HighlightCommand : Command
{
    private CardAreaVisual area;
    private List<int> slotIndexes;
    private Color32 color;
    private bool isOn;

    public HighlightCommand(CardAreaVisual area, List<int> slotIndexes, Color32 color, bool isOn)
    {
        this.area = area;
        this.slotIndexes = slotIndexes;
        this.color = color;
        this.isOn = isOn;
    }

    public override void StartCommandExecution()
    {
        foreach (int slotIndex in slotIndexes)
            area.HighlightCard(slotIndex, color, isOn);
        Command.CommandExecutionComplete();
    }
}
