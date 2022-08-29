using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public Card card;
    public DiscardPile discardPile;

    private void Start()
    {
        card = GetComponent<DisplayCard>().card;
    }
}
