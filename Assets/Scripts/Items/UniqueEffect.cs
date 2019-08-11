using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Items/Effect")]
public class UniqueEffect : ScriptableObject
{
    public string effectName = string.Empty;
    [TextArea(2, 5)]
    public string effectDescription = string.Empty;
    public Item item;

    // Can be for every type of item
    public virtual void PassiveEffect()
    {
        
    }

    // Only for weapons
    public virtual void UsableEffect()
    {
        
    }

    public string EffectText()
    {
        if (effectName != string.Empty)
        {
            return $"\n<color=yellow><size=20><b>{effectName}</b></size></color>\n" +
                   $"<color=#ffffe0><size=18><i>{effectDescription}</i></size></color>";
        }
        else
        {
            return null;
        }
    }
}
