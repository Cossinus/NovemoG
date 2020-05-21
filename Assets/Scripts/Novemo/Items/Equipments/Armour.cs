using UnityEngine;

namespace Novemo.Items.Equipments
{
	[CreateAssetMenu(fileName = "New Armour", menuName = "Items/Equipment/Armour")]
	public class Armour : Equipment
	{
		[Header("Made Of")]
		public Item edge;
		public Item core;
		
		//[HideInInspector] public Rune runeSocket;
	}
}