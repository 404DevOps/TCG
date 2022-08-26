using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class DisplayPlayerStats : NetworkBehaviour
{
    public Owner Owner;

    public TextMeshProUGUI txtName;
    public TextMeshProUGUI txtGold;
    public TextMeshProUGUI txtHealth;
    public TextMeshProUGUI txtDamage;

    [Client]
    public void UpdateTexts(int? gold, int? damage, int? health)
    {
        if(damage != null)
            txtDamage.text = damage.ToString();
        if(gold != null)
            txtGold.text = gold.ToString();
        if(health != null)
            txtHealth.text = health.ToString();        
    }

    [Client]
    public void SetPlayerName(string name)
    {
        txtName.text = name;
    }
}
