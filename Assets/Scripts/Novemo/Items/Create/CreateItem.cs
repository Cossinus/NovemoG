using Novemo.Characters.Player;
using Novemo.Controllers;
using Novemo.Items.Equipments;
using Novemo.StatusEffects;
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
				CreateNewScroll(Random.Range(1,5), false);
			}
			if (Input.GetKeyDown(KeyCode.L))
			{
				CreateNewEquipment<RangeWeapon>(Random.Range(1,5), false);
			}
			if (Input.GetKeyDown(KeyCode.K))
			{
				CreateNewEffect<ActiveEffect>(Random.Range(1,5), -1, StatusEffect.EffectType.Active);
			}
		}

		private static float ComputePowerWithRarity(float power, Rarity rarity)
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
	}
}