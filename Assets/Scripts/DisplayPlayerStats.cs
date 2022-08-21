using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class DisplayPlayerStats : NetworkBehaviour
{
    public Owner Owner;
    public PlayerData playerData;

    public NetworkIdentity Identity;
    //Display PlayerStats
    public TextMeshProUGUI txtName;
    public TextMeshProUGUI txtGold;
    public TextMeshProUGUI txtHealth;
    public TextMeshProUGUI txtDamage;

    public DamageButton damageButton;

    //Start is called before the first frame update
    void Start()
    {
        playerData = GetComponent<PlayerData>();
        //txtName.text = playerData.Owner == Owner.Player ? "Player" : "Enemy";
        UpdateTexts(0,0,0);
    }

    public void AddDamageToPool(int damageAmount)
    {
        playerData.DamagePool += damageAmount;
        txtDamage.text = playerData.DamagePool.ToString();

        if (playerData.DamagePool > 0 && !damageButton.isActive)
            damageButton.SetActive();
        else if (playerData.DamagePool <= 0 && damageButton.isActive)
        {
            damageButton.SetInactive();
        }
    }
    public void AddGoldToPool(int goldAmount)
    {
        playerData.GoldPool += goldAmount;
        txtGold.text = playerData.GoldPool.ToString();
    }

    public void AddHealthToPool(int healthAmount)
    {
        playerData.HealthPool += healthAmount;
        txtHealth.text = playerData.HealthPool.ToString();

        if (playerData.HealthPool < 1)
            GameManager.Instance.GameOver();
    }

    // Update is called once per frame
    public void UpdateTexts(int gold, int damage, int health)
    {
        txtDamage.text = damage.ToString();
        txtGold.text = gold.ToString();
        txtHealth.text = health.ToString();        
    }

    public void SetPlayerName(string name)
    {
        txtName.text = name;
    }
}
