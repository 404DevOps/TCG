using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public Card card;

    public TextMeshProUGUI txtAttack;
    public TextMeshProUGUI txtDefense;
    public TextMeshProUGUI txtName;

    public Image imgSprite;

    // Start is called before the first frame update
    void Start()
    {
        txtAttack.text = card.attack.ToString();
        txtDefense.text = card.defense.ToString();
        txtName.text = card.name.ToString();
    }
}
