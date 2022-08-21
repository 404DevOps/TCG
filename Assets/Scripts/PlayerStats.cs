using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class PlayerStats : NetworkBehaviour
{
    public Owner owner;

    public NetworkIdentity Identity;
    //Display PlayerStats
    public TextMeshProUGUI txtName;
    public TextMeshProUGUI txtGold;
    public TextMeshProUGUI txtHealth;
    public TextMeshProUGUI txtDamage;

    public int DamagePool { get; private set; }
    public int HealthPool { get; private set; }
    public int GoldPool { get; private set; }

    public DamageButton damageButton;

    // Start is called before the first frame update
    void Start()
    {
        txtName.text = owner == Owner.Player ? "Player" : "Enemy";
        DamagePool = 0;
        HealthPool = 50;
        GoldPool = 0;
        UpdateTexts();
    }

    public void AddDamageToPool(int damageAmount)
    {
        DamagePool += damageAmount;
        txtDamage.text = DamagePool.ToString();

        if (DamagePool > 0 && !damageButton.isActive)
            damageButton.SetActive();
        else if(DamagePool <= 0 && damageButton.isActive)
        {
            damageButton.SetInactive();
        }
    }
    public void AddGoldToPool(int goldAmount)
    {
        GoldPool += goldAmount;
        txtGold.text = GoldPool.ToString();
    }

    public void AddHealthToPool(int healthAmount)
    {
        HealthPool += healthAmount;
        txtHealth.text = HealthPool.ToString();

        if (HealthPool < 1)
            GameManager.Instance.GameOver();
    }

    // Update is called once per frame
    void UpdateTexts()
    {
        txtDamage.text = DamagePool.ToString();
        txtGold.text = GoldPool.ToString();
        txtHealth.text = HealthPool.ToString();        
    }
}
