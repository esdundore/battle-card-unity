using UnityEngine;
using UnityEngine.UI;
using TMPro;

// this class should be attached to the deck
// generates new cards and places them into the hand
public class DeckVisual : MonoBehaviour {

    public Image CardBack;
    public float HeightOfOneCard = 0.03f;
    public TMP_Text DeckSizeText;

    void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, -HeightOfOneCard * cardsInDeck);
        DeckSizeText.text = cardsInDeck.ToString();
    }

    private int cardsInDeck = 0;
    public int CardsInDeck
    {
        get{ return cardsInDeck; }

        set
        {
            cardsInDeck = value;
            transform.position = new Vector3(transform.position.x, transform.position.y, -HeightOfOneCard * value);
            DeckSizeText.text = value.ToString();
            if (value < 1)
            {
                Color tempColor = CardBack.color;
                tempColor.a = .25f;
                CardBack.color = tempColor;
            }
            else
            {
                Color tempColor = CardBack.color;
                tempColor.a = 1f;
                CardBack.color = tempColor;
            }
        }
    }
   
}
