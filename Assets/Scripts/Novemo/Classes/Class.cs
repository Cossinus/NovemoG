using System;
using Novemo.Abilities;
using Novemo.Character;
using Novemo.Items;
using Novemo.Stats;
using UnityEngine;

namespace Novemo.Classes
{
    public abstract class Class : ScriptableObject
    {
        public string className;
        public string classDescription;

        public Passive classPassive;
        
        public RangeType rangeType;
        public Role role;
        public DamageType damageType;

        public Sprite classIcon;

        public Equipment defaultWeapon;

        [NonSerialized] public Characters.Character myStats;

        public abstract void AddComponents();
        
        public abstract void InitializeValues();

        public virtual void LevelUp()
        {
            myStats.level++;
            myStats.RequiredExperience *= myStats.experienceMultiplier;
        }
    }

    [Serializable]
    public struct Passive
    {
        public Ability passive;
        public Sprite passiveIcon;
        public string passiveName;
        public string passiveDescription;
    }
    
    public enum RangeType
    {
        Melee,
        Ranged,
        Thrower
    }

    public enum Role
    {
        Tank,
        Ranger,
        Assassin,
        DamageDealer,
        Support,
        Sniper
    }

    public enum DamageType
    {
        Physical,
        Magical,
        LethalPhysical,
        LethalMagical,
        Mixed,
        MixedLethal
    }
}
