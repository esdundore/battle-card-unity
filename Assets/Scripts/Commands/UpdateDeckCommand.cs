public class UpdateDeckCommand : Command
{

    private DeckVisual deckVisual;
    private int deckSize;


    public UpdateDeckCommand(DeckVisual deckVisual, int deckSize)
    {
        this.deckVisual = deckVisual;
        this.deckSize = deckSize;
    }

    public override void StartCommandExecution()
    {
        deckVisual.CardsInDeck = deckSize;
        Command.CommandExecutionComplete();
    }
}
