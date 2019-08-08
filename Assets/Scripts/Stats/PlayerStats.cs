using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                    if (stat.Name == modifier.Name)
                        stat.AddModifier(modifier.Value);
                }
            }
        }

        if (oldItem != null)
        {
            foreach (var stat in stats)
            {
                foreach (var modifier in oldItem.Modifiers)
                {
                    if (stat.Name == modifier.Name)
                        stat.RemoveModifier(modifier.Value);
                }
            }
        }
    }
}
