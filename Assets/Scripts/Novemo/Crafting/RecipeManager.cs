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
	        CreateBlueprints();

	        Instance = this;
        }

		#endregion

		public List<Recipe> playerRecipes = new List<Recipe>();
		public List<Recipe> blacksmithRecipes = new List<Recipe>();
		public List<Recipe> armorerRecipes = new List<Recipe>();
		public List<Recipe> shadowsRecipes = new List<Recipe>();
		public Dictionary<string, Item> processingRecipe = new Dictionary<string, Item>();
		/*public Dictionary<string, Item> playersCraftingRecipe = new Dictionary<string, Item>(); // 5 Ingredients recipe  (in player's layout)
		public Dictionary<string, Item> blacksmithsCraftingRecipe = new Dictionary<string, Item>();
		public Dictionary<string, Item> armorersCraftingRecipe = new Dictionary<string, Item>();
		public Dictionary<string, Item> shadowCraftingRecipe = new Dictionary<string, Item>();
		public Dictionary<string, Item> eliteRecipe = new Dictionary<string, Item>();             // 10 Ingredients recipe
		public Dictionary<string, Item> advancedRecipe = new Dictionary<string, Item>();          // 7 Ingredients recipe
		public Dictionary<string, Item> basicRecipe = new Dictionary<string, Item>();*/           // 5 Ingredients recipe  (in special layout)

		private void CreateBlueprints()
        {
	        /*PlayersLayout();
	        EliteCrafting();
	        AdvancedCrafting();
	        BasicCrafting();
	        ProcessingCrafting();*/
        }

        /*private void PlayersLayout()
        {
			playersCraftingRecipe.Add("WarriorSword-ChestOfAttackSpeed-AlexisPet-NeonaziHelmet-WarriorSword-", Resources.Load<Item>("Items/Weapons/GodSword"));
        }

        private void EliteCrafting()
        {
			eliteRecipe.Add("IronPlate-SteelPlate-IronPlate-IronPlate-SteelPlate-IronPlate-IronPlate-SteelPlate-IronPlate-SteelPlate-", Resources.Load<Item>("Items/CraftingMaterials/HardenedPlate"));
        }

        private void AdvancedCrafting()
        {
			advancedRecipe.Add("GoldBar-SteelBar-SteelBar-SteelBar-SteelBar-SteelBar-GoldBar-", Resources.Load<Item>("Items/Materials/GoldenSteel"));
		}

        private void BasicCrafting()
        {
			basicRecipe.Add("IronBar-IronBar-IronBar-IronBar-IronBar-", Resources.Load<Item>("Items/CraftingMaterials/IronPlate"));
			basicRecipe.Add("SteelBar-SteelBar-SteelBar-SteelBar-SteelBar-", Resources.Load<Item>("Items/CraftingMaterials/SteelPlate"));
		}*/

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