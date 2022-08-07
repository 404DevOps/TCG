using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MarketDeck : DeckBase
{
    public GameObject marketField;
    public GameObject placeholderPrefab;

    public override void Start()
    {
        InitializeDeck();

        //add first five cards to market
        for (int i = 0; i < 5; i++)
        {
            AddCardToMarket();
        }
    }

    public override void InitializeDeck()
    {
        var slime = Resources.Load<CreatureCard>("Cards/Creatures/Slime");
        var mage = Resources.Load<CreatureCard>("Cards/Creatures/Mage");
        var vamp = Resources.Load<CreatureCard>("Cards/Creatures/Vampire");
        var fireball = Resources.Load<ActionCard>("Cards/Actions/Fireball");
        var heal = Resources.Load<ActionCard>("Cards/Actions/Heal");
        var icebolt = Resources.Load<ActionCard>("Cards/Actions/Icebolt");
        for (int i = 0; i < 3; i++)
        {
            cards.Add(slime);
            cards.Add(mage);
            cards.Add(vamp);
            cards.Add(fireball);
            cards.Add(icebolt);
            cards.Add(heal);
        }

        ShuffleDeck();
    }


    public void AddCardToMarket(int index = 0)
    {
        var card = DrawNextCard();
        var newCard = Instantiate(placeholderPrefab, marketField.transform);
        var ph = newCard.GetComponent<CardPlaceholder>();
        ph.instantiatedInMarket = true;
        ph.card = card;

        //Set New Market card to correct position
        newCard.transform.SetSiblingIndex(index);
    }
}
