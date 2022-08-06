using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffect : CardEffectBase
{
    int DamageAmount;
    PlayerStats target;
    public override void ApplyEffect()
    {
        target.AddDamageToPool(DamageAmount);
    }
}
