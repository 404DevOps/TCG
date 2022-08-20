using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardBase : ScriptableObject
{
    [Header("Base Info")]
    public Owner owner;
    [ReadOnly] 
    public string Id = Guid.NewGuid().ToString();

    public new string name;
    public Sprite sprite;
    public bool isPermanent;

    [Header("Card Effects")]
    public List<CardEffectBase> instantEffect;

    //public void Discard()
    //{
    //    var discardPile = FindObjectsOfType<DiscardPile>().Where(m => m.owner == Owner.Player).First();
        
    //}

    public void ApplyInstantEffects()
    {
        foreach (var fx in instantEffect)
        {
            switch (fx.effectType)
            {
                case EffectType.Heal:
                    EffectExecutor.Instance.Heal(fx.amount);
                    break;
                case EffectType.AddDamage:
                    EffectExecutor.Instance.AddDamage(fx.amount);
                    break;
                case EffectType.AddGold:
                    EffectExecutor.Instance.AddGold(fx.amount);
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
