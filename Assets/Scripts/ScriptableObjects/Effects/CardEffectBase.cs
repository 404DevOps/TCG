using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardEffectBase : MonoBehaviour
{
    bool meetsCondition;
    public abstract void ApplyEffect();
}
