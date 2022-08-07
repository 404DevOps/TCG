using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public Owner owner;
    public TextMeshProUGUI txtName;

    public TextMeshProUGUI txtGold;
    public TextMeshProUGUI txtHealth;
    public TextMeshProUGUI txtDamage;

    public int DamagePool;
    public int HealthPool;
    public int GoldPool;

    // Start is called before the first frame update
    void Start()
    {
        txtName.text = owner == Owner.Player ? "Gimu" : "Enemy";
        DamagePool = 0;
        HealthPool = 50;
        GoldPool = 0;
        UpdateTexts();
    }

    public void AddDamageToPool(int damageAmount)
    {
        DamagePool += damageAmount;
        txtDamage.text = DamagePool.ToString();
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
