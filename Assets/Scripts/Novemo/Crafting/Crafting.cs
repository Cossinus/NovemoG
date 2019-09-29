using UnityEngine;

namespace Novemo.Crafting
{
    public class Crafting : Inventory.Inventory
    {
        private RecipeManager _recipeManager;
        
        public GameObject craftPrefabButton; // also preview slot

        void Start()
        {
            _recipeManager = RecipeManager.Instance;
        }
    }
}