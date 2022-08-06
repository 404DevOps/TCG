using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Creature", menuName = "Creature Card")]
public class CreatureCard : MarketCard
{
    //creature cards
    public uint health;
    public bool hasTaunt;
    public List<CardEffectBase> tapEffect;
}
