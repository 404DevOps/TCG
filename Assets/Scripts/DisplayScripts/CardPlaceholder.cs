using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardPlaceholder : MonoBehaviour
{
    public CardBase card;

    public InstantiatedField instantiatedIn;

    public GameObject creatureCardPrefab;
    public GameObject starterCardPrefab;
    public GameObject actionCardPrefab;

    private void OnEnable()
    {
        DisplayCard();
    }
    private void Start()
    {
        DisplayCard();
    }

    // Start is called before the first frame update
    void DisplayCard()
    {
        GameObject newCard = null;
        if (card is CreatureCard)
        {
            newCard = creatureCardPrefab;
            var display = newCard.GetComponent<DisplayCreature>();
            display.card = (CreatureCard)card;
            display.FillCard();

        }
        if (card is ActionCard)
        {
            newCard = actionCardPrefab;
            var display = newCard.GetComponent<DisplayAction>();
            display.card = (ActionCard)card;
            display.FillCard();
        }
        if (card is StarterCard)
        {
            newCard = starterCardPrefab;
            var display = newCard.GetComponent<DisplayStarter>();
            display.card = (StarterCard)card;
            display.FillCard();
        }
        if (newCard != null)
        {
            
            
            var newObject = Instantiate(newCard, transform.parent);
            //attach different behaviours based on where the card is instatiated
            switch (instantiatedIn)
            {
                case InstantiatedField.PlayerField:
                    if (card is CreatureCard)
                    {
                        newObject.AddComponent<Tapable>();
                    }
                    if (card is MarketCard)
                    {
                        var mC = (MarketCard)card;
                        if (mC.sacrificeEffect.Any())
                        {
                            newObject.AddComponent<Sacrificable>();
                        }
                    }
                    break;
                case InstantiatedField.Hand: newObject.AddComponent<Draggable>(); break;
                case InstantiatedField.Market: newObject.AddComponent<Buyable>(); break;
                case InstantiatedField.EnemyField:
                    if (card is CreatureCard)
                    {
                        newObject.AddComponent<Damageable>();
                    }
                    break;
            }
                
                //Destroy Placeholder
            Destroy(gameObject);
        }
    }
}
