using System;
using Novemo.Items;
using UnityEngine;

namespace Novemo.Crafting
{
	[CreateAssetMenu(fileName = "New Recipe", menuName = "Recipes/Recipe")]
	public class Recipe : ScriptableObject
	{
		public Item[] Ingredients;
		public RecipeType Type;
		public bool Discovered;
	}

	public enum RecipeType
	{
		Player,
		Blacksmith,
		Armorer,
		Shadow,
		Processing
	}
}
