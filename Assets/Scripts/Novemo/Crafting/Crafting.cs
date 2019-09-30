using Novemo.Inventory.Slot;
using UnityEngine;
using UnityEngine.UI;

namespace Novemo.Crafting
{
    public class Crafting : Inventory.Inventory
    {
        private RecipeManager _recipeManager;
        
        public GameObject craftPrefabButton; // also preview slot

        void Start()
        {
            _recipeManager = RecipeManager.Instance;
            craftPrefabButton.GetComponent<Button>().onClick.AddListener(CraftItem);
        }
        
        public void CraftItem()
        {
            var output = string.Empty;
            
            foreach (var slot in slots)
            {
                var tmp = slot.GetComponent<Slot>();
                
                if (tmp.IsEmpty)
                {
                    output += "EMPTY-";
                }
                else
                {
                    output += tmp.CurrentItem.craftName + "-";
                }
            }
        }
    }
}