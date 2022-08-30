using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MarketDeck : DeckBase
{
    public GameObject marketField;
    public GameObject placeholderPrefab;

    public readonly SyncList<string> marketCards = new SyncList<string>();

    public List<Card> creatureCards;
    public List<Card> actionCards;

    //Server repares the Deck
    [Server]
    public override void InitializeDeck()
    {
        var allCards = new List<string>();

        //only take market cards, so creatures and actions
        allCards = GameManager.Instance.allCards.Where(c => c.cardType == CardType.Action || c.cardType == CardType.Creature).Select(c => c.Id).ToList();

        //Add each cards to market deck 3 times
        foreach (var c in allCards)
        {
            for (int i = 0; i < 3; i++)
            {
                cards.Add(c);
            }
        }

        cards.ShuffleDeck();
    }

    [Server]
    public void InitializeMarketField()
    {
        //add first five cards to market
        for (int i = 0; i < 5; i++)
        {
            AddCardToMarketField();
        }
    }

    [Server]
    public void AddCardToMarketField(int index = 0)
    {
        //add card to synclist on server
        var cardId = DrawNextCard();

        //cant draw new card cause deck is empty. => game ends.
        if (cardId == null)
            GameManager.Instance.GameOver();
        marketCards.Add(cardId);

        SpawnMarketCardRpc(index, cardId);
    }

    [ClientRpc]
    public void SpawnMarketCardRpc(int index, string cardId)
    {
        //make card visible for clients

        var newCard = Instantiate(placeholderPrefab, marketField.transform);
        var ph = newCard.GetComponent<CardPlaceholder>();
        ph.instantiatedIn = InstantiatedField.Market;
        ph.card = GameManager.Instance.allCards.Where(c => c.Id == cardId).FirstOrDefault();
        ph.DisplayCard();

        //Set New Market card to correct position
        newCard.transform.SetSiblingIndex(index);
    }

    [ClientRpc]
    public void RemoveMarketCardRpc(int index)
    {
        Debug.Log("Removing Market Card " + index);

        if (index == -1)
        {
            marketField.transform.Clear();
        }
        else 
        {
            if (marketField.transform.childCount > 0)
                Destroy(marketField.transform.GetChild(index).gameObject);
        }
    }
}
