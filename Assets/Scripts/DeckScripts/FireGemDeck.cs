using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class FireGemDeck : DeckBase
{
    public DiscardPile discardPile;

    public override void OnPointerClick(PointerEventData eventData)
    {
        var player = FindObjectsOfType<PlayerStats>().Where(m => m.owner == Owner.Player).First();
        var card = (MarketCard)DrawNextCard();

        if (player.GoldPool >= card.cost)
        {
            player.AddGoldToPool(-card.cost);
            //put in player discard pile
            discardPile.AddCardToPile(card);
        }
        else 
        {
            Debug.Log("Not enough money");
        }
    }

    public override void InitializeDeck()
    {
        cards = new List<CardBase>();
        CardBase firegem = Resources.Load<ActionCard>("Cards/Actions/FireGem");
        for (int i = 0; i <= 16; i++)
        {
            cards.Add(firegem);
        }
    }
}
