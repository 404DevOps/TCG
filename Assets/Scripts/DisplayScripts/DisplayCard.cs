using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisplayCard : MonoBehaviour
{
    public Card card;

    //Creature Card Attributes
    public TextMeshProUGUI txtHealth;
    public Image imgTaunt;

    //Market Card Attributes
    public TextMeshProUGUI txtCost;
    public Image costContainer;
    public Image imgFaction;

    public TextMeshProUGUI txtName;
    public Image imgSprite;
    public Image imgBorder;

    public GameObject abilityContainer;
    public GameObject abilityTextTemplate;

    private void Start()
    {
    }
    public void FillCard()
    {
        //for all cards fill base info
        var factionColor = ColorHelper.GetFactionColor(card.faction);
        imgBorder.color = factionColor;
        txtName.text = card.name.ToString();

        //market cards & firegems
        if (card.cardType == CardType.Action || card.cardType == CardType.Creature || card.cardType == CardType.FireGem)
        {
            txtCost.text = card.cost.ToString();
            txtCost.enabled = true;
            costContainer.enabled = true;

            if (card.cardType != CardType.FireGem)
            {
                imgFaction.color = factionColor;
                imgFaction.enabled = true;
            }
                
            else
                imgFaction.enabled = false;
        }

        if (card.cardType == CardType.Creature)
        {
            txtHealth.text = card.health.ToString();
            imgTaunt.color = new Color(imgTaunt.color.r, imgTaunt.color.g, imgTaunt.color.b, card.hasTaunt ? 255 : 0);
        }
        else 
        {
            imgTaunt.enabled = false;
            txtHealth.enabled = false;
        }

        //only starters have no cost
        if (card.cardType == CardType.Starter)
        {
            txtCost.enabled = false;
            costContainer.enabled = false;
            imgFaction.enabled = false;
        }

        //Display all Card Effects.
        foreach (var fx in card.instantEffect)
        {
            AddEffectText(fx.GetEffectDesc());
        }
        foreach (var fx in card.tapEffect)
        {
            AddEffectText(fx.GetEffectDesc());
        }
        foreach (var fx in card.comboEffect)
        {
            AddEffectText(fx.GetEffectDesc());
        }
        foreach (var fx in card.sacrificeEffect)
        {
            AddEffectText(fx.GetEffectDesc());
        }
    }

    public void AddEffectText(string text)
    {
        var newText = Instantiate(abilityTextTemplate, abilityContainer.transform);
        newText.GetComponent<TextMeshProUGUI>().text = text;
        newText.SetActive(true);
    }
}
