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

        public void CreateBlueprints()
        {
            craftingRecipe.Add("EMPTY-Iron-EMPTY-Iron-Iron-Iron-EMPTY-Iron-EMPTY-", Resources.Load<Item>("CraftingMaterials/IronPlate"));
        }

        public void CraftItem()
        {
            
        }
    }
}