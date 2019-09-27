using System;
using System.Collections;
using Novemo.Items.UniqueEffects;
using UnityEngine;

namespace Novemo.Items
{
    public class UniqueEffect : ScriptableObject
    {
        public string effectName = "New Effect";
        [TextArea(2, 5)]
        public string effectDescription = "New Description";
        public EffectType effectType;
    }

    public enum EffectType
    {
        Passive, Active
    }
}
