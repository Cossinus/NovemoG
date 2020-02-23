using Novemo.Abilities.WarriorAbilities;
using Novemo.Items;
using Novemo.Player;
using Novemo.Stats;
using UnityEngine;

namespace Novemo.Classes
{
    public class Warrior : ClassManager
    {
        private void Awake()
        {
            className = "Warrior";
            classDescription = "Default";
            passiveName = "Thick Skin";
            
            rangeType = RangeType.Melee;
            damageType = DamageType.Physical;
            role = Role.Tank;
        }

        private void Start()
        {
            var player = PlayerManager.Instance.player;
            myStats = player.GetComponent<CharacterStats>();
            defaultWeapon = Resources.Load<Equipment>("Items/Weapons/WarriorSword");
            
            player.AddComponent<Charge>();
            player.AddComponent<ThickSkin>();
            //player.AddComponent<StrongMental>();
            //player.AddComponent<WrathFury>();
            
            myStats.CurrentHealth += 25;
            myStats.stats[0].baseValue += 25;
            myStats.stats[0].modifierValue += 0.03f;
            myStats.stats[0].Scale();
        }

        private void Update()
        {
            passiveDescription = "Gives you (<color=#ff3232><b>+3%</b></color> max HP) " +
                                 "and from the start player has <color=#ff3232><b>25 HP</b></color> more." +
                                 $"\nActually: (<color=#ff3232><b>+{myStats.GetScaledValueByMultiplier(0, 0.03f):F1} HP</b></color>)";
        }

        public override void LevelUp()
        {
            base.LevelUp();
            myStats.stats[0].baseValue += 5;       // Health
            myStats.CurrentHealth += 5;            // Current Health
            myStats.stats[1].baseValue += 2;       // Mana
            myStats.stats[2].baseValue += 3;       // Damage Max Value
            myStats.stats[3].baseValue += 2;       // Armor
            myStats.stats[4].baseValue += 1.75f;   // Magic Resist
            myStats.stats[7].baseValue += 0.02f;   // Health Regen
            myStats.stats[8].baseValue += 0.0175f; // Mana Regen
            myStats.stats[21].baseValue += 0.1f;   // Pair Chance
            myStats.stats[22].baseValue += 0.08f;  // Block Chance
        }
    }
}