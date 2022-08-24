using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Field : MonoBehaviour
{
    public Owner owner;

    public GameObject cardPlaceholder;
    public GameObject cardBack; //to play flip cards at some point

    public void AddFieldCard(int index, string cardId)
    {
        var newCard = Instantiate(cardPlaceholder, transform);
        var placeHolder = newCard.GetComponent<CardPlaceholder>();

        placeHolder.card = GameManager.Instance.allCards.Where(c => c.Id == cardId).FirstOrDefault();
        placeHolder.instantiatedIn = owner == Owner.Player ? InstantiatedField.PlayerField : InstantiatedField.EnemyField;
        placeHolder.DisplayCard();

        newCard.transform.SetSiblingIndex(index);

        //TODO Refactor or sum, its only gonna destroy a card with same id like this.
        if (!placeHolder.card.isPermanent)
        {
            
            Debug.Log("Refactor WaitForDiscard");
            return;
            StartCoroutine(WaitForDiscard(cardId, index));
        }
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

    IEnumerator WaitForDiscard(string cardId, int index)
    {
        yield return new WaitForSeconds(1);

        var discardPile = FindObjectsOfType<DiscardPile>().Where(m => m.owner == Owner.Player).First();
        discardPile.AddCardToPile(cardId);
        RemoveCard(index);
    }
}
