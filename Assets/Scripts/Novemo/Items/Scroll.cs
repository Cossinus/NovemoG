namespace Novemo.Items
{
	public class Scroll : Item
	{
		public UniqueEffect uniqueEffect;

		public override bool Equals(Item other)
		{
			var otherScroll = (Scroll)other;
			return base.Equals(other) && uniqueEffect == otherScroll.uniqueEffect;
		}
	}
}