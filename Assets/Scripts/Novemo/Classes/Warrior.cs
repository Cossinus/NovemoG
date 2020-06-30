using System;
using Novemo.Abilities.WarriorAbilities;
using Novemo.Characters.Player;
using UnityEngine;

namespace Novemo.Classes
{
    [CreateAssetMenu(fileName = "Warrior", menuName = "Classes/Warrior")]
    public class Warrior : Class
    {
        public override void AddComponents()
        {
            classPassive.passive = PlayerManager.Instance.player.AddComponent<WarriorPassive>();
            PlayerManager.Instance.player.AddComponent<Charge>();
            PlayerManager.Instance.player.AddComponent<ThickSkin>();
            PlayerManager.Instance.player.AddComponent<AbleBodied>();
            //PlayerManager.Instance.player.AddComponent<>();
        }

        public override void InitializeValues()
        {
            classPassive.passiveDescription = "Gives you (<color=#ff3232><b>+3%</b></color> max HP) " +
                                              "and from the start player has <color=#ff3232><b>25 HP</b></color> more." +
                                              $"{Environment.NewLine}Actually: (<color=#ff3232><b>+{myStats.GetScaledValueByMultiplier(0, 0.03f):F1} HP</b></color>)";
            
            classPassive.passive.Passive(myStats);
        }

        public override float Damage(DamageType dmgType)
        {
            throw new NotImplementedException();
        }

        public override void LevelUp()
        {
            base.LevelUp();
            myStats.stats[0].AddBaseValue(5);       // Health
            myStats.ModifyHealth(5);       // Current Health
            myStats.stats[1].AddBaseValue(2);       // Mana
            myStats.stats[2].AddBaseValue(3);       // Damage Max Value
            myStats.stats[3].AddBaseValue(2);       // Armor
            myStats.stats[4].AddBaseValue(1.75f);   // Magic Resist
            myStats.stats[7].AddBaseValue(0.02f);   // Health Regen
            myStats.stats[8].AddBaseValue(0.0175f); // Mana Regen
            myStats.stats[21].AddBaseValue(0.1f);   // Pair Chance
            myStats.stats[22].AddBaseValue(0.08f);  // Block Chance
        }
    }
}