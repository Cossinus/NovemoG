using System;
using System.Collections.Generic;
using Novemo.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Novemo.Inventory.Slot
{
    public class CraftSlot : Slot
    {
        public Item craftItem;

        public GameObject ingredientObject;

        private static GameObject tooltipObject;

        private void Start()
        {
            icon.sprite = craftItem.itemIcon;
            icon.gameObject.SetActive(true);
            tooltipObject = InventoryManager.Instance.toolTipObject;
        }

        public void CraftItem()
        {
            if (InventoryManager.Instance.MovingSlot.IsEmpty)
            {
                var recipe = craftItem.recipe;
                var tmpIngredients = new List<Item>(recipe.Ingredients);
                var playerSlotsContainingItem = new List<GameObject>();

                foreach (var slot in Inventory.Instance.Slots)
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
                    var tmpSlotsList = new List<GameObject>(playerSlotsContainingItem);

                    foreach (var slot in tmpSlotsList)
                    {
                        if (tmpIngredients.Contains(slot.GetComponent<Slot>().Items.Peek()))
                        {
                            slot.GetComponent<Slot>().RemoveItem();
                        }

                        if (slot.GetComponent<Slot>().IsEmpty)
                        {
                            playerSlotsContainingItem.Remove(slot);
                        }
                    }

                    if (Inventory.Instance.EmptySlots == 0)
                    {
                        InventoryManager.Instance.Clicked = gameObject;
                        InventoryManager.Instance.MovingSlot.AddItem(craftItem);
                        Inventory.Instance.CreateHoverIcon(craftItem.itemIcon);
                    }
                    else
                    {
                        Inventory.Instance.AddItem(craftItem);
                    }
                }
            }
            else
            {
                Inventory.Instance.DropItem();
                //CraftItem();
            }
        }
        
        public void ShowTooltip(GameObject targetObject)
        {
            if (InventoryManager.Instance.HoverObject == null && !InventoryManager.Instance.selectStackSize.activeSelf)
            {
                var itemTooltipText = craftItem.GetTooltip();
                var requiredSkill = $"Required Craft Skill: {craftItem.recipe.craftSkillRequired.ToString()}\n";
                var ingredientsText = string.Empty;
                var count = 0;

                tooltipObject.SetActive(true);
                
                var tmpIngredientTypeList = new List<Item>();
                
                var yIngredientPos = -tooltipObject.transform/*.Find("SizeText")*/.GetComponent<RectTransform>().sizeDelta.y - 240f;

                foreach (var recipeIngredient in craftItem.recipe.Ingredients)
                {
                    if (!tmpIngredientTypeList.Contains(recipeIngredient))
                    {
                        tmpIngredientTypeList.Add(recipeIngredient);
                        var ingredient = Instantiate(ingredientObject, GameObject.Find("Tooltip").transform, true);
                        yIngredientPos += 70f;
                        ingredient.transform.position = new Vector3(tooltipObject.transform.position.x + 45f, 0);
                        ingredient.transform.localPosition = new Vector3(ingredient.transform.localPosition.x, yIngredientPos);
                        ingredient.transform.localScale = new Vector3(2f, 2f, 0f);
                        ingredient.transform.Find("Image").GetComponent<Image>().sprite = recipeIngredient.itemIcon;
                        ingredient.name = $"Ingredient{tmpIngredientTypeList.Count}";
                        ingredientsText += $"{(int)recipeIngredient.itemRarity}\n";
                        count = 1;
                    }
                    else
                    {
                        count++;
                        GameObject.Find($"Ingredient{tmpIngredientTypeList.Count}/Count").GetComponent<TextMeshProUGUI>().text = count.ToString();
                    }
                }
                
                InventoryManager.Instance.sizeTextObject.text = $"{requiredSkill}{itemTooltipText}{Environment.NewLine}Ingredients:{Environment.NewLine}<size=65>{ingredientsText}</size>";
                InventoryManager.Instance.visualTextObject.text = InventoryManager.Instance.sizeTextObject.text;
            }
        }

        public void HideTooltip()
        {
            var ingredients = GameObject.FindGameObjectsWithTag("Ingredient");

            foreach(var ingredient in ingredients)
            {
                Destroy(ingredient);
            }

            tooltipObject.SetActive(false);
        }
    }
}