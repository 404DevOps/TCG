using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectExecutor : MonoBehaviour
{
    public static EffectExecutor Instance;

    public PlayerStats player;
    public PlayerStats enemy;

    private void Start()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }
    }
    public void Heal(int amount)
    {
        player.AddHealthToPool(amount);
    }
    public void AddGold(int amount)
    {
        player.AddGoldToPool(amount);
    }
    public void AddDamage(int amount)
    {
        player.AddDamageToPool(amount);
    }
}