using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hand : MonoBehaviour
{
    //public readonly SyncList<string> handCards = new SyncList<string>();
    public GameObject cardPlaceholder;
    public GameObject cardBack;
    public Owner owner;

    //[Server]
    //public void AddHandCard(string cardId)
    //{
    //    handCards.Add(cardId);
    //    AddHandCardRpc(cardId);
    //}

    public void AddHandCard(string cardId)
    {
        GameObject newCard = null;
        if (owner == Owner.Player)
        {
            newCard = Instantiate(cardPlaceholder, transform);
            var placeHolder = newCard.GetComponent<CardPlaceholder>();

            placeHolder.card = GameManager.Instance.allCards.Where(c => c.Id == cardId).FirstOrDefault();
            placeHolder.instantiatedIn = InstantiatedField.PlayerHand;
            placeHolder.DisplayCard();
        }
        else 
        {
            newCard = Instantiate(cardBack, transform);
        }

        newCard.transform.SetParent(transform);
        newCard.transform.SetAsLastSibling();
    }

    public void RemoveCard(int index)
    {
        if (index == -1)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
        else 
        {
            Destroy(transform.GetChild(index).gameObject);
        }
    }
}
