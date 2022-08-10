using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public CreatureCard card;
    public DiscardPile discardPile;

    private void Start()
    {
        card = (CreatureCard)GetComponent<DisplayBase>().card;
    }

    public void ReceiveDamage(int amount)
    {
        if (amount - card.health >= 0)
        {
            return;
            //AddCardToDiscardPile(card);
            Destroy(gameObject);
        }
    }
}
