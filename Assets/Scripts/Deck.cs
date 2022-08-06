using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class Deck : MonoBehaviour, IPointerClickHandler
{
    public List<CardBase> cards;

    public Owner owner;

    public Hand playerHand;
    public DiscardPile discardPile;
    public GameObject cardBack;

    public void Start()
    {
        InitializeStarterDeck();
    }

    private void InitializeStarterDeck()
    {
        var listCards = new List<CardBase>();
        CardBase dagger = Resources.Load<StarterCard>("Cards/Starters/Dagger");
        CardBase sword = Resources.Load<StarterCard>("Cards/Starters/Sword");
        CardBase gold = Resources.Load<StarterCard>("Cards/Starters/Gold");
        CardBase ruby = Resources.Load<StarterCard>("Cards/Starters/Ruby");

        listCards.Add(dagger);
        listCards.Add(sword);
        listCards.Add(ruby);
        while (listCards.Count < 7)
            listCards.Add(gold);

        cards = ShuffleDeck(listCards);

        Debug.Log("Initialized Starter Deck, Count = " + cards.Count);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (owner == Owner.Player)
        {
            if (cards.Any())
            {
                Debug.Log("Draw CardBase from Player Deck");
                var card = DrawNextCard();
                if (card != null)
                    playerHand.AddHandCard(card);
            }
            else 
            {
                ResetDeck();
            }
        }
    }

    public void ResetDeck()
    {
        var shuffledDeck = ShuffleDeck(discardPile.cards);
        cards = shuffledDeck;
        if (cards.Any()) 
        {
            Debug.Log("Resfhuffled");
            cardBack.SetActive(true);
        }
        else
            Debug.Log("No Cards were reshuffleld");
    }

    List<CardBase> ShuffleDeck(List<CardBase> cards)
    {
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

        return listShuffled;
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
    void HideDeck()
    {
        cardBack.SetActive(false);
    }
}
