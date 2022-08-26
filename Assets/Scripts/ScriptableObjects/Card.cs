using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    [Header("Base Info")]
    public Owner owner;
    [ReadOnly] 
    public string Id = Guid.NewGuid().ToString();

    public CardType cardType;

    public new string name;
    public Sprite sprite;

    //Only Creatures are Permanent.
    public bool isPermanent;

    [Header("Market Cards")]
    public Faction faction;
    public int cost;

    [Header("Creature Cards")]
    public int health;
    public bool hasTaunt;

    [Header("Card Effects")]
    public List<CardEffectBase> instantEffect;
    public List<CardEffectBase> comboEffect;
    public List<CardEffectBase> sacrificeEffect;
    public List<CardEffectBase> tapEffect;

    [Server]
    public void ApplyInstantEffects(Player player)
    {
        foreach (var fx in instantEffect)
        {
            switch (fx.effectType)
            {
                case EffectType.Heal:
                    EffectExecutor.Heal(player, fx.amount);
                    break;
                case EffectType.AddDamage:
                    EffectExecutor.AddDamage(player, fx.amount);
                    break;
                case EffectType.AddGold:
                    EffectExecutor.AddGold(player, fx.amount);
                    break;
                case EffectType.Stun:
                    Debug.Log("Stun not implemented");
                    break;
                default:
                    break;
            }
        }
    }

}

//struct used to identify field cards with guid
public struct FieldCard
{
    public FieldCard(string cId, string fId)
    {
        cardId = cId;
        fieldId = fId;
    }
    public string cardId;
    public string fieldId;
}
