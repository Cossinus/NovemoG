using System;
using System.Collections;
using System.Collections.Generic;
using Novemo.Items;
using Novemo.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Novemo.Inventory
{
    public class Inventory : MonoBehaviour
    {
        #region Singleton

        public static Inventory Instance;

        private void Awake()
        {
            Instance = this;
        }

        #endregion

        // Inventory Parent Object
        public Transform itemsParent;

        // Canvas Group of an Inventory
        public CanvasGroup canvasGroup;

        // Game Objects
        private static GameObject _playerRef;
        public GameObject inventoryUI;

        // All Slots in Inventory
        public Slot.Slot[] Slots { get; private set; }

        // Floats
        private float _hoverYOffset;
        public float fadeTime;

        public bool IsOpen { get; private set; }
        private bool _fadingIn;
        private bool _fadingOut;

        // Instance of Manager
        private InventoryManager _inventoryManager;

        // EmptySlots and All Slots Count
        public int EmptySlots { get; set; }
        public int debugEmptySlots;
        public int slotsCount;

        public virtual void Start()
        {
            _playerRef = PlayerManager.Instance.player;

            if (Slots != null)
                foreach (var go in Slots)
                    Destroy(go);

            Slots = itemsParent.GetComponentsInChildren<Slot.Slot>();

            EmptySlots = slotsCount;

            _hoverYOffset = 100f * 0.01f;

            _inventoryManager = InventoryManager.Instance;
            
            canvasGroup.blocksRaycasts = false;
            _inventoryManager.blur.SetActive(false);
            IsOpen = false;
        }

        public virtual void Update()
        {
            if (Input.GetMouseButtonUp(0) && !_inventoryManager.eventSystem.IsPointerOverGameObject(-1))
            {
                DropItem();
            }

            if (_inventoryManager.HoverObject != null)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    _inventoryManager.canvas.transform as RectTransform,
                    Input.mousePosition,
                    _inventoryManager.canvas.worldCamera, out var position);
                position.Set(position.x, position.y - _hoverYOffset);
                _inventoryManager.HoverObject.transform.position =
                    _inventoryManager.canvas.transform.TransformPoint(position);
            }

            if (_inventoryManager.toolTipObject.activeSelf)
            {
                var xPos = Input.mousePosition.x;
                var yPos = Input.mousePosition.y;

                _inventoryManager.toolTipObject.transform.position = new Vector2(xPos, yPos);
            }
            
            if (!_inventoryManager.MovingSlot.IsEmpty)
                _inventoryManager.toolTipObject.SetActive(false);

            debugEmptySlots = EmptySlots;
        }

        public virtual void Open()
        {
            if (canvasGroup.alpha <= 0)
            {
                StartCoroutine(nameof(FadeIn));
                IsOpen = true;
            }
            else
            {
                StartCoroutine(nameof(FadeOut));
                InventoryManager.Instance.selectStackSize.SetActive(false);
                IsOpen = false;
                HideToolTip();
                DropItem();
            }
        }

        public void DropItem()
        {
            if (!_inventoryManager.MovingSlot.IsEmpty)
            {
                var amount = 0;

                for (var i = 0; i < _inventoryManager.MovingSlot.Items.Count; i++)
                {
                    amount++;
                }
                
                var angle = UnityEngine.Random.Range(0.0f, Mathf.PI * 2);
                
                var v = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle));
                v *= 1f;

                var tmpDrp = Instantiate(_inventoryManager.dropItem, _playerRef.transform.position - v,
                    Quaternion.identity);
                tmpDrp.tag = "Item";

                tmpDrp.GetComponentInChildren<SpriteRenderer>().sprite = _inventoryManager.MovingSlot.CurrentItem.itemIcon;
                tmpDrp.GetComponentInChildren<SpriteRenderer>().sortingLayerName = "Interactable";
                tmpDrp.GetComponent<ItemPickup>().item = _inventoryManager.MovingSlot.CurrentItem;
                tmpDrp.GetComponent<ItemPickup>().amount = amount;

                _inventoryManager.MovingSlot.ClearSlot();
                Destroy(GameObject.Find("Hover"));
            }
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
                foreach (var slot in Slots)
                {
	                var tmpSlot = slot.GetComponent<Slot.Slot>();

                    if (tmpSlot.IsEmpty) continue;
                    if (!tmpSlot.CurrentItem.Equals(item) || !tmpSlot.IsAvailable) continue;
                    if (_inventoryManager.Clicked != null &&
                        _inventoryManager.Clicked.GetComponent<Slot.Slot>() == tmpSlot.GetComponent<Slot.Slot>() &&
                        tmpSlot.Items.Count == item.stackLimit - _inventoryManager.MovingSlot.Items.Count)
                    {
	                    continue;
                    }
                    else
                    {
	                    tmpSlot.AddItem(item);
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
                foreach (var slot in Slots)
                {
                    var tmp = slot.GetComponent<Slot.Slot>();
                    if (tmp.IsEmpty)
                    {
                        tmp.AddItem(item);
                        EmptySlots--;
                        return true;
                    }
                }

            return false;
        }

        public virtual void MoveItem(GameObject clicked)
        {
            _inventoryManager.Clicked = clicked;
            _inventoryManager.selectStackSize.SetActive(false);

            if (!_inventoryManager.MovingSlot.IsEmpty)
            {
                var tmp = clicked.GetComponent<Slot.Slot>();

                if (tmp.IsEmpty)
                {
                    tmp.AddItems(_inventoryManager.MovingSlot.Items);
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
                        tmp.AddItem(item);
                    }

                    _inventoryManager.MovingSlot.ClearSlot();
                    foreach (var item in tmpItems)
                    {
                        _inventoryManager.MovingSlot.AddItem(item);
                    }

                    tmpItems.Clear();
                    CreateHoverIcon(_inventoryManager.MovingSlot.CurrentItem.itemIcon);
                    Destroy(GameObject.Find("Hover"));
                }
            }
            else if (_inventoryManager.From == null && canvasGroup.alpha >= 1)
            {
                if (!clicked.GetComponent<Slot.Slot>().IsEmpty)
                {
                    EmptySlots++;
                    _inventoryManager.MovingSlot.Items = clicked.GetComponent<Slot.Slot>()
                        .RemoveItems(clicked.GetComponent<Slot.Slot>().Items.Count);
                    CreateHoverIcon(clicked.transform.Find("ItemButton/Icon").GetComponent<Image>().sprite);
                    clicked.GetComponent<Slot.Slot>().ClearSlot();
                }
            }
            else if (_inventoryManager.To == null)
            {
                _inventoryManager.To = clicked.GetComponent<Slot.Slot>();
                _inventoryManager.Clicked = null;
                Destroy(GameObject.Find("Hover"));
            }
            else if (_inventoryManager.To != null && !_inventoryManager.MovingSlot.IsEmpty)
            {
                var tmpTo = new Stack<Item>(_inventoryManager.To.Items);
                if (tmpTo.Count == 0)
                {
                    foreach (var item in _inventoryManager.MovingSlot.Items)
                        clicked.GetComponent<Slot.Slot>().AddItem(item);

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

            if (Crafting.Crafting.Instance.IsOpen)
            {
                Crafting.Crafting.Instance.Preview();
            }
        }

        public void MergeStacks(Slot.Slot source, Slot.Slot destination)
        {
            var max = destination.CurrentItem.stackLimit - destination.Items.Count;
            var count = source.Items.Count < max ? source.Items.Count : max;

            for (var i = 0; i < count; i++)
            {
                destination.AddItem(source.RemoveItem());
                if (!_inventoryManager.MovingSlot.IsEmpty)
                {
                    _inventoryManager.HoverObject.transform.Find("ItemButton/Amount").GetComponent<Text>().text =
                        _inventoryManager.MovingSlot.Items.Count.ToString();
                    _inventoryManager.HoverObject.transform.Find("ItemButton/Icon").GetComponent<Image>().sprite =
                        _inventoryManager.MovingSlot.CurrentItem.itemIcon;
                }
                /*else
                {
                    _inventoryManager.HoverObject.transform.Find("ItemButton/Amount").GetComponent<Text>().text =
                        source.Items.Count.ToString();
                    _inventoryManager.HoverObject.transform.Find("ItemButton/Icon").GetComponent<Image>().sprite =
                        source.CurrentItem.itemIcon;
                }*/
            }

            if (source.Items.Count == 0)
            {
                source.ClearSlot();
                Destroy(GameObject.Find("Hover"));
            }
        }

        public void CreateHoverIcon(Sprite icon)
        {
            _inventoryManager.HoverObject = Instantiate(_inventoryManager.iconPrefab, GameObject.Find("Inventory Canvas").transform, true);
            _inventoryManager.HoverObject.GetComponent<Slot.Slot>().AddItem(_inventoryManager.MovingSlot.CurrentItem);

            _inventoryManager.HoverObject.transform.Find("ItemButton/Icon").gameObject.SetActive(true);
            _inventoryManager.HoverObject.name = "Hover";

            var hoverTransform = _inventoryManager.HoverObject.GetComponent<RectTransform>();
            var clickedTransform = _inventoryManager.Clicked.GetComponent<RectTransform>();

            hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, clickedTransform.sizeDelta.x - 50f);
            hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, clickedTransform.sizeDelta.y - 50f);

            _inventoryManager.HoverObject.transform.localScale = _inventoryManager.Clicked.gameObject.transform.localScale;

            _inventoryManager.HoverObject.transform.Find("ItemButton/Amount").gameObject.SetActive(true);
            if (!_inventoryManager.MovingSlot.IsEmpty)
            {
	            _inventoryManager.HoverObject.transform.Find("ItemButton/Amount").GetComponent<Text>().text =
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
                _inventoryManager.MovingSlot.Items = _inventoryManager.Clicked.GetComponent<Slot.Slot>().RemoveItems(_inventoryManager.SplitAmount);
                CreateHoverIcon(_inventoryManager.Clicked.transform.Find("ItemButton/Icon").GetComponent<Image>().sprite);
                _inventoryManager.Clicked.GetComponent<Slot.Slot>().ClearSlot();
                EmptySlots++;
            }
            else if (_inventoryManager.SplitAmount > 0)
            {
                _inventoryManager.MovingSlot.Items = _inventoryManager.Clicked.GetComponent<Slot.Slot>().RemoveItems(_inventoryManager.SplitAmount);
                CreateHoverIcon(_inventoryManager.Clicked.transform.Find("ItemButton/Icon").GetComponent<Image>().sprite);
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
            var tmpSlot = slot.GetComponent<Slot.Slot>();

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

        private IEnumerator FadeOut()
        {
            if (!_fadingOut)
            {
                _fadingOut = true;
                _fadingIn = false;
                StopCoroutine(nameof(FadeIn));

                var startAlpha = canvasGroup.alpha;
                var rate = 1.0f / fadeTime;
                var progress = 0.0f;
                
                while (progress < 1.0)
                {
                    canvasGroup.alpha = Mathf.Lerp(startAlpha, 0, progress);

                    progress += rate * Time.deltaTime;
                    yield return null;
                }
                _inventoryManager.blur.SetActive(false);
                canvasGroup.blocksRaycasts = false;

                canvasGroup.alpha = 0;
                _fadingOut = false;
            }
        }

        private IEnumerator FadeIn()
        {
            if (!_fadingIn)
            {
                _fadingOut = false;
                _fadingIn = true;
                StopCoroutine(nameof(FadeOut));

                var startAlpha = canvasGroup.alpha;
                var rate = 1.0f / fadeTime;
                var progress = 0.0f;
                
                canvasGroup.blocksRaycasts = true;
                _inventoryManager.blur.SetActive(true);
                while (progress < 1.0)
                {
                    canvasGroup.alpha = Mathf.Lerp(startAlpha, 1, progress);

                    progress += rate * Time.deltaTime;
                    yield return null;
                }

                canvasGroup.alpha = 1;
                _fadingIn = false;
            }
        }

        private void SaveInventory()
        {
            var content = string.Empty;

            for (var i = 0; i < Slots.Length; i++)
            {
                var tmp = Slots[i].GetComponent<Slot.Slot>();

                if (!tmp.IsEmpty) content += i + "-" + tmp.CurrentItem.itemType + "-" + tmp.Items.Count + ";";
            }

            PlayerPrefs.SetString(gameObject.name + "content", content);
            PlayerPrefs.SetInt(gameObject.name + "slots", slotsCount);
            var position = inventoryUI.transform.position;
            PlayerPrefs.SetFloat(gameObject.name + "xPos", position.x);
            PlayerPrefs.SetFloat(gameObject.name + "yPos", position.y);

            PlayerPrefs.Save();
        }

        private void LoadInventory()
        {
            var content = PlayerPrefs.GetString(gameObject.name + "content");
            slotsCount = PlayerPrefs.GetInt(gameObject.name + "slots");

            inventoryUI.transform.position = new Vector3(PlayerPrefs.GetFloat(gameObject.name + "xPos"),
                PlayerPrefs.GetFloat(gameObject.name + "yPos"), inventoryUI.transform.position.z);

            var splitContent = content.Split(';');
            for (var x = 0; x < splitContent.Length - 1; x++)
            {
                var splitValues = splitContent[x].Split('-');

                var index = int.Parse(splitValues[0]);
                var type = (ItemType) Enum.Parse(typeof(ItemType), splitValues[1]);
                var amount = int.Parse(splitValues[2]);

                for (var i = 0; i < amount; i++)
                {
                }
            }
        }
    }
}