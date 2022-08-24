using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DiscardPile : NetworkBehaviour
{
    public Owner owner;
    public readonly SyncList<string> cards = new SyncList<string>();

    public GameObject cardPlaceholderPrefab;

    [ClientRpc]
    public void AddCardToPile(string cardId)
    {
        //Destroy Current TopCard
        DestroyChildObjects();

        cards.Add(cardId);


        var newCard = Instantiate(cardPlaceholderPrefab, transform);
        var placeholder = newCard.GetComponent<CardPlaceholder>();
        placeholder.instantiatedIn = InstantiatedField.DiscardPile;

        //get card from game manager list
        placeholder.card = GameManager.Instance.allCards.Where(c => c.Id == cardId).FirstOrDefault();
    }

    [Server]
    public List<string> EmptyPile()
    {
        //destroy placeholder
        DestroyChildObjects();

        var pile = cards.ToList();
        cards.RemoveAll(id => id != null);
        return pile;
    }

    [Client]
    void DestroyChildObjects()
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
