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
            if (Instance != null)
            {
                Debug.LogWarning("More than one instance of Inventory found!");
                return;
            }

            Instance = this;
        }

        #endregion
        
        public Dictionary<string, Item> craftingRecipe = new Dictionary<string, Item>();
        public Dictionary<Item, Item> processingRecipe = new Dictionary<Item, Item>();

        public void CreateBlueprints()
        {
            PlayersLayout();
            EliteCrafting();
            AdvancedCrafting();
            BasicCrafting();
            ProcessingCrafting();
        }

        private void PlayersLayout()
        {
            craftingRecipe.Add("EMPTY-Iron-EMPTY-Iron-Iron-Iron-EMPTY-Iron-EMPTY-", Resources.Load<Item>("Items/CraftingMaterials/IronPlate"));
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