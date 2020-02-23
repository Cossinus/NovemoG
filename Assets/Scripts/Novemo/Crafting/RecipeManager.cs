using System;
using System.Collections.Generic;
using Novemo.Items;
using UnityEngine;

namespace Novemo.Crafting
{
    public class RecipeManager : MonoBehaviour
    {
        #region Singleton

        public static RecipeManager Instance;

        private void Awake()
        {
	        Instance = this;
	        ProcessingCrafting();
        }

		#endregion

		public List<Recipe> playerRecipes = new List<Recipe>();
		public List<Recipe> blacksmithRecipes = new List<Recipe>();
		public List<Recipe> armorerRecipes = new List<Recipe>();
		public List<Recipe> shadowsRecipes = new List<Recipe>();
		public Dictionary<string, Item> processingRecipe = new Dictionary<string, Item>();

		private void ProcessingCrafting()
        {
            processingRecipe.Add("IronBar", Resources.Load<Item>("Items/CraftingMaterials/IronPlate"));
        }

        public void UnlockRecipe(Item item)
        {
	        item.recipe.Discovered = true;
        }
    }
}