using Novemo.Player;
using Novemo.Stats;
using UnityEngine;

namespace Novemo
{
	public class Metrics : MonoBehaviour
	{
		private CharacterStats _stats;
		
		private void Start()
		{
			_stats = PlayerManager.Instance.player.GetComponent<CharacterStats>();
		}

		public float GetCurrentFraction(bool health)
		{
			if (health)
				return _stats.CurrentHealth / _stats.stats[0].GetValue();
			
			return _stats.CurrentMana / _stats.stats[1].GetValue();
		}

		public static void CalculateScale(Stat stat, string scaleType, float modifierValue)
		{
			stat.modifiers[scaleType] = stat.modifiers.ContainsKey(scaleType)
				? stat.modifiers[scaleType] > 0
					? modifierValue * (stat.GetValue() - stat.modifiers[scaleType])
					: modifierValue * stat.GetValue()
				: modifierValue * stat.GetValue();
		}
	}
}
