using UnityEngine;

namespace Novemo.Items.Create
{
	public partial class CreateItem
	{
		public static void CreateNewEquipment()
		{
			var equipment = ScriptableObject.CreateInstance<Equipment>();
		}
	}
}