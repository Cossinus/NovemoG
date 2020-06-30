using UnityEngine;

namespace Novemo.Items.Create
{
	public partial class CreateItem
	{
		public static void CreateNewScroll(int baseLevel, bool guaranteedQuality)
		{
			var scroll = ScriptableObject.CreateInstance<Scroll>();
		}
	}
}