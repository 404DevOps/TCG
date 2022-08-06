using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MarketDeck : MonoBehaviour
{
    public List<CardBase> cards;
    public GameObject marketField;


    // Start is called before the first frame update

    public CardBase DrawNextCard()
    {
        if (cards.Any())
        {
            var drawnCard = cards[0];
            cards.RemoveAt(0);
            return drawnCard;
        }
        else
            return null;
    }
}
