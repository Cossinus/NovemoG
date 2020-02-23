using System.Collections.Generic;
using Novemo.Items;
using UnityEngine;
using UnityEngine.Serialization;

namespace Novemo.Crafting
{
	[CreateAssetMenu(fileName = "New Recipe", menuName = "Recipes/Recipe")]
	public class Recipe : ScriptableObject
	{
		public List<Item> Ingredients;
		public RecipeType recipeType;
		public int craftSkillRequired;
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
