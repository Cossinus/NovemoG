using Novemo.Abilities;
using Novemo.Controllers;
using Novemo.Items;
using Novemo.Items.Equipments;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Novemo.Characters.Player
{
    public class Player : Character
    {
        public TextMeshProUGUI levelText;
        
        public Image experienceSlider;

        public int CraftSkill { get; set; } = 1;

        private PlayerController _controller;

        private void Start()
        {
            TargetType = TargetType.Player;
            EquipmentManager.Instance.onEquipmentChanged += OnEquipmentChanged;
            GetComponent<Character>().OnExperienceChanged += SetExperienceBar;
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
                        if (stat.statName == modifier.name)
                        {
                            switch (stat.statName)
                            {
                                case "Health":
                                    var healthFraction = Metrics.GetCurrentFraction(true, this);
                                    stat.AddModifier(modifier.name, modifier.value);
                                    SetCurrentStat(0, healthFraction);
                                    
                                    OnHealthChangeInvoke();
                                    break;
                                case "Mana":
                                    var manaFraction = Metrics.GetCurrentFraction(false, this);
                                    stat.AddModifier(modifier.name, modifier.value);
                                    SetCurrentStat(1, manaFraction);
                                    
                                    OnManaChangeInvoke();
                                    break;
                                default:
                                    stat.AddModifier(modifier.name, modifier.value);
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
                        if (stat.statName == modifier.name)
                        {
                            switch (stat.statName)
                            {
                                case "Health":
                                    var healthFraction = Metrics.GetCurrentFraction(true, this);
                                    stat.RemoveModifier(modifier.name, modifier.value);
                                    SetCurrentStat(0, healthFraction);

                                    OnHealthChangeInvoke();
                                    break;
                                case "Mana":
                                    var manaFraction = Metrics.GetCurrentFraction(false, this);
                                    stat.RemoveModifier(modifier.name, modifier.value);
                                    SetCurrentStat(1, manaFraction);

                                    OnManaChangeInvoke();
                                    break;
                                default:
                                    stat.RemoveModifier(modifier.name, modifier.value);
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

        public override void LevelUp()
        {
            var moveToNext = CurrentExperience - RequiredExperience;

            level++;
            levelText.text = level.ToString();
            
            _controller.PlayerClass.LevelUp();
                
            experienceSlider.fillAmount = 0;

            CurrentExperience = 0;
            AddExperience(moveToNext);
            base.LevelUp();
        }
    }
}