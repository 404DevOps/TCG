using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisplayBase : MonoBehaviour
{
    public CardBase card;

    public TextMeshProUGUI txtName;
    public Image imgSprite;
    public Image imgBorder;

    public GameObject abilityContainer;
    public GameObject abilityTextTemplate;

    private void Start()
    {
        foreach (var effect in card.instantEffect)
        {
            AddEffectText(effect.GetEffectDesc());
        }
    }
    public virtual void FillCard()
    { 
    
    }

    public void AddEffectText(string text)
    {
        var newText = Instantiate(abilityTextTemplate, abilityContainer.transform);
        newText.GetComponent<TextMeshProUGUI>().text = text;
        newText.SetActive(true);
    }
}
