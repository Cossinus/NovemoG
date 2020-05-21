using UnityEngine;

namespace Novemo.Items.Create
{
	public partial class CreateItem
	{
		public static void CreateNewScroll()
		{
			var scroll = ScriptableObject.CreateInstance<Scroll>();
		}
	}
}