using System;
using Random = System.Random;

namespace Novemo.Items
{
	public class ChooseRarity
	{
		private const int MaxProbability = 2250;
		private readonly Random random = new Random((int)(DateTime.UtcNow.Ticks & 0x7FFFFFFF));

		private readonly int[] probabilities = {
			750, 1300, 1775, 2075, 2175, 2225, 2245, 2249, 2250
		};
		
		public Rarity Choose(bool guaranteedQuality)
		{
			Rarity rarityType = 0;
			
			var randomValue = guaranteedQuality
				? random.Next(probabilities[4], MaxProbability)
				: random.Next(MaxProbability);
			
			while (probabilities[(int) rarityType] <= randomValue)
			{
				rarityType++;
			}

			return rarityType;
		}
	}
}