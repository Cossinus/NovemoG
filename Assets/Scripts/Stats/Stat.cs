using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    public string Name;
    public float BaseValue;
    public float Power; // Power of debuffs
    public int Number; // number that Stat occupies in a List
    private List<float> modifiers = new List<float>();
    
    public float GetValue()
    {
        float finalValue = BaseValue;
        modifiers.ForEach(x => finalValue += x);
        return finalValue;
    }

    public void AddModifier(float modifier)
    {
        if (modifier != 0)
        {
            modifiers.Add(modifier);
        }
    }
    
    public void RemoveModifier(float modifier)
    {
        if (modifier != 0)
        {
            modifiers.Remove(modifier);
        }
    }
}
