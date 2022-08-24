using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Buyable : MonoBehaviour, IPointerClickHandler
{
    Card card;

    Player player;
    DiscardPile discardPile;
    MarketDeck marketDeck;

    public void Start()
    {
        player = FindObjectsOfType<Player>().Where(m => m.isLocalPlayer).FirstOrDefault();
        discardPile = FindObjectsOfType<DiscardPile>().Where(m => m.owner == Owner.Player).FirstOrDefault();
        marketDeck = FindObjectOfType<MarketDeck>();
        card = GetComponent<DisplayCard>().card;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (player.GoldPool >= card.cost)
        {
            player.AddToGoldPool(-card.cost);
            //put in player discard pile
            discardPile.AddCardToPile(card.Id);
            //get new card from market deck and put in market field
            marketDeck.AddCardToMarketField(transform.GetSiblingIndex());
            //destroy market card
            Destroy(gameObject);
        }
        else 
        {
            GameManager.Instance.ShowMessage("Not enough Money", Color.red);
        }
    }
}
