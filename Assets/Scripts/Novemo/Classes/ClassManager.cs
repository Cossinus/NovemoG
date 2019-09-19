using Novemo.Abilities;
using Novemo.Items;
using Novemo.Player;
using Novemo.Stats;
using UnityEngine;

namespace Novemo.Classes
{
    public class ClassManager : MonoBehaviour
    {
        #region Singleton

        public static ClassManager Instance;

        void Awake()
        {
            Instance = this;
        }

        #endregion

        public string className;
        public string classDescription;

        public string passiveName;
        public string passiveDescription;

        public RangeType rangeType;
        public Role role;
        public DamageType damageType;

        public Equipment defaultWeapon;

        public Ability ability;

        public PlayerManager playerManager;
        public CharacterStats myStats;

        public virtual void LevelUp()
        {
            myStats.CurrentExperience = 0;
            myStats.level++;
        }
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
