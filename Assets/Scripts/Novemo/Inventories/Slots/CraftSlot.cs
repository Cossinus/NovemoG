using System;
using System.Collections.Generic;
using System.Linq;
using Novemo.Characters.Player;
using Novemo.Items;
using Novemo.UI;
using UnityEngine;

namespace Novemo.Inventories.Slots
{
    public class CraftSlot : Slot
    {
        public Item craftItem;

        private static GameObject _tooltipObject;

        private static Inventory _inventory;
        private static InventoryManager _inventoryManager;

        //TODO make a drag list for all recipes discovered in separate script
        
        private void Start()
        {
            _inventory = Inventory.Instance;
            _inventoryManager = InventoryManager.Instance;
            icon.sprite = craftItem.itemIcon;
            icon.gameObject.SetActive(true);
            _tooltipObject = _inventoryManager.toolTipObject;
        }

        public void CraftItem()
        {
            if (_inventoryManager.MovingSlot.Items.Count == craftItem.stackLimit)
            {
                EventLog.Instance.RaiseEventLog("Empty your hand and craft again.");
            }
            else if (PlayerManager.Instance.player.GetComponent<Player>().CraftSkill >= craftItem.recipe.craftSkillRequired)
            {
                var recipe = craftItem.recipe;
                var tmpIngredients = recipe.Ingredients.ToList();
                var playerSlotsContainingItem = new List<GameObject>();

                foreach (var slot in _inventory.AllSlots)
                {
                    if (slot.IsEmpty) { }
                    else
                    {
                        foreach (var item in slot.Items)
                        {
                            if (tmpIngredients.Contains(item))
                            {
                                tmpIngredients.Remove(item);
                                playerSlotsContainingItem.Add(slot.gameObject);
                            }
                        }
                    }
                }

                if (tmpIngredients.Count == 0)
                {
                    tmpIngredients = new List<Item>(recipe.Ingredients);
                    
                    foreach (var slot in playerSlotsContainingItem.ToList())
                    {
                        if (tmpIngredients.Contains(slot.GetComponent<Slot>().Items.Peek()))
                        {
                            slot.GetComponent<Slot>().RemoveSlotItem();
                        }

                        if (slot.GetComponent<Slot>().IsEmpty)
                        {
                            playerSlotsContainingItem.Remove(slot);
                        }
                    }

                    var wasAdded = _inventory.AddItem(craftItem);

                    if (_inventory.EmptySlots == 0 && !wasAdded &&
                        _inventoryManager.MovingSlot.Items.Count != craftItem.stackLimit)
                    {
                        _inventoryManager.Clicked = gameObject;
                        _inventoryManager.MovingSlot.AddItemToSlot(craftItem);
                        if (_inventoryManager.HoverObject == null) _inventory.CreateHoverIcon();
                        else
                        {
                            Destroy(GameObject.Find("Hover"));
                            _inventory.CreateHoverIcon();
                        }
                        HideTooltip();
                    }
                    else
                    {
                        if (!wasAdded) _inventory.AddItem(craftItem);
                        EventLog.Instance.RaiseEventLog("You don't have enough space in your inventory!");
                    }
                }
                else
                {
                    EventLog.Instance.RaiseEventLog("You don't have enough ingredients!");
                }
            }
            else
            {
                EventLog.Instance.RaiseEventLog("Comeback later with a higher craft skill.");
            }
        }

        public void ShowTooltip(GameObject targetObject)
        {
            if (_inventoryManager.HoverObject != null || _inventoryManager.selectStackSize.activeSelf) return;

            var itemTooltipText = craftItem.GetTooltip();
            var requiredSkill = $"Required Craft Skill: {craftItem.recipe.craftSkillRequired.ToString()}\n";
            var ingredientsText = string.Empty;
            Item previousIngredient = null;

            _tooltipObject.SetActive(true);
            
            var tmpCountList = new List<int>();

            foreach (var recipeIngredient in craftItem.recipe.Ingredients)
            {
                if (recipeIngredient.Equals(previousIngredient))
                {
                    tmpCountList[tmpCountList.Count - 1]++;
                }
                else
                {
                    tmpCountList.Add(1);
                }
                previousIngredient = recipeIngredient;
            }

            for (var i = 1; i < tmpCountList.Count + 1; i++)
            {
                var countIndex = tmpCountList[tmpCountList.Count - i];
                
                ingredientsText += countIndex.ToString();
                
                if (tmpCountList.Count == 1)
                {
                    countIndex -= 1;
                }
                
                ingredientsText += $" <sprite name=\"{craftItem.recipe.Ingredients[countIndex].itemName}\">{Environment.NewLine}";
            }

            _inventoryManager.sizeTextObject.text =
                $"{requiredSkill}{itemTooltipText}{Environment.NewLine}Ingredients:{Environment.NewLine}<size=65>{ingredientsText}</size>";
            _inventoryManager.visualTextObject.text = _inventoryManager.sizeTextObject.text;
        }

        public void HideTooltip()
        {
            _tooltipObject.SetActive(false);
        }
    }
}