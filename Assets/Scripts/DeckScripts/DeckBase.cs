using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class DeckBase : MonoBehaviour, IPointerClickHandler
{
    public List<CardBase> cards;
    public Owner owner;
    public GameObject deckVisual;

    public virtual void Start()
    {
        InitializeDeck();
    }

    public  virtual void InitializeDeck()
    {
        
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
       
    }

    public void ShuffleDeck()
    {
        if(!cards.Any())
            Debug.Log("Cant shuffle, no cards in this Deck.");

        var listShuffled = new List<CardBase>();
        var iterations = cards.Count;
        for (int i = 0; i < iterations; i++)
        {
            var randomIndex = Random.Range(0, cards.Count);
            
            //pick random card and remove it from list
            var pickedCard = cards[randomIndex];
            cards.RemoveAt(randomIndex);

            //add random card to new list
            listShuffled.Add(pickedCard);
        }

        cards =  listShuffled;
    }

    public CardBase DrawNextCard()
    {
        if (cards.Any())
        {
            //always draw first aka top card
            var drawCard = cards[0];
            cards.RemoveAt(0);
            if (cards.Count == 0)
                HideDeck();
            return drawCard;
        }
        else 
        {
            return null;
        }
    }
    public void HideDeck()
    {
        deckVisual.SetActive(false);
    }

    public void ShowDeck()
    {
        deckVisual.SetActive(true);
    }
}
