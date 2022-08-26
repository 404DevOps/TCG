using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DiscardPile : NetworkBehaviour
{
    public Owner owner;
    public GameObject cardPlaceholderPrefab;

    [Client]
    public void AddCardToPile(string cardId)
    {
        //Remove old Visual
        EmptyPile();

        var newCard = Instantiate(cardPlaceholderPrefab, transform);
        var placeholder = newCard.GetComponent<CardPlaceholder>();
        placeholder.instantiatedIn = InstantiatedField.DiscardPile;

        //get card from game manager list
        placeholder.card = GameManager.Instance.allCards.Where(c => c.Id == cardId).FirstOrDefault();
        placeholder.DisplayCard();

    }

    [Client]
    public void EmptyPile()
    {
        //destroy placeholder and instantiate new one
        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }
}
