using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayCreature : DisplayBase
{
    //public CreatureCard card;

    public TextMeshProUGUI txtHealth;
    //public TextMeshProUGUI txtName;
    public TextMeshProUGUI txtCost;
    public Image imgTaunt;
    public Image imgFaction;
    //public Image imgSprite;
    //public Image imgBorder;

    //public GameObject abilityContainer;
    //public TextMeshProUGUI abilityTextTemplate;

    // Start is called before the first frame update
    public override void FillCard()
    {
        var cCard = (CreatureCard)card;

        txtHealth.text = cCard.health.ToString();
        txtName.text = cCard.name.ToString();
        txtCost.text = cCard.cost.ToString();
        imgTaunt.color = new Color(imgTaunt.color.r, imgTaunt.color.g, imgTaunt.color.b, cCard.hasTaunt ? 255 : 0);

        var factionColor = ColorHelper.GetFactionColor(cCard.faction);
        imgFaction.color = factionColor;
        imgBorder.color = factionColor;

        //todo: display other ability types
        foreach (var fx in cCard.tapEffect)
        {
            AddEffectText(fx.GetEffectDesc());
        }
    }
}
