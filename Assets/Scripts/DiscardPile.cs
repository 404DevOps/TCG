using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardPile : MonoBehaviour
{
    public Owner owner;
    public List<CardBase> cards;

    CardPlaceholder topCard;
    // Start is called before the first frame update
    private void Start()
    {
        cards = new List<CardBase>();
    }

    void AddCardToPile(CardBase card)
    {
        cards.Add(card);
        topCard.card = card;
        topCard.gameObject.SetActive(true);

    }
    List<CardBase> EmptyPile()
    {
        var pile = cards;
        cards = new List<CardBase>();
        return pile;
        
    }
}
