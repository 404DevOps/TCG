using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class Deck : DeckBase
{
    public Hand playerHand;
    public DiscardPile discardPile;

    [Server]
    public override void InitializeDeck()
    {
        var listCards = new List<string>();
        
        var starters = GameManager.Instance.allCards.Where(c => c.cardType == CardType.Starter).Select(c => c.Id);
        var goldCardId = GameManager.Instance.allCards.FirstOrDefault(c => c.name == "Gold").Id;

        listCards.AddRange(starters);
        while (listCards.Count < 7)
            listCards.Add(goldCardId);

        foreach(var c in listCards)
            cards.Add(c);

        cards.ShuffleDeck();

        Debug.Log("Initialized Starter Deck, Count = " + cards.Count);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Moved to Server");
        //if (owner == Owner.Player)
        //{
        //    if (cards.Any())
        //    {
        //        if (playerHand.transform.childCount > 6)
        //        {
        //            GameManager.Instance.ShowMessage("Hand is already full", Color.red);
        //        }
        //        else
        //        {
        //            Debug.Log("Draw Card from Player Deck");
        //            var card = DrawNextCard();
        //            if (card != null)
        //                playerHand.AddHandCard(card);
        //        }
        //    }
        //    else
        //    {
        //        ResetDeck();
        //    }
        //}
    }

    public void ResetDeck()
    {
        //cards = discardPile.cards;
        cards.AddRange(discardPile.EmptyPile());

        cards.ShuffleDeck();

        if (cards.Any())
        {
            Debug.Log("Resfhuffled");
            ShowDeck();
        }
        else
            GameManager.Instance.ShowMessage("No Cards to reshuffle into Deck.", Color.red);
    }
}
