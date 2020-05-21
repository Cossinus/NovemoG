using UnityEngine;

namespace Novemo.Items.Equipments
{
	[CreateAssetMenu(fileName = "New Bow", menuName = "Items/Equipment/Bow")]
	public class Bow : Equipment
	{
		[Header("Handle")]
		public Item limb;
		public Item grip;

		public Item @string;

		//[HideInInspector] public Rune runeSocket;
	}
}