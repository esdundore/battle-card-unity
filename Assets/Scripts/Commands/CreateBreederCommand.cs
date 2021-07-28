public class CreateBreederCommand : Command
{

    private CardAreaVisual area;
    private Breeder breeder;
    private Deck deck;
    private int slotIndex;

    public CreateBreederCommand(CardAreaVisual area, Breeder breeder, Deck deck, int slotIndex)
    {
        this.area = area;
        this.breeder = breeder;
        this.deck = deck;
        this.slotIndex = slotIndex;
    }

    public override void StartCommandExecution()
    {
        area.CreateBreeder(slotIndex, breeder, deck);
        Command.CommandExecutionComplete();
    }
}