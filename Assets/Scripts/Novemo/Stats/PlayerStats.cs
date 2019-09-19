using Novemo.Items;
using Novemo.Player;

namespace Novemo.Stats
{
    public class PlayerStats : CharacterStats
    {
        void Start()
        {
            EquipmentManager.Instance.onEquipmentChanged += OnEquipmentChanged;
        }

        void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
        {
            if (newItem != null)
            {
                foreach (var stat in stats)
                {
                    foreach (var modifier in newItem.Modifiers)
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
                    foreach (var modifier in oldItem.Modifiers)
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

        public override void Die()
        {
            base.Die();
            PlayerManager.Instance.KillPlayer();
        }
    }
}