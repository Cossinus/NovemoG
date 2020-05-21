using System;
using System.Collections.Generic;
using System.Linq;
using Novemo.Character.Player;
using Novemo.Characters.Player;
using Novemo.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Novemo.Inventories.Slot
{
    public class CraftSlot : Slot
    {
        public Item craftItem;

        public GameObject ingredientObject;

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
                Debug.Log("Empty your hand and craft again.");
            }
            else if (PlayerManager.Instance.player.GetComponent<Player>().CraftSkill >= craftItem.recipe.craftSkillRequired)
            {
                var recipe = craftItem.recipe;
                var tmpIngredients = recipe.Ingredients.ToList();
                var playerSlotsContainingItem = new List<GameObject>();

                foreach (var slot in _inventory.AllSlots)
                {
                    Debug.Log("xd");
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
                    }
                }
            }
            else
            {
                Debug.Log("Comeback later with a higher craft skill.");
            }
        }

        public void ShowTooltip(GameObject targetObject)
        {
            if (_inventoryManager.HoverObject != null || _inventoryManager.selectStackSize.activeSelf) return;

            var itemTooltipText = craftItem.GetTooltip();
            var requiredSkill = $"Required Craft Skill: {craftItem.recipe.craftSkillRequired.ToString()}\n";
            var ingredientsText = string.Empty;
            var count = 0;

            _tooltipObject.SetActive(true);
            _tooltipObject.GetComponent<CanvasGroup>().alpha = 0;

            var tmpIngredientTypeList = new List<Item>();

            foreach (var recipeIngredient in craftItem.recipe.Ingredients)
            {
                if (!tmpIngredientTypeList.Contains(recipeIngredient))
                {
                    tmpIngredientTypeList.Add(recipeIngredient);
                    var ingredient = Instantiate(ingredientObject, GameObject.Find("Tooltip").transform, true);
                    ingredient.transform.localScale = new Vector3(2f, 2f, 0f);
                    ingredient.transform.position = new Vector3(_tooltipObject.transform.position.x, 0);
                    ingredient.transform.localPosition = new Vector3(
                        ingredient.transform.localPosition.x + _tooltipObject.GetComponent<RectTransform>().localScale.x * 43, 
                        1000f);
                    ingredient.transform.Find("Image").GetComponent<Image>().sprite = recipeIngredient.itemIcon;
                    ingredient.name = $"Ingredient{tmpIngredientTypeList.Count}";
                    ingredientsText += "0\n";
                    count = 1;
                }
                else
                {
                    count++;
                    GameObject.Find($"Ingredient{tmpIngredientTypeList.Count}/Count").GetComponent<TextMeshProUGUI>()
                        .text = count.ToString();
                }
            }

            _inventoryManager.sizeTextObject.text =
                $"{requiredSkill}{itemTooltipText}{Environment.NewLine}Ingredients:{Environment.NewLine}<size=65>{ingredientsText}</size>";
            _inventoryManager.visualTextObject.text = _inventoryManager.sizeTextObject.text;

            Invoke(nameof(SetIngredientPositions), 0.075f);
        }

        private void SetIngredientPositions()
        {
            var yIngredientPos = -_tooltipObject.transform.GetComponent<RectTransform>().sizeDelta.y - _inventoryManager.sizeTextObject.rectTransform.rect.height + 10f;
            
            var ingredients = GameObject.FindGameObjectsWithTag("Ingredient");
            foreach(var ingredient in ingredients)
            {
                yIngredientPos += 70f;
                
                ingredient.transform.localPosition = new Vector3(ingredient.transform.localPosition.x, yIngredientPos);
            }
            
            _tooltipObject.GetComponent<CanvasGroup>().alpha = 1;
        }

        public void HideTooltip()
        {
            var ingredients = GameObject.FindGameObjectsWithTag("Ingredient");

            foreach(var ingredient in ingredients)
            {
                Destroy(ingredient);
            }

            _tooltipObject.SetActive(false);
        }
    }
}