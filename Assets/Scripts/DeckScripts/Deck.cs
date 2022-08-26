using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class Deck : DeckBase
{
    //maybe use for "You May Draw a Card, if you Do, Discard one.
    public override void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Moved to Server");

    }
}
