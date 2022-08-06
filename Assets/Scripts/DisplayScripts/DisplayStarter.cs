using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayStarter : MonoBehaviour
{
    public StarterCard card;

    public TextMeshProUGUI txtName;
    public Image imgSprite;
    public Image imgBorder;

    public GameObject abilityContainer;
    public TextMeshProUGUI abilityTextTemplate;

    // Start is called before the first frame update
    public void FillCard()
    {
        txtName.text = card.name.ToString();
        imgBorder.color = ColorHelper.GetFactionColor(Faction.Neutral);

        //todo: display other ability types
        foreach (var fx in card.instantEffect)
        {
            var ability = Instantiate(abilityTextTemplate, abilityContainer.transform);
            ability.text = fx.GetType().ToString();
            ability.gameObject.SetActive(true);
        }
    }
}
