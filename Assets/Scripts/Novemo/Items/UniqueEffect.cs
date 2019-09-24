using System;
using System.Collections;
using Novemo.Items.UniqueEffects;
using UnityEngine;

namespace Novemo.Items
{
    public class UniqueEffect : ScriptableObject
    {
        public string eName = "New Effect";
        public string eDescription = "New Description";
        public bool canCastOnSelf;
        public EffectType eType;
    }

    public enum EffectType
    {
        Passive, Active
    }
}
