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

    //deal damage is raised on player action, therefore command
    [Command(requiresAuthority = false)]
    public void DealDamage(Player player, Player enemy, string fieldId)
    {

        //if target is a card;
        var cardId = enemy.fieldCards.First(c => c.fieldId == fieldId).cardId;
        var card = GameManager.Instance.allCards.FirstOrDefault(c => c.Id == cardId);

        //if not attacking taunt creature, check if theres no taunt creature that needs to be attacked before.
        if (!card.hasTaunt)
        {
            if (HasTauntCards(enemy))
            {
                if (player.isLocalPlayer)
                    GameManager.Instance.RpcMessage("Attack Creatures with Taunt first.", Color.red);
                return;
            }
        }
        
        if (card.health <= player.DamagePool)
        {
            //subtract from damage pool
            player.AddToDamagePool(-card.health);
            //remove card that was damaged and put on discard pile.
            enemy.fieldCards.RemoveAll(c => c.fieldId == fieldId);
            enemy.discardCards.Add(card.Id);
        }
        else
        {
            if (player.isLocalPlayer)
                GameManager.Instance.RpcMessage("Not enough Damage to Stun that Creature", Color.red);
        }
    }

    [Server]
    private bool HasTauntCards(Player enemy)
    {
        foreach (var c in enemy.fieldCards)
        {
            var card = GameManager.Instance.allCards.FirstOrDefault(card => card.Id == c.cardId);
            if (card.hasTaunt)
                return true;
        }

        return false;
    }

    [Command(requiresAuthority = false)]
    public void DealDamageToPlayer(Player player, Player enemy)
    {
        if (HasTauntCards(enemy))
        {
            if (player.isLocalPlayer)
                GameManager.Instance.RpcMessage("Attack Creatures with Taunt first.", Color.red);
            return;
        }

        //always deal all damage at once.
        enemy.AddToHealthPool(-player.DamagePool);
        player.DamagePool.Value = 0;
    }

}
