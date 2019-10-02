using Novemo.Controllers;
using Novemo.Items;
using Novemo.Player;
using UnityEngine.UI;

namespace Novemo.Stats
{
    public class PlayerStats : CharacterStats
    {
        public Image experienceSlider;

        public int CraftSkill { get; set; } = 1;

        private PlayerController _controller;
        
        void Start()
        {
            EquipmentManager.Instance.onEquipmentChanged += OnEquipmentChanged;
            GetComponent<CharacterStats>().OnExperienceChanged += SetExperienceBar;
            _controller = GetComponent<PlayerController>();
        }

        void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
        {
            if (newItem != null)
            {
                foreach (var stat in stats)
                {
                    foreach (var modifier in newItem.modifiers)
                    {
                        if (stat.statName == modifier.Name)
                        {
                            stat.AddModifier(modifier.Name, modifier.Value);
                            if (stat.statName == "Health")
                                CurrentHealth += modifier.Value;
                            if (stat.statName == "Mana")
                                CurrentMana += modifier.Value;
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
                            stat.RemoveModifier(modifier.Name, modifier.Value);
                            if (stat.statName == "Health")
                                CurrentHealth -= modifier.Value;
                            if (stat.statName == "Mana")
                                CurrentMana -= modifier.Value;
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