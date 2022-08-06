using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBase : ScriptableObject
{
    public new string name;
    public string description;
    public Sprite sprite;
    public bool isPermanent;

    public List<CardEffectBase> instantEffect;

}
