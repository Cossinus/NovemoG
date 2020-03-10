using Novemo.Controllers;
using Novemo.Items;
using Novemo.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Novemo.Stats
{
    public class PlayerStats : CharacterStats
    {
        public Image experienceSlider;

        public int CraftSkill { get; set; } = 1;

        private PlayerController _controller;

        private void Start()
        {
            EquipmentManager.Instance.onEquipmentChanged += OnEquipmentChanged;
            GetComponent<CharacterStats>().OnExperienceChanged += SetExperienceBar;
            _controller = GetComponent<PlayerController>();
        }

        private void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
        {
            if (newItem != null)
            {
                foreach (var stat in stats)
                {
                    foreach (var modifier in newItem.modifiers)
                    {
                        if (stat.statName == modifier.Name)
                        {
                            switch (stat.statName)
                            {
                                case "Health":
                                    var healthFraction = metrics.GetCurrentFraction(true);
                                    stat.AddModifier(modifier.Name, modifier.Value);
                                    SetCurrentStat(0, healthFraction);
                                    
                                    OnHealthChangeInvoke();
                                    break;
                                case "Mana":
                                    var manaFraction = metrics.GetCurrentFraction(false);
                                    stat.AddModifier(modifier.Name, modifier.Value);
                                    SetCurrentStat(1, manaFraction);
                                    
                                    OnManaChangeInvoke();
                                    break;
                                default:
                                    stat.AddModifier(modifier.Name, modifier.Value);
                                    break;
                            }
                        }
                    }
                }
            }

            if (oldItem != null)
            {
                foreach (var stat in stats)
                {
                    foreach (var modifier in oldItem.modifiers)
                    {
                        if (stat.statName == modifier.Name)
                        {
                            switch (stat.statName)
                            {
                                case "Health":
                                    var healthFraction = metrics.GetCurrentFraction(true);
                                    stat.RemoveModifier(modifier.Name, modifier.Value);
                                    SetCurrentStat(0, healthFraction);

                                    OnHealthChangeInvoke();
                                    break;
                                case "Mana":
                                    var manaFraction = metrics.GetCurrentFraction(false);
                                    stat.RemoveModifier(modifier.Name, modifier.Value);
                                    SetCurrentStat(1, manaFraction);

                                    OnManaChangeInvoke();
                                    break;
                                default:
                                    stat.RemoveModifier(modifier.Name, modifier.Value);
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private void SetExperienceBar(float requiredExperience, float currentExperience)
        {
            var experiencePercent = currentExperience / requiredExperience;
            experienceSlider.fillAmount = experiencePercent;
        }

        protected override void LevelUp()
        {
            if (!(CurrentExperience >= RequiredExperience)) return;
            var moveToNext = CurrentExperience - RequiredExperience;

            _controller.playerClass.LevelUp();
                
            experienceSlider.fillAmount = 0;

            CurrentExperience = 0;
            CurrentExperience += moveToNext;
            base.LevelUp();
        }

        protected override void Die()
        {
            base.Die();
            PlayerManager.Instance.KillPlayer();
        }
    }
}