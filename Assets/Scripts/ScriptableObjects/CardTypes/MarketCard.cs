using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketCard : CardBase
{
    //market cards
    public Faction faction;
    public int cost;
    public List<CardEffectBase> comboEffect;
    public List<CardEffectBase> sacrificeEffect; //firegems are market cards too
}
