using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class FireGemDeck : DeckBase
{
    public DiscardPile discardPile;
    public Player player;
    public string card;

    public Card fireGemCard;

    [Client]
    public override void OnPointerClick(PointerEventData eventData)
    {
        if(player == null)
            player = FindObjectsOfType<Player>()?.Where(m => m.isLocalPlayer)?.First();
        //send command to server to check if possible to buy && add
        if (player.GoldPool >= fireGemCard.cost)
        {
            player.AddToGoldPool(-fireGemCard.cost);
            //put in player discard pile
            discardPile.AddCardToPile(card);
        }
        else 
        {
            GameManager.Instance.ShowMessage("Not enough Money.", Color.red);
        }
    }

    [Server]
    public override void InitializeDeck()
    {
        
        card = DrawNextCard();
        fireGemCard = GameManager.Instance.allCards.Where(c => c.cardType == CardType.FireGem).FirstOrDefault();

        for (int i = 0; i <= 16; i++)
        {
            cards.Add(fireGemCard.Id);
        }
    }
}
