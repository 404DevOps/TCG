using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EffectExecutor : NetworkBehaviour
{

    [Server]
    public static void Heal(Player player, int amount)
    {
        player.AddToHealthPool(amount);
    }
    [Server]
    public static void AddGold(Player player, int amount)
    {
        player.AddToGoldPool(amount);
    }
    [Server]
    public static void AddDamage(Player player, int amount)
    {
        player.AddToDamagePool(amount);
    }

    [Server]
    public void DealDamage(Card target, int amount)
    { 
        //if(target.health <= amount)
        //{

        //    player.AddToDamagePool(-target.health);
        //    throw new NotImplementedException();
        //}
    }

    [Server]
    public void DealDamageToPlayer()
    {
       // enemy.AddToDamagePool(-player.DamagePool);
       // player.AddToHealthPool(-player.DamagePool);
    }

}
