using Novemo.Characters.Player;
using UnityEngine;

namespace Novemo.Items
{
	[CreateAssetMenu(fileName = "New Loot Chest", menuName = "Items/LootChest")]
	public class LootChest : Item
	{
		[SerializeField] private LootTable lootTable = null;

		public override bool Use()
		{
			var itemToDrop = lootTable.RollItem(PlayerManager.Instance.player.GetComponent<Characters.Character>().stats[27].GetValue());
            
			return itemToDrop != null && Inventories.Inventory.Instance.AddItem(Instantiate(itemToDrop));
		}
	}
}