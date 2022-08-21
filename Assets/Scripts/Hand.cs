using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public Owner owner;

    public GameObject cardPlaceholder;
    public GameObject canvas;

    public List<Card> cards;
    // Start is called before the first frame update
    void Start()
    {
        cards = new List<Card>();
    }

    public void AddHandCard(Card card)
    {
        var newCard = Instantiate(cardPlaceholder, transform);
        var placeHolder = newCard.GetComponent<CardPlaceholder>();
        placeHolder.card = card;
        placeHolder.instantiatedIn = InstantiatedField.Hand;
        placeHolder.DisplayCard();

        newCard.transform.SetParent(transform);
        newCard.transform.SetAsLastSibling();
    }
}
