using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPlaceholder : MonoBehaviour
{
    public CardBase card;

    public bool instantiatedInHand;
    public bool instantiatedInMarket;

    public GameObject creatureCardPrefab;
    public GameObject starterCardPrefab;
    public GameObject actionCardPrefab;

    private void OnEnable()
    {
        DisplayCard();
    }
    private void Start()
    {
        DisplayCard();
    }

    // Start is called before the first frame update
    void DisplayCard()
    {
        GameObject newCard = null;
        if (card is CreatureCard)
        {
            newCard = creatureCardPrefab;
            var display = newCard.GetComponent<DisplayCreature>();
            display.card = (CreatureCard)card;
            display.FillCard();

        }
        if (card is ActionCard)
        {
            newCard = actionCardPrefab;
            var display = newCard.GetComponent<DisplayAction>();
            display.card = (ActionCard)card;
            display.FillCard();
        }
        if (card is StarterCard)
        {
            newCard = starterCardPrefab;
            var display = newCard.GetComponent<DisplayStarter>();
            display.card = (StarterCard)card;
            display.FillCard();
        }
        if (newCard != null)
        {
            
            
            var newObject = Instantiate(newCard, transform.parent);
            if (instantiatedInHand)
                newObject.AddComponent<Draggable>();
            if (instantiatedInMarket)
                newObject.AddComponent<Buyable>();
                //Destroy Placeholder
            Destroy(gameObject);
        }
    }
}
