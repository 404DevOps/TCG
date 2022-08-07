using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayAction : DisplayBase
{
    //public ActionCard card;

    //public TextMeshProUGUI txtName;
    public TextMeshProUGUI txtCost;
    public Image imgFaction;
    //public Image imgSprite;
    //public Image imgBorder;

    //public GameObject abilityContainer;
    //public TextMeshProUGUI abilityTextTemplate;

    // Start is called before the first frame update
    public override void FillCard()
    {
        var aCard = (ActionCard)card;

        txtName.text = card.name.ToString();
        txtCost.text = aCard.cost.ToString();

        var factionColor = ColorHelper.GetFactionColor(aCard.faction);
        if (aCard.faction != Faction.Neutral)
        {
            imgFaction.color = factionColor;
        }
        else 
        {
            //remove faction image if its a firegem
            imgFaction.gameObject.SetActive(false);
        }

        imgBorder.color = factionColor;
    }
}
