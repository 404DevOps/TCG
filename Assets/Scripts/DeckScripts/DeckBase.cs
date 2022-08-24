using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class DeckBase : NetworkBehaviour, IPointerClickHandler
{
    //only store guid in cardlist
    public readonly SyncList<string> cards = new SyncList<string>();
    public Owner owner;
    public GameObject deckVisual;

    public virtual void Start()
    {
        //InitializeDeck();
    }

    public  virtual void InitializeDeck()
    {
        
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
       
    }

    [Server]
    public string DrawNextCard()
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

    [ClientRpc]
    public void HideDeck()
    {
        deckVisual.SetActive(false);
    }

    [ClientRpc]
    public void ShowDeck()
    {
        deckVisual.SetActive(true);
    }
}
