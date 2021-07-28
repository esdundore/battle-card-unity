using UnityEngine;
public class UpdateBreederCommand : Command
{
    private CardAreaVisual area;
    private int slotIndex;
    private Breeder breeder;
    private Deck deck;

    public UpdateBreederCommand(CardAreaVisual area, int slotIndex, Breeder breeder, Deck deck)
    {
        this.area = area;
        this.slotIndex = slotIndex;
        this.breeder = breeder;
        this.deck = deck;
    }

    public override void StartCommandExecution()
    {
        area.UpdateBreeder(slotIndex, breeder, deck);
        Command.CommandExecutionComplete();
    }
}
