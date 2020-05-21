using UnityEngine;

namespace Novemo.Items.Equipments
{
	[CreateAssetMenu(fileName = "New Weapon", menuName = "Items/Equipment/Weapon")]
	public class ColdSteel : Equipment
	{
		[Header("Blade")]
		public Item edge;
		public Item core;

		[Header("Handle")]
		public Item guard;
		public Item grip;
		
		//[HideInInspector] public Rune runeSocket;
	}
}