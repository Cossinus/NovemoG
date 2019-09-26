using System;
using System.Collections;
using System.Collections.Generic;
using Novemo.Items;
using Novemo.Player;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace Novemo.Inventory
{
    public class Inventory : MonoBehaviour
    {
        #region Singleton

        public static Inventory Instance;

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

        public delegate void OnItemChanged();
        public OnItemChanged onItemChangedCallback;

        // Inventory Parent Object
        public Transform itemsParent;

        // Canvas Group of an Inventory
        public CanvasGroup canvasGroup;

        // Game Objects
        private static GameObject _playerRef;
        public GameObject inventoryUI;
        public GameObject statsUI;

        // All Slots in Inventory
        private Slot.Slot[] _slots;

        // Floats
        private float _hoverYOffset;
        public float fadeTime;

        private bool _fadingIn;
        private bool _fadingOut;

        public Item iron;

        // Instance of Manager
        private InventoryManager inventoryManager;

        // EmptySlots and All Slots as an int
        public static int EmptySlots { get; set; }
        public int emptySlots;
        public int slots;

        void Start()
        {
            _playerRef = PlayerManager.Instance.player;

            if (_slots != null)
                foreach (var go in _slots)
                    Destroy(go);

            _slots = itemsParent.GetComponentsInChildren<Slot.Slot>();

            EmptySlots = slots;

            _hoverYOffset = 100f * 0.01f;

            inventoryManager = InventoryManager.Instance;

            inventoryUI.SetActive(false);
            statsUI.SetActive(false);
        }

        void Update()
        {
            if (Input.GetKey(KeyCode.C))
            {
                AddItem(iron);
            }

            if (Input.GetButtonDown("Inventory"))
            {
                if (canvasGroup.alpha <= 0)
                {
                    StartCoroutine(nameof(FadeIn));
                }
                else
                {
                    StartCoroutine(nameof(FadeOut));
                    DropItem();
                }
            }

            if (Input.GetMouseButtonUp(0) && !inventoryManager.eventSystem.IsPointerOverGameObject(-1))
            {
                DropItem();
            }

            if (inventoryManager.HoverObject != null)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    inventoryManager.canvas.transform as RectTransform,
                    Input.mousePosition,
                    inventoryManager.canvas.worldCamera, out var position);
                position.Set(position.x, position.y - _hoverYOffset);
                inventoryManager.HoverObject.transform.position =
                    inventoryManager.canvas.transform.TransformPoint(position);
            }

            if (inventoryManager.toolTipObject.activeSelf)
            {
                float xPos = Input.mousePosition.x;
                float yPos = Input.mousePosition.y;

                inventoryManager.toolTipObject.transform.position = new Vector2(xPos, yPos);
            }
            
            if (!inventoryManager.MovingSlot.IsEmpty)
                inventoryManager.toolTipObject.SetActive(false);

            emptySlots = EmptySlots;
        }

        private void DropItem()
        {
            if (!inventoryManager.MovingSlot.IsEmpty)
            {
                var amount = 0;

                for (var i = 0; i < inventoryManager.MovingSlot.Items.Count; i++)
                {
                    amount++;
                }
                
                var angle = UnityEngine.Random.Range(0.0f, Mathf.PI * 2);
                
                Vector3 v = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle));
                v *= 1f;

                GameObject tmpDrp = Instantiate(inventoryManager.dropItem, _playerRef.transform.position - v,
                    Quaternion.identity);
                tmpDrp.tag = "Item";

                tmpDrp.GetComponentInChildren<SpriteRenderer>().sprite = inventoryManager.MovingSlot.CurrentItem.icon;
                tmpDrp.GetComponentInChildren<SpriteRenderer>().sortingLayerName = "Interactable";
                tmpDrp.GetComponent<ItemPickup>().item = inventoryManager.MovingSlot.CurrentItem;
                tmpDrp.GetComponent<ItemPickup>().amount = amount;

                inventoryManager.MovingSlot.ClearSlot();
                Destroy(GameObject.Find("Hover"));
            }
        }

        public bool AddItem(Item item)
        {
            if (item.stackLimit == 1 && EmptySlots > 0) return PlaceEmpty(item);

            if (item.stackLimit > 1)
            {
                foreach (var slot in _slots)
                {
                    var tmp = slot.GetComponent<Slot.Slot>();

                    if (tmp.IsEmpty) continue;
                    if (tmp.CurrentItem.type != item.type || !tmp.IsAvailable) continue;
                    if (inventoryManager.clicked != null &&
                        inventoryManager.clicked.GetComponent<Slot.Slot>() == tmp.GetComponent<Slot.Slot>() &&
                        tmp.Items.Count == item.stackLimit - inventoryManager.MovingSlot.Items.Count)
                    {
                        continue;
                    }
                    else if (inventoryManager.Clicked != null &&
                             inventoryManager.Clicked.GetComponent<Slot.Slot>() == tmp.GetComponent<Slot.Slot>() &&
                             tmp.Items.Count == item.stackLimit - inventoryManager.MovingSlot.Items.Count)
                    {
                        continue;
                    }
                    else
                    {
                        tmp.AddItem(item);
                        return true;
                    }
                }

                if (EmptySlots > 0) return PlaceEmpty(item);
            }

            onItemChangedCallback?.Invoke();

            return false;
        }

        private bool PlaceEmpty(Item item)
        {
            if (EmptySlots > 0)
                foreach (var slot in _slots)
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

        public void MoveItem(GameObject clicked)
        {
            inventoryManager.Clicked = clicked;
            inventoryManager.selectStackSize.SetActive(false);

            if (!inventoryManager.MovingSlot.IsEmpty)
            {
                var tmp = clicked.GetComponent<Slot.Slot>();

                if (tmp.IsEmpty)
                {
                    tmp.AddItems(inventoryManager.MovingSlot.Items);
                    inventoryManager.MovingSlot.Items.Clear();
                    Destroy(GameObject.Find("Hover"));
                    EmptySlots--;
                }
                else if (!tmp.IsEmpty && inventoryManager.MovingSlot.CurrentItem.type == tmp.CurrentItem.type &&
                         tmp.IsAvailable)
                {
                    MergeStacks(inventoryManager.MovingSlot, tmp);
                }
            }
            else if (inventoryManager.From == null && canvasGroup.alpha >= 1)
            {
                if (!clicked.GetComponent<Slot.Slot>().IsEmpty)
                {
                    EmptySlots++;
                    inventoryManager.MovingSlot.Items = clicked.GetComponent<Slot.Slot>()
                        .RemoveItems(clicked.GetComponent<Slot.Slot>().Items.Count);
                    CreateHoverIcon();
                    clicked.GetComponent<Slot.Slot>().ClearSlot();
                }
            }
            else if (inventoryManager.To == null)
            { 
                inventoryManager.To = clicked.GetComponent<Slot.Slot>();
                inventoryManager.Clicked = null;
                Destroy(GameObject.Find("Hover"));
            }

            if (inventoryManager.To != null && !inventoryManager.MovingSlot.IsEmpty)
            {
                var tmpTo = new Stack<Item>(inventoryManager.To.Items);
                if (tmpTo.Count == 0)
                {
                    foreach (var item in inventoryManager.MovingSlot.Items)
                        clicked.GetComponent<Slot.Slot>().AddItem(item);

                    inventoryManager.MovingSlot.ClearSlot();
                    EmptySlots--;
                }
                else
                {
                    MergeStacks(inventoryManager.From, inventoryManager.To);
                }

                inventoryManager.To = null;
                inventoryManager.From = null;
                Destroy(GameObject.Find("Hover"));
            }

            emptySlots = EmptySlots;
        }

        public void MergeStacks(Slot.Slot source, Slot.Slot destination)
        {
            var max = destination.CurrentItem.stackLimit - destination.Items.Count;
            var count = source.Items.Count < max ? source.Items.Count : max;

            for (var i = 0; i < count; i++)
            {
                destination.AddItem(source.RemoveItem());
                if (!inventoryManager.MovingSlot.IsEmpty)
                    inventoryManager.HoverObject.transform.Find("ItemButton/Amount").GetComponent<Text>().text =
                        inventoryManager.MovingSlot.Items.Count.ToString();
                else
                    inventoryManager.HoverObject.transform.Find("ItemButton/Amount").GetComponent<Text>().text =
                        source.Items.Count.ToString();
            }

            if (source.Items.Count == 0)
            {
                source.ClearSlot();
                Destroy(GameObject.Find("Hover"));
            }
        }

        public void CreateHoverIcon()
        {
            inventoryManager.HoverObject = Instantiate(inventoryManager.iconPrefab,
                GameObject.Find("Inventory Canvas").transform, true);
            inventoryManager.HoverObject.transform.Find("ItemButton/Icon").GetComponent<Image>().sprite =
                inventoryManager.clicked == null
                    ? inventoryManager.Clicked.transform.Find("ItemButton/Icon").GetComponent<Image>().sprite
                    : inventoryManager.clicked.transform.Find("ItemButton/Icon").GetComponent<Image>().sprite;

            inventoryManager.HoverObject.transform.Find("ItemButton/Icon").gameObject.SetActive(true);
            inventoryManager.HoverObject.name = "Hover";

            var hoverTransform = inventoryManager.HoverObject.GetComponent<RectTransform>();
            var clickedTransform = inventoryManager.clicked == null
                ? inventoryManager.Clicked.GetComponent<RectTransform>()
                : inventoryManager.clicked.GetComponent<RectTransform>();

            hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, clickedTransform.sizeDelta.x - 50f);
            hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, clickedTransform.sizeDelta.y - 50f);

            inventoryManager.HoverObject.transform.localScale = inventoryManager.clicked == null
                ? inventoryManager.Clicked.gameObject.transform.localScale
                : inventoryManager.clicked.gameObject.transform.localScale;

            inventoryManager.HoverObject.transform.Find("ItemButton/Amount").gameObject.SetActive(true);
            if (!inventoryManager.MovingSlot.IsEmpty)
                inventoryManager.HoverObject.transform.Find("ItemButton/Amount").GetComponent<Text>().text =
                    inventoryManager.MovingSlot.Items.Count > 1
                        ? inventoryManager.MovingSlot.Items.Count.ToString()
                        : string.Empty;
            else
                inventoryManager.HoverObject.transform.Find("ItemButton/Amount").GetComponent<Text>().text =
                    inventoryManager.From.Items.Count > 1 ? inventoryManager.From.Items.Count.ToString() : string.Empty;
        }

        public void SplitStack()
        {
            inventoryManager.selectStackSize.SetActive(false);

            if (inventoryManager.SplitAmount == inventoryManager.MaxStackCount)
            {
                inventoryManager.MovingSlot.Items = inventoryManager.clicked.GetComponent<Slot.Slot>()
                    .RemoveItems(inventoryManager.SplitAmount);
                CreateHoverIcon();
                inventoryManager.clicked.GetComponent<Slot.Slot>().ClearSlot();
                EmptySlots++;
            }
            else if (inventoryManager.SplitAmount > 0)
            {
                inventoryManager.MovingSlot.Items = inventoryManager.clicked.GetComponent<Slot.Slot>()
                    .RemoveItems(inventoryManager.SplitAmount);
                CreateHoverIcon();
            }

            inventoryManager.splitSlider.value = 0;
        }

        public void ChangeStackText()
        {
            inventoryManager.SplitAmount = (int) inventoryManager.splitSlider.value;

            if (inventoryManager.SplitAmount < 0) inventoryManager.SplitAmount = 0;
            if (inventoryManager.SplitAmount > inventoryManager.MaxStackCount)
                inventoryManager.SplitAmount = inventoryManager.MaxStackCount;

            inventoryManager.stackTxt.text = inventoryManager.SplitAmount.ToString();
        }

        public void ShowToolTip(GameObject slot)
        {
            var tmpSlot = slot.GetComponent<Slot.Slot>();

            if (!tmpSlot.IsEmpty && inventoryManager.HoverObject == null &&
                !inventoryManager.selectStackSize.activeSelf)
            {
                inventoryManager.visualTextObject.text = tmpSlot.CurrentItem.GetTooltip();
                inventoryManager.sizeTextObject.text = inventoryManager.visualTextObject.text;

                inventoryManager.toolTipObject.SetActive(true);
            }
        }

        public void HideToolTip()
        {
            inventoryManager.toolTipObject.SetActive(false);
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

                statsUI.SetActive(!statsUI.activeSelf);
                inventoryUI.SetActive(!inventoryUI.activeSelf);

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

                statsUI.SetActive(!statsUI.activeSelf);
                inventoryUI.SetActive(!inventoryUI.activeSelf);
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

            for (var i = 0; i < _slots.Length; i++)
            {
                var tmp = _slots[i].GetComponent<Slot.Slot>();

                if (!tmp.IsEmpty) content += i + "-" + tmp.CurrentItem.type + "-" + tmp.Items.Count + ";";
            }

            PlayerPrefs.SetString("content", content);
            PlayerPrefs.SetInt("slots", slots);
            var position = inventoryUI.transform.position;
            PlayerPrefs.SetFloat("xPos", position.x);
            PlayerPrefs.SetFloat("yPos", position.y);

            PlayerPrefs.Save();
        }

        private void LoadInventory()
        {
            var content = PlayerPrefs.GetString("content");
            slots = PlayerPrefs.GetInt("slots");

            inventoryUI.transform.position = new Vector3(PlayerPrefs.GetFloat("xPos"), PlayerPrefs.GetFloat("yPos"),
                inventoryUI.transform.position.z);

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

        private string SetDropItemName(Item item)
        {
            string dropItemName;

            switch (item.rarity)
            {
                case Rarity.Common:
                    dropItemName = "<color=#696969>" + item.itemName + "</color>";
                    return dropItemName;
                case Rarity.Normal:
                    dropItemName = "<color=yellow>" + item.itemName + "</color>";
                    return dropItemName;
                case Rarity.Uncommon:
                    dropItemName = "<color=#bfff00>" + item.itemName + "</color>";
                    return dropItemName;
                case Rarity.Rare:
                    dropItemName = "<color=#bc3c21>" + item.itemName + "</color>";
                    return dropItemName;
                case Rarity.VeryRare:
                    dropItemName = "<color=#00CED1>" + item.itemName + "</color>";
                    return dropItemName;
                case Rarity.Epic:
                    dropItemName = "<color=orange><b>" + item.itemName + "</b></color>";
                    return dropItemName;
                case Rarity.Legendary:
                    dropItemName = "<color=#ff00ff><b>" + item.itemName + "</b></color>";
                    return dropItemName;
                case Rarity.Mystical:
                    dropItemName = "<color=red><b>" + item.itemName + "</b></color>";
                    return dropItemName;
                case Rarity.Artifact:
                    dropItemName = "<color=white><b>" + item.itemName + "</b></color>";
                    return dropItemName;
            }

            return null;
        }
    }
}