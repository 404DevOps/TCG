using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPlaceholder : MonoBehaviour
{
    public CardBase card;

    public GameObject creatureCardPrefab;
    public GameObject starterCardPrefab;
    public GameObject actionCardPrefab;

    // Start is called before the first frame update
    void Start()
    {
        GameObject newCard = new GameObject();
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

        Instantiate(newCard, transform.parent);
        //Destroy Placeholder
        Destroy(gameObject);

    }
}
