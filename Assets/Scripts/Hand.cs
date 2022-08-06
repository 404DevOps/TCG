using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public Owner owner;

    public GameObject cardPlaceholder;
    public GameObject canvas;

    public List<CardBase> cards;
    // Start is called before the first frame update
    void Start()
    {
        cards = new List<CardBase>();
    }

    public void AddHandCard(CardBase card)
    {
        var newCard = Instantiate(cardPlaceholder, canvas.transform);
        var cardDisplay = newCard.GetComponent<CardPlaceholder>();
        cardDisplay.card = card;

        newCard.transform.SetParent(transform);
        newCard.transform.SetAsLastSibling();
    }
}
