using System.Linq;
using Novemo.Characters.Player;
using Novemo.Controllers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Novemo.Items.Create
{
	public partial class CreateItem : MonoBehaviour
	{
		private static Inventories.Inventory _inventory;

		private void Start() => _inventory = PlayerManager.Instance.player.GetComponent<PlayerController>().inventory;

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.P))
			{
				CreateNewPotion(Random.Range(1,5), false);
			}
			if (Input.GetKeyDown(KeyCode.O))
			{
				CreateNewPotion(Random.Range(6,10), false);
			}
			if (Input.GetKeyDown(KeyCode.L))
			{
				CreateNewPotion(Random.Range(11,15), false);
			}
			if (Input.GetKeyDown(KeyCode.K))
			{
				CreateNewPotion(Random.Range(16,20), false);
			}
		}

		private static float CalculatePowerWithRarity(float power, Rarity rarity)
		{
			switch (rarity)
			{
				case Rarity.Common:
					power *= 0.5f;
					break;
				case Rarity.Normal:
					power *= 0.7f;
					break;
				case Rarity.Uncommon:
					power *= 0.85f;
					break;
				case Rarity.Rare:
					power *= 0.95f;
					break;
				case Rarity.VeryRare:
					power *= 1.05f;
					break;
				case Rarity.Epic:
					power *= 1.13f;
					break;
				case Rarity.Legendary:
					power *= 1.21f;
					break;
				case Rarity.Mystical:
					power *= 1.32f;
					break;
				case Rarity.Artifact:
					power *= 1.5f;
					break;
			}
			return power;
		}
		
		private static void CalculateRarity(Item item, int baseLevel, bool guaranteedQuality)
		{
			var choose = new RarityRandomizer();

			var probabilities = Metrics.RarityProbabilities;
			
			if (guaranteedQuality) {
				item.itemRarity = (Rarity)choose.Choose(probabilities, probabilities.Last(), true);
				item.level = Random.Range(baseLevel, baseLevel + 2);
			} else
			{
				item.itemRarity = (Rarity)choose.Choose(probabilities, probabilities.Last(), false);
				item.level = Random.Range(baseLevel - 2, baseLevel + 1);
			}
			
			if (item.level < 1)
			{
				item.level = 1;
			}
		}
	}
}