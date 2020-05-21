using System;
using Random = System.Random;

namespace Novemo.Items
{
	public class RarityRandomizer
	{
		private readonly Random _random = new Random((int)(DateTime.UtcNow.Ticks & 0x7FFFFFFF));

		public int Choose(int[] probabilities, int maxProbability, bool guaranteedQuality)
		{
			var value = 0;
			
			var randomValue = guaranteedQuality
				? _random.Next(probabilities[4], maxProbability)
				: _random.Next(maxProbability);

			while (probabilities[value] <= randomValue) value++;
			
			return value;
		}
	}
}