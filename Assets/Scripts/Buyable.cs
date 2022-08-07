using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Buyable : MonoBehaviour, IPointerClickHandler
{
    MarketCard card;
    public void OnPointerClick(PointerEventData eventData)
    {
        card = (MarketCard)GetComponent<DisplayBase>().card;

        var player = FindObjectsOfType<PlayerStats>().Where(m => m.owner == Owner.Player).FirstOrDefault();
        var discardPile = FindObjectsOfType<DiscardPile>().Where(m => m.owner == Owner.Player).FirstOrDefault();
        var marketDeck = FindObjectOfType<MarketDeck>();

        if (player.GoldPool >= card.cost)
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
            Debug.Log("Not enough Money");
        }
    }
}
