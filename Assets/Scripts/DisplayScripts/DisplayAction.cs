using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayAction : MonoBehaviour
{
    public ActionCard card;

    public TextMeshProUGUI txtName;
    public TextMeshProUGUI txtCost;
    public Image imgFaction;
    public Image imgSprite;
    public Image imgBorder;

    public GameObject abilityContainer;
    public TextMeshProUGUI abilityTextTemplate;

    // Start is called before the first frame update
    public void FillCard()
    {
        txtName.text = card.name.ToString();
        txtCost.text = card.cost.ToString();

        var factionColor = ColorHelper.GetFactionColor(card.faction);
        imgFaction.color = factionColor;
        imgBorder.color = factionColor;

        //todo: display other ability types
        //foreach (var fx in card.instantEffect)
        //{
        //    var ability = Instantiate(abilityTextTemplate, abilityContainer.transform);
        //    ability.text = "SomeAction";//fx.GetType().ToString();
        //    ability.gameObject.SetActive(true);
        //}
    }

    
}
