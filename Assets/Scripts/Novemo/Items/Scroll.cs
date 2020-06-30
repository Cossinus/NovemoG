using Novemo.StatusEffects;
using UnityEngine;

namespace Novemo.Items
{
	public class Scroll : Item
	{
		[SerializeField] public StatusEffect statusEffect;

		public override bool Equals(Item other)
		{
			var otherScroll = (Scroll)other;
			return otherScroll != null && base.Equals(other) && statusEffect.Equals(otherScroll.statusEffect);
		}
	}
}