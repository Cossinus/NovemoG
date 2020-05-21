using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Novemo.Items
{
	[Serializable]
	public class LootTable
	{
		[Serializable]
		public class Loot
		{
			public Item item;

			public float weight;
		}
		
		public List<Loot> loot;

		public Item RollItem(float luckChance)
		{
			var weightSum = 0;
			var roll = Random.Range(0, 101);

			foreach (var drop in loot)
			{
				if (drop.item != null) weightSum += Mathf.RoundToInt(drop.weight);
				else
				{
					drop.weight *= 1 - luckChance;
					weightSum += Mathf.RoundToInt(drop.weight);
				}

				if (roll < weightSum)
				{
					return drop.item;
				}
			}

			return null;
		}
	}
}