using System.Linq;
using Novemo.Abilities;
using Novemo.Items;
using UnityEngine;

namespace Novemo.Characters.Enemies
{
    public class EnemyStats : Character
    {
        [Header("Enemy Difficulty Settings")]
        public int stars;
        public Rarity enemyRarity;
        
        [Header("Container for Loot")]
        public LootTable lootTable;

        private void Start()
        {
            RollRarity();
            TargetType = TargetType.Enemy;
            OnCharacterDeath += Die;

            var multiplier = 1 + stars / 5f;

            foreach (var stat in stats)
                stat.AddWholeModifier(multiplier, this);

            CurrentHealth = stats[0].GetValue();
            CurrentMana = stats[1].GetValue();
        }

        private void RollRarity()
        {
            enemyRarity = Metrics.CalculateRarity(false);
        }
        
        private void Die(Character source, Character target)
        {
            // death animation
            
            var itemToDrop = lootTable.RollItem(source.stats[27].GetValue());
            
            if (itemToDrop != null) Inventories.Inventory.Instance.DropItem(itemToDrop, transform);

            Destroy(gameObject);
        }
    }
}
