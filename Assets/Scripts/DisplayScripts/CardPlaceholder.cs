using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardPlaceholder : MonoBehaviour
{
    public Card card;

    public InstantiatedField instantiatedIn;

    public GameObject cardPrefab;

    private void OnEnable()
    {
        if(card != null)
            DisplayCard();
    }
    private void Start()
    {
        if (card != null)
            DisplayCard();
    }

    public void DisplayCard()
    {
        var newObject = Instantiate(cardPrefab, transform.parent);
        var display = newObject.GetComponent<DisplayCard>();
        display.card = card;
        display.FillCard();

        //attach different behaviours based on where the card is instatiated
        switch (instantiatedIn)
        {
            case InstantiatedField.PlayerField:
                if (card.tapEffect.Any())
                {
                    newObject.AddComponent<Tapable>();
                }
                if (card.sacrificeEffect.Any())
                {
                    newObject.AddComponent<Sacrificable>();
                }

                break;
            case InstantiatedField.Hand: newObject.AddComponent<Draggable>(); break;
            case InstantiatedField.Market: newObject.AddComponent<Buyable>(); break;
            case InstantiatedField.EnemyField:
                if (card.cardType == CardType.Creature)
                {
                    newObject.AddComponent<Damageable>();
                }
                break;
            case InstantiatedField.DiscardPile: Destroy(newObject.GetComponent<Draggable>()); break;
        }

        //Destroy Placeholder
        Destroy(gameObject);
    }
}
