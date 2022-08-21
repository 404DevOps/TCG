using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class FireGemDeck : DeckBase
{
    public DiscardPile discardPile;
    public PlayerStats player;
    public Card card;

    public override void Start()
    {
        base.Start();
        player = FindObjectsOfType<PlayerStats>().Where(m => m.owner == Owner.Player).First();
        card = DrawNextCard();
    }
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (player.GoldPool >= card.cost)
        {
            player.AddGoldToPool(-card.cost);
            //put in player discard pile
            discardPile.AddCardToPile(card);
        }
        else 
        {
            GameManager.Instance.ShowMessage("Not enough Money.", Color.red);
        }
    }

    public override void InitializeDeck()
    {
        cards = new List<Card>();
        Card firegem = Resources.Load<Card>("Cards/Actions/FireGem");
        for (int i = 0; i <= 16; i++)
        {
            cards.Add(firegem);
        }
    }
}
