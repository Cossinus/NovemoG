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

        private RarityRandomizer _randomizer = new RarityRandomizer();
        private int[] _probabilities = Metrics.RarityProbabilities;

        private void Start()
        {
            TargetType = TargetType.Enemy;
            OnCharacterDeath += Die;

            var multiplier = 1 + stars / 5f;

            foreach (var stat in stats)
                stat.AddWholeModifier(multiplier, this);
        }

        public void RollRarity()
        {
            enemyRarity = (Rarity)_randomizer.Choose(_probabilities, _probabilities.Last(), false);
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
