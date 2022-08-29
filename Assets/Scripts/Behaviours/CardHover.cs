using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardHover : NetworkBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public Card card;
    public GameObject canvas;
    public GameObject cardPlaceholderPrefab;
    // Start is called before the first frame update
    void Start()
    {
        card = GetComponent<DisplayCard>().card;
        canvas = GameObject.Find("Canvas");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //dont do this when drawing line
        if (GameObject.Find("RedArrow") != null)
        {
            return;
        }
        var newObject = Instantiate(cardPlaceholderPrefab, canvas.transform);
        var placeholder = newObject.GetComponent<CardPlaceholder>();
        placeholder.card = card;
        placeholder.isZoomCard = true;
        placeholder.DisplayCard();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DestroyZoomCard();
    }

    private void DestroyZoomCard()
    {
        //Destroy Zoom Card again.
        var cards = FindObjectsOfType<DisplayCard>().Where(dc => dc.isZoomCard);

        foreach (var c in cards)
            Destroy(c.gameObject);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        DestroyZoomCard();
    }
}
