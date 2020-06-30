using System;
using System.Collections.Generic;
using UnityEngine;

namespace Novemo.Items
{
	[CreateAssetMenu(fileName = "New Gem", menuName = "Items/Gem")]
	public class Gem : Item
	{
		public GemType gemType;
		public Hardness hardness;

		public Property properties;

		[Multiline]
		public string gemDescription;

		private const int DefaultValue = 150;

		public void OnValidate()
		{
			stackLimit = 1;
			
			if (properties.gemColor == Color.Default)
			{
				itemName = gemType.ToString();
			}
			else
			{
				itemName = $"{properties.gemColor} " + gemType;
			}

			value = CalculateValue();
		}

		public override void SetDescription()
		{
			itemDescription =
				$"{gemDescription}{Environment.NewLine}<color=#00FFFF>Properties:{Environment.NewLine}Hardness: {hardness}" +
				$"{Environment.NewLine}Strength: {properties.strength}{Environment.NewLine}Clearness: {properties.clearness}</color>";
		}

		private int CalculateValue()
		{
			float cValue = DefaultValue;
			
			cValue *= 1f + properties.strength;
			cValue *= 1f + properties.clearness;
			
			cValue *= 1f + (float) properties.gemColor / 5f;
			
			return Mathf.RoundToInt(cValue);
		}
	}
	
	[Serializable]
	public struct Property
	{
		[Range(0,1)]
		public float strength;

		[Range(0,1)]
		public float clearness;
		
		public Color gemColor;
	}
	
	public enum Color
	{
		Default = 1,
		Yellow = 2,
		Blue = 3,
		Pink = 4,
		Red = 5,
		Black = 6
	}

	public enum GemType
	{
		Amethyst,
		Aquamarine,
		Beryl,
		Citrine,
		Dawnstone,
		Divinine,
		Diamond,
		Emerald,
		Elismuth,
		Garnet,
		Kunzite,
		Obsidian,
		Peridot,
		Pearl,
		Quartz,
		Ruby,
		Sapphire,
		Sunstone,
		Topaz,
		Zircon
	}
		
	public enum Hardness
	{
		Iron = 1,
		Aluminum = 2,
		Quartz = 3,
		Steel = 4,
		StainlessSteel = 5,
		Obsidian = 6,
		Titanium = 7,
		Diamond = 8
	}
}