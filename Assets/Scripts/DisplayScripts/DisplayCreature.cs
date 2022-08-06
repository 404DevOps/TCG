using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayCreature : MonoBehaviour
{
    public CreatureCard card;

    public TextMeshProUGUI txtHealth;
    public TextMeshProUGUI txtName;
    public TextMeshProUGUI txtCost;
    public Image imgTaunt;
    public Image imgFaction;
    public Image imgSprite;
    public Image imgBorder;

    public GameObject abilityContainer;
    public TextMeshProUGUI abilityTextTemplate;

    // Start is called before the first frame update
    public void FillCard()
    {
        txtHealth.text = card.health.ToString();
        txtName.text = card.name.ToString();
        txtCost.text = card.cost.ToString();
        imgTaunt.color = new Color(imgTaunt.color.r, imgTaunt.color.g, imgTaunt.color.b, card.hasTaunt ? 255 : 0);

        var factionColor = ColorHelper.GetFactionColor(card.faction);
        imgFaction.color = factionColor;
        imgBorder.color = factionColor;

        //todo: display other ability types
        foreach (var fx in card.instantEffect)
        {
            var ability = Instantiate(abilityTextTemplate, abilityContainer.transform);
            ability.text = fx.GetType().ToString();
            ability.gameObject.SetActive(true);
        }
    }

    
}
