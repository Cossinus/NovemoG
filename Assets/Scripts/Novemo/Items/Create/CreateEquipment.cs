using Novemo.Items.Equipments;
using UnityEngine;

namespace Novemo.Items.Create
{
	public partial class CreateItem
	{
		public static Equipment CreateNewEquipment<T>(/*Random enum value from WeaponType, */int baseLevel, bool guaranteedQuality) where T : Equipment
		{
			var equipment = ScriptableObject.CreateInstance<T>();

			return equipment;
		}
	}
}