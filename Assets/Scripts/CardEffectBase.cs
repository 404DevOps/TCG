using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardEffectBase
{
    public EffectType effectType;
    public int amount;
    public bool targetsCreature;
    public Target target;
    public string Description;

    public string GetEffectDesc()
    {
        return Description.Replace("{0}", amount.ToString());
    }
}
