using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Buyable : MonoBehaviour, IPointerClickHandler
{
    Card card;

    DisplayPlayerStats player;
    DiscardPile discardPile;
    MarketDeck marketDeck;

    public void Start()
    {
        player = FindObjectsOfType<DisplayPlayerStats>().Where(m => m.playerData.Owner == Owner.Player).FirstOrDefault();
        discardPile = FindObjectsOfType<DiscardPile>().Where(m => m.owner == Owner.Player).FirstOrDefault();
        marketDeck = FindObjectOfType<MarketDeck>();
        card = GetComponent<DisplayCard>().card;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (player.playerData.GoldPool >= card.cost)
        {
            player.AddGoldToPool(-card.cost);
            //put in player discard pile
            discardPile.AddCardToPile(card);
            //get new card from market deck and put in market field
            marketDeck.AddCardToMarket(transform.GetSiblingIndex());
            //destroy market card
            Destroy(gameObject);
        }
        else 
        {
            GameManager.Instance.ShowMessage("Not enough Money", Color.red);
        }
    }
}
