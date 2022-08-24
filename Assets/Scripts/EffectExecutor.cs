using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EffectExecutor : NetworkBehaviour
{
    public static EffectExecutor Instance;

    public Player player;
    public Player enemy;

    private void Start()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }
    }

    [Client]
    private void Update()
    {
        if (player == null)
            player = NetworkClient.localPlayer?.gameObject.GetComponent<Player>();
        else if (enemy == null)
            player = GameManager.Instance.players.FirstOrDefault(p => p.netId != player.netId);
    }
    [Server]
    public void Heal(int amount)
    {
        player.AddToHealthPool(amount);
    }
    [Server]
    public void AddGold(int amount)
    {
        player.AddToGoldPool(amount);
    }
    [Server]
    public void AddDamage(int amount)
    {
        player.AddToDamagePool(amount);
    }

    [Server]
    public void DealDamage(Card target, int amount)
    { 
        if(target.health <= amount)
        {

            player.AddToDamagePool(-target.health);
            throw new NotImplementedException();
        }
    }

    [Server]
    public void DealDamageToPlayer()
    {
        enemy.AddToDamagePool(-player.DamagePool);
        player.AddToHealthPool(-player.DamagePool);
    }

}
