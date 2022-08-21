using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardPile : MonoBehaviour
{
    public Owner owner;
    public List<Card> cards;

    public GameObject cardPlaceholderPrefab;
    // Start is called before the first frame update
    private void Start()
    {
        cards = new List<Card>();
    }

    public void AddCardToPile(Card card)
    {
        DestroyChildObjects();

        cards.Add(card);

        var newCard = Instantiate(cardPlaceholderPrefab, transform);
        var placeholder = newCard.GetComponent<CardPlaceholder>();
        placeholder.instantiatedIn = InstantiatedField.DiscardPile;
        placeholder.card = card;
    }
    public List<Card> EmptyPile()
    {
        //destroy placeholder
        DestroyChildObjects();

        var pile = cards;
        cards = new List<Card>();
        return pile;
        
    }

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
