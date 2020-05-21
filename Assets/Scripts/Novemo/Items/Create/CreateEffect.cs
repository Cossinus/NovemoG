using System;
using Novemo.Status_Effects;

namespace Novemo.Items.Create
{
	public partial class CreateItem
	{
		public static void CreateNewEffect()
		{
			var effectType = Metrics.NextBoolean(new System.Random());

			//TODO randomize from all status effects and set its values and type
			
			if (effectType)
			{
				var uniqueEffect = Activator.CreateInstance<StatusEffect>();
			}
			else
			{
				var uniqueEffect = Activator.CreateInstance<ActiveEffect>();
			}
		}
	}
}