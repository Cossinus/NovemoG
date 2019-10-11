using Novemo.Controllers;
using Novemo.Inventory.Slot;
using Novemo.Player;
using Novemo.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Novemo.Crafting
{
    public class Crafting : Inventory.Inventory
    {
        #region Singleton

        public new static Crafting Instance;

        private void Awake()
        {
            Instance = this;
        }

        #endregion

        private RecipeManager _recipeManager;

        private PlayerStats _playerStats;

        public TextMeshProUGUI craftSkillText;
        
        public GameObject craftPrefabButton; // also a preview slot

        public override void Start()
        {
            base.Start();
            _recipeManager = RecipeManager.Instance;
            _playerStats = PlayerManager.Instance.player.GetComponent<PlayerStats>();
            craftPrefabButton.GetComponentInChildren<Button>().onClick.AddListener(CraftItem);
		}

        public override void Update()
        {
            base.Update();
            craftSkillText.text = _playerStats.CraftSkill.ToString();
        }

        public override void MoveItem(GameObject clicked)
        {
	        base.MoveItem(clicked);

	        Preview();
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

            if (_recipeManager.playersCraftingRecipe.ContainsKey(output))
            {
                _recipeManager.playersCraftingRecipe.TryGetValue(output, out var craftedItem);

                if (craftedItem != null)
                {
                    if (PlayerManager.Instance.player.GetComponent<PlayerController>().inventory.AddItem(craftedItem))
                    {
                        foreach (var slot in slots)
                        {
                            slot.RemoveItem();
                        }
                    }
                }
            }

            Preview();
        }
        
        public void Preview()
        {
            var output = string.Empty;
            
            craftPrefabButton.GetComponent<Slot>().ClearSlot();
            
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

            if (_recipeManager.playersCraftingRecipe.ContainsKey(output))
            {
	            _recipeManager.playersCraftingRecipe.TryGetValue(output, out var craftedItem);

                if (craftedItem != null)
                {
	                craftPrefabButton.GetComponent<Slot>().ClearSlot();
					craftPrefabButton.transform.Find("ItemButton/Icon").gameObject.SetActive(true);
					craftPrefabButton.transform.Find("ItemButton/Icon").GetComponent<Image>().sprite = craftedItem.itemIcon;
					craftPrefabButton.GetComponent<Slot>().AddItem(craftedItem);
				}
            }
        }
    }
}