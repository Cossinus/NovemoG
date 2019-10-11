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

			if (Instance != null)
            {
                Debug.LogWarning("More than one instance of Inventory found!");
                return;
            }

            Instance = this;
        }

		#endregion

		public Dictionary<string, Item> playersCraftingRecipe = new Dictionary<string, Item>();   // 5 Ingredients recipe  (in player's layout)
		public Dictionary<string, Item> eliteRecipe = new Dictionary<string, Item>();             // 10 Ingredients recipe
		public Dictionary<string, Item> advancedRecipe = new Dictionary<string, Item>();          // 7 Ingredients recipe
		public Dictionary<string, Item> basicRecipe = new Dictionary<string, Item>();             // 5 Ingredients recipe  (in special layout)
		public Dictionary<Item, Item> processingRecipe = new Dictionary<Item, Item>();            // 1 Ingredient recipe

		private void CreateBlueprints()
        {
            PlayersLayout();
            EliteCrafting();
            AdvancedCrafting();
            BasicCrafting();
            ProcessingCrafting();
        }

        private void PlayersLayout()
        {
            playersCraftingRecipe.Add("EMPTY-IRON-EMPTY-IRON-IRON-IRON-EMPTY-IRON-EMPTY-", Resources.Load<Item>("Items/CraftingMaterials/IronPlate"));
        }

        private void EliteCrafting()
        {
            
        }

        private void AdvancedCrafting()
        {
            
        }

        private void BasicCrafting()
        {
            
        }

        private void ProcessingCrafting()
        {
            processingRecipe.Add(Resources.Load<Item>("Items/Materials/IronBar"), Resources.Load<Item>("Items/CraftingMaterials/IronPlate"));
        }
    }
}