using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardPile : MonoBehaviour
{
    public Owner owner;
    public List<CardBase> cards;

    public GameObject cardPlaceholderPrefab;
    // Start is called before the first frame update
    private void Start()
    {
        cards = new List<CardBase>();
    }

    public void AddCardToPile(CardBase card)
    {
        DestroyChildObjects();

        cards.Add(card);

        var newCard = Instantiate(cardPlaceholderPrefab, transform);
        var placeholder = newCard.GetComponent<CardPlaceholder>();
        placeholder.instantiatedIn = InstantiatedField.DiscardPile;
        placeholder.card = card;
    }
    public List<CardBase> EmptyPile()
    {
        //destroy placeholder
        DestroyChildObjects();

        var pile = cards;
        cards = new List<CardBase>();
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
