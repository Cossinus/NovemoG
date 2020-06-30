using System;
using System.Collections.Generic;
using System.Linq;
using Novemo.Interactables;
using Novemo.Items;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace Novemo.Inventories
{
    public sealed class Inventory : MonoBehaviour
    {
        #region Singleton

        public static Inventory Instance;

        private void Awake()
        {
            Instance = this;
        }

        #endregion
        
        public Transform itemsParent;
        
        public CanvasGroup canvasGroup;

        public Slots.Slot[] AllSlots { get; private set; }
        
        public bool IsOpen { get; private set; }

        private InventoryManager _inventoryManager;

        private static Tilemap _tilemap;

        // EmptySlots and All Slots Count
        public int EmptySlots { get; set; }
        public int debugEmptySlots;
        public int slotsCount;

        public void Start()
        {
            _tilemap = GameObject.Find("Collideable").GetComponent<Tilemap>();

            if (AllSlots != null)
                foreach (var slot in AllSlots)
                    Destroy(slot);

            AllSlots = itemsParent.GetComponentsInChildren<Slots.Slot>();
            slotsCount = AllSlots.Length;
            EmptySlots = slotsCount;

            _inventoryManager = InventoryManager.Instance;

            IsOpen = false;
        }

        public void Update()
        {
            debugEmptySlots = EmptySlots;
        }

        public void Open()
        {
            if (Metrics.EqualFloats(canvasGroup.alpha, 0, 0.01f))
            {
                StartCoroutine(_inventoryManager.FadeIn(canvasGroup));
                IsOpen = true;
            }
            else
            {
                HideToolTip();
                StartCoroutine(_inventoryManager.FadeOut(canvasGroup));
                StatsPanel.Instance.HideAdvancedStats();
                InventoryManager.Instance.selectStackSize.SetActive(false);
                IsOpen = false;
            }
        }

        public Item GetItemWithName(string passedName)
        {
            return (from s in AllSlots
                where s.Items.Count > 0
                where s.CurrentItem.itemName.Contains(passedName)
                select s.CurrentItem).FirstOrDefault();
        }
        
        public int GetItemCount(Item item)
        {
            return AllSlots.Where(slot => slot.CurrentItem.Equals(item)).Sum(slot => slot.Items.Count);
        }

        public bool ContainItem(Item item)
        {
            return AllSlots.FirstOrDefault(slot => slot.CurrentItem.Equals(item));
        }

        public void DropItems(List<Item> itemsToDrop, Transform target)
        {
            var tmpDrp = Instantiate(_inventoryManager.dropItem, Metrics.GetRandomPosition(target), Quaternion.identity);
            
            while (_tilemap.HasTile(_tilemap.WorldToCell(tmpDrp.transform.position)))
                tmpDrp.transform.position = Metrics.GetRandomPosition(target);

            tmpDrp.tag = "Item";

            var spriteRenderer = tmpDrp.GetComponentInChildren<SpriteRenderer>();
            var itemPickup = tmpDrp.GetComponent<ItemPickup>();
            
            spriteRenderer.sprite = itemsToDrop[0].itemIcon;
            spriteRenderer.sortingLayerName = "Interactable";
            itemPickup.dropTime = DateTime.UtcNow;
            itemPickup.item = itemsToDrop[0];
            itemPickup.amount = itemsToDrop.Count;
        }
        
        public void DropItem(Item itemToDrop, Transform target)
        {
            var tmpDrp = Instantiate(_inventoryManager.dropItem, Metrics.GetRandomPosition(target), Quaternion.identity);

            while (_tilemap.HasTile(_tilemap.WorldToCell(tmpDrp.transform.position)))
                tmpDrp.transform.position = Metrics.GetRandomPosition(target);

            tmpDrp.tag = "Item";

            var spriteRenderer = tmpDrp.GetComponentInChildren<SpriteRenderer>();
            var itemPickup = tmpDrp.GetComponent<ItemPickup>();
            
            spriteRenderer.sprite = itemToDrop.itemIcon;
            spriteRenderer.sortingLayerName = "Interactable";
            itemPickup.dropTime = DateTime.UtcNow;
            itemPickup.item = itemToDrop;
        }

        public bool AddItem(Item item)
        {
            if (item.stackLimit == 1 && EmptySlots > 0)
            {
	            if (!item.isDiscovered)
	            {
		            item.isDiscovered = true;
	            }
	            return PlaceEmpty(item);
			}

            if (item.stackLimit > 1)
            {
                foreach (var slot in AllSlots)
                {
	                var tmpSlot = slot.GetComponent<Slots.Slot>();

                    if (tmpSlot.IsEmpty) continue;
                    if (!tmpSlot.CurrentItem.Equals(item) || !tmpSlot.IsAvailable) continue;
                    if (_inventoryManager.Clicked != null &&
                        _inventoryManager.Clicked.GetComponent<Slots.Slot>() == tmpSlot.GetComponent<Slots.Slot>() &&
                        tmpSlot.Items.Count == item.stackLimit - _inventoryManager.MovingSlot.Items.Count)
                    {
	                    continue;
                    }
                    else
                    {
	                    tmpSlot.AddItemToSlot(item);
                        return true;
                    }
                }

                if (EmptySlots > 0)
                {

					return PlaceEmpty(item);
				}
            }

            return false;
        }

        private bool PlaceEmpty(Item item)
        {
            if (EmptySlots > 0)
            {
                foreach (var slot in AllSlots)
                {
                    var tmp = slot.GetComponent<Slots.Slot>();
                    if (tmp.IsEmpty)
                    {
                        tmp.AddItemToSlot(item);
                        EmptySlots--;
                        return true;
                    }
                }
            }
            
            return false;
        }

        public void MoveItem(GameObject clicked)
        {
            _inventoryManager.Clicked = clicked;
            _inventoryManager.selectStackSize.SetActive(false);

            if (!_inventoryManager.MovingSlot.IsEmpty)
            {
                var tmp = clicked.GetComponent<Slots.Slot>();

                if (tmp.IsEmpty)
                {
                    tmp.AddItemsToSlot(_inventoryManager.MovingSlot.Items);
                    _inventoryManager.MovingSlot.Items.Clear();
                    Destroy(GameObject.Find("Hover"));
                    EmptySlots--;
                }
                else if (!tmp.IsEmpty && _inventoryManager.MovingSlot.CurrentItem == tmp.CurrentItem &&
                         tmp.IsAvailable)
                {
                    MergeStacks(_inventoryManager.MovingSlot, tmp);
                }
                else if (!tmp.IsEmpty)
                {
                    var tmpItems = new Stack<Item>();
                    foreach (var item in tmp.Items)
                    {
                        tmpItems.Push(item);
                    }

                    Destroy(GameObject.Find("Hover"));
                    tmp.ClearSlot();
                    foreach (var item in _inventoryManager.MovingSlot.Items)
                    {
                        tmp.AddItemToSlot(item);
                    }

                    _inventoryManager.MovingSlot.ClearSlot();
                    foreach (var item in tmpItems)
                    {
                        _inventoryManager.MovingSlot.AddItemToSlot(item);
                    }

                    tmpItems.Clear();
                    CreateHoverIcon();
                    Destroy(GameObject.Find("Hover"));
                }
            }
            else if (_inventoryManager.From == null && canvasGroup.alpha >= 1)
            {
                if (!clicked.GetComponent<Slots.Slot>().IsEmpty)
                {
                    EmptySlots++;
                    _inventoryManager.MovingSlot.Items = clicked.GetComponent<Slots.Slot>()
                        .RemoveSlotItems(clicked.GetComponent<Slots.Slot>().Items.Count);
                    CreateHoverIcon();
                    clicked.GetComponent<Slots.Slot>().ClearSlot();
                }
            }
            else if (_inventoryManager.To == null)
            {
                _inventoryManager.To = clicked.GetComponent<Slots.Slot>();
                _inventoryManager.Clicked = null;
                Destroy(GameObject.Find("Hover"));
            }
            else if (_inventoryManager.To != null && !_inventoryManager.MovingSlot.IsEmpty)
            {
                var tmpTo = new Stack<Item>(_inventoryManager.To.Items);
                if (tmpTo.Count == 0)
                {
                    foreach (var item in _inventoryManager.MovingSlot.Items)
                        clicked.GetComponent<Slots.Slot>().AddItemToSlot(item);

                    _inventoryManager.MovingSlot.ClearSlot();
                    EmptySlots--;
                }
                else
                {
                    MergeStacks(_inventoryManager.From, _inventoryManager.To);
                }

                _inventoryManager.To = null;
                _inventoryManager.From = null;
                Destroy(GameObject.Find("Hover"));
            }

            debugEmptySlots = EmptySlots;
        }

        public void MergeStacks(Slots.Slot source, Slots.Slot destination)
        {
            var max = destination.CurrentItem.stackLimit - destination.Items.Count;
            var count = source.Items.Count < max ? source.Items.Count : max;

            for (var i = 0; i < count; i++)
            {
                destination.AddItemToSlot(source.RemoveSlotItem());
                if (_inventoryManager.MovingSlot.IsEmpty) continue;
                _inventoryManager.HoverObject.transform.Find("ItemButton/Amount").GetComponent<TextMeshProUGUI>().text =
                    _inventoryManager.MovingSlot.Items.Count.ToString();
                _inventoryManager.HoverObject.transform.Find("ItemButton/Icon").GetComponent<Image>().sprite =
                    _inventoryManager.MovingSlot.CurrentItem.itemIcon;
            }

            if (source.Items.Count != 0) return;
            source.ClearSlot();
            Destroy(GameObject.Find("Hover"));
        }

        public void CreateHoverIcon()
        {
            _inventoryManager.HoverObject = Instantiate(_inventoryManager.iconPrefab, GameObject.Find("Inventory Canvas").transform, true);
            
            _inventoryManager.HoverObject.GetComponent<Slots.Slot>().AddItemToSlot(_inventoryManager.MovingSlot.CurrentItem);

            _inventoryManager.HoverObject.transform.Find("ItemButton/Icon").gameObject.SetActive(true);
            _inventoryManager.HoverObject.name = "Hover";

            var hoverTransform = _inventoryManager.HoverObject.GetComponent<RectTransform>();
            var clickedTransform = _inventoryManager.Clicked.GetComponent<RectTransform>();

            hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, clickedTransform.sizeDelta.x - 50f);
            hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, clickedTransform.sizeDelta.y - 50f);

            _inventoryManager.HoverObject.transform.localScale = gameObject.transform.localScale;

            _inventoryManager.HoverObject.transform.Find("ItemButton/Amount").gameObject.SetActive(true);
            if (!_inventoryManager.MovingSlot.IsEmpty)
            {
	            _inventoryManager.HoverObject.transform.Find("ItemButton/Amount").GetComponent<TextMeshProUGUI>().text =
		            _inventoryManager.MovingSlot.Items.Count > 1
			            ? _inventoryManager.MovingSlot.Items.Count.ToString()
			            : string.Empty;
            }
        }

        public void SplitStack()
        {
            _inventoryManager.selectStackSize.SetActive(false);

            if (_inventoryManager.SplitAmount == _inventoryManager.MaxStackCount)
            {
                _inventoryManager.MovingSlot.Items = _inventoryManager.Clicked.GetComponent<Slots.Slot>().RemoveSlotItems(_inventoryManager.SplitAmount);
                CreateHoverIcon();
                _inventoryManager.Clicked.GetComponent<Slots.Slot>().ClearSlot();
                EmptySlots++;
            }
            else if (_inventoryManager.SplitAmount > 0)
            {
                _inventoryManager.MovingSlot.Items = _inventoryManager.Clicked.GetComponent<Slots.Slot>().RemoveSlotItems(_inventoryManager.SplitAmount);
                CreateHoverIcon();
            }

            _inventoryManager.splitSlider.value = 0;
        }

        public void ChangeStackText()
        {
            _inventoryManager.SplitAmount = (int) _inventoryManager.splitSlider.value;

            if (_inventoryManager.SplitAmount < 0)
            {
	            _inventoryManager.SplitAmount = 0;
            }
            if (_inventoryManager.SplitAmount > _inventoryManager.MaxStackCount)
            {
	            _inventoryManager.SplitAmount = _inventoryManager.MaxStackCount;
            }

            _inventoryManager.stackTxt.text = _inventoryManager.SplitAmount.ToString();
        }

        public void ShowToolTip(GameObject slot)
        {
            var tmpSlot = slot.GetComponent<Slots.Slot>();

            if (!tmpSlot.IsEmpty && _inventoryManager.HoverObject == null &&
                !_inventoryManager.selectStackSize.activeSelf)
            {
                _inventoryManager.visualTextObject.text = tmpSlot.CurrentItem.GetTooltip();
                _inventoryManager.sizeTextObject.text = _inventoryManager.visualTextObject.text;

                _inventoryManager.toolTipObject.SetActive(true);
            }
        }

        public void HideToolTip()
        {
            _inventoryManager.toolTipObject.SetActive(false);
        }
    }
}