using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorHelper
{
    public static Color GetFactionColor(Faction faction)
    {
        switch (faction)
        {
            case Faction.Wild:
                return Color.green;
            case Faction.Guild:
                return Color.blue;
            case Faction.Necros:
                return Color.red;
            case Faction.Imperial: 
                return Color.yellow;
            default: 
                return Color.white;
        }
    }
}
