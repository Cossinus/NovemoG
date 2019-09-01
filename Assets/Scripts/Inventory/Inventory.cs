using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    // Actual Inventory Parent Object
    public Transform itemsParent;

    // Canvas Group of an Inventory
    public CanvasGroup canvasGroup;
    
    // Game Objects
    private static GameObject _playerRef;
    public GameObject inventoryUI;

    // All Slots in Inventory
    private Slot[] _slots;

    // Floats
    private float _hoverYOffset;
    public float fadeTime;
    
    private bool _fadingIn;
    private bool _fadingOut;

    public Item iron;
    
    // EmptySlots and All Slots as an int
    public static int EmptySlots { get; set; }
    public int emptySlots;
    public int slots;

    private void Start()
    {
        _playerRef = PlayerManager.Instance.player;

        if (_slots != null)
            foreach (var go in _slots)
                Destroy(go);

        _slots = itemsParent.GetComponentsInChildren<Slot>();

        EmptySlots = slots;

        _hoverYOffset = 100f * 0.01f;

        InventoryManager.Instance.MovingSlot = GameObject.Find("MovingSlot").GetComponent<Slot>();

        inventoryUI.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            AddItem(iron);
        }

        if (Input.GetButtonDown("Inventory"))
        {
            if (canvasGroup.alpha == 0)
            {
                StartCoroutine(nameof(FadeIn));
            }
            else
            {
                StartCoroutine(nameof(FadeOut));
                PutItemBack();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            DropItem();
        }

        if (InventoryManager.Instance.HoverObject != null)
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(InventoryManager.Instance.canvas.transform as RectTransform,
                Input.mousePosition,
                InventoryManager.Instance.canvas.worldCamera, out position);
            position.Set(position.x, position.y - _hoverYOffset);
            InventoryManager.Instance.HoverObject.transform.position = InventoryManager.Instance.canvas.transform.TransformPoint(position);
        }
        emptySlots = EmptySlots;
    }

    private void DropItem()
    {
        if (!InventoryManager.Instance.eventSystem.IsPointerOverGameObject(-1) &&
            InventoryManager.Instance.From != null)
        {
            InventoryManager.Instance.From.icon.color = Color.white;

            foreach (Item item in InventoryManager.Instance.From.Items)
            {
                float angle = UnityEngine.Random.Range(0.0f, Mathf.PI * 2);

                Vector3 v = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));
                v *= 2;

                GameObject tmpDrp = Instantiate(InventoryManager.Instance.dropItem, _playerRef.transform.position - v,
                    Quaternion.identity);
                tmpDrp.transform.Find("Canvas/SizeItemName/ItemName").GetComponent<TextMeshProUGUI>().text = SetDropItemName(item);
                tmpDrp.transform.Find("Canvas/SizeItemName").GetComponent<TextMeshProUGUI>().text =
                    SetDropItemName(item);
                tmpDrp.transform.Rotate(-90f, 180f, -180f);
                tmpDrp.GetComponent<ItemPickup>().item = InventoryManager.Instance.From.CurrentItem;
            }

            InventoryManager.Instance.From.ClearSlot();
            Destroy(GameObject.Find("Hover"));
            InventoryManager.Instance.To = null;
            InventoryManager.Instance.From = null;
        }
        else if (!InventoryManager.Instance.eventSystem.IsPointerOverGameObject(-1) &&
                 !InventoryManager.Instance.MovingSlot.IsEmpty)
        {
            foreach (Item item in InventoryManager.Instance.MovingSlot.Items)
            {
                float angle = UnityEngine.Random.Range(0.0f, Mathf.PI * 2);

                Vector3 v = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));
                v *= 2;

                GameObject tmpDrp = Instantiate(InventoryManager.Instance.dropItem, _playerRef.transform.position - v,
                    Quaternion.identity);
                tmpDrp.transform.Find("Canvas/SizeItemName/ItemName").GetComponent<TextMeshProUGUI>().text =
                    SetDropItemName(item);
                tmpDrp.transform.Find("Canvas/SizeItemName").GetComponent<TextMeshProUGUI>().text =
                    SetDropItemName(item);
                tmpDrp.transform.Rotate(-90f, 180f, -180f);
                tmpDrp.GetComponent<ItemPickup>().item = InventoryManager.Instance.MovingSlot.CurrentItem;
            }

            InventoryManager.Instance.MovingSlot.ClearSlot();
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
                var tmp = slot.GetComponent<Slot>();
                
                if (tmp.IsEmpty) continue;
                if (tmp.CurrentItem.type != item.type || !tmp.IsAvailable) continue;
                if (InventoryManager.Instance.clicked != null &&
                    InventoryManager.Instance.clicked.GetComponent<Slot>() == tmp.GetComponent<Slot>() &&
                    tmp.Items.Count == item.stackLimit - InventoryManager.Instance.MovingSlot.Items.Count)
                {
                    continue;
                }
                else if (InventoryManager.Instance.Clicked != null &&
                    InventoryManager.Instance.Clicked.GetComponent<Slot>() == tmp.GetComponent<Slot>() &&
                    tmp.Items.Count == item.stackLimit - InventoryManager.Instance.MovingSlot.Items.Count)
                {
                    continue;
                }
                else
                {
                    tmp.AddItem(item);
                    return true;
                }
            }

            if (EmptySlots > 0) return PlaceEmpty(item); // TODO sprawdzić to jutro
        }

        onItemChangedCallback?.Invoke();

        return false;
    }

    private bool PlaceEmpty(Item item)
    {
        if (EmptySlots > 0)
            foreach (var slot in _slots)
            {
                var tmp = slot.GetComponent<Slot>();
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
        InventoryManager.Instance.Clicked = clicked;
        InventoryManager.Instance.selectStackSize.SetActive(false);
        Debug.Log(EmptySlots);

        if (!InventoryManager.Instance.MovingSlot.IsEmpty)
        {
            var tmp = clicked.GetComponent<Slot>();

            if (tmp.IsEmpty)
            {
                tmp.AddItems(InventoryManager.Instance.MovingSlot.Items);
                InventoryManager.Instance.MovingSlot.Items.Clear();
                Destroy(GameObject.Find("Hover"));
                EmptySlots--;
            }
            else if (!tmp.IsEmpty && InventoryManager.Instance.MovingSlot.CurrentItem.type == tmp.CurrentItem.type && tmp.IsAvailable)
            {
                MergeStacks(InventoryManager.Instance.MovingSlot, tmp);
            }
        }
        else if (InventoryManager.Instance.From == null && canvasGroup.alpha == 1)
        {
            if (!clicked.GetComponent<Slot>().IsEmpty)
            {
                EmptySlots++;
                InventoryManager.Instance.MovingSlot.Items = clicked.GetComponent<Slot>().RemoveItems(clicked.GetComponent<Slot>().Items.Count);
                CreateHoverIcon();
                clicked.GetComponent<Slot>().ClearSlot();
            }
        }
        else if (InventoryManager.Instance.To == null)
        {
            InventoryManager.Instance.To = clicked.GetComponent<Slot>();
            InventoryManager.Instance.Clicked = null;
            Destroy(GameObject.Find("Hover"));
        }

        if (InventoryManager.Instance.To != null && !InventoryManager.Instance.MovingSlot.IsEmpty)
        {
            var tmpTo = new Stack<Item>(InventoryManager.Instance.To.Items);
            if (tmpTo.Count == 0)
            {
                foreach (var item in InventoryManager.Instance.MovingSlot.Items)
                    clicked.GetComponent<Slot>().AddItem(item);
                
                InventoryManager.Instance.MovingSlot.ClearSlot();
                EmptySlots--;
            }
            else
            {
                MergeStacks(InventoryManager.Instance.From, InventoryManager.Instance.To);
            }
            
            InventoryManager.Instance.To = null;
            InventoryManager.Instance.From = null;
            Destroy(GameObject.Find("Hover"));
        }
        emptySlots = EmptySlots;
    }
    
    public void MergeStacks(Slot source, Slot destination)
    {
        var max = destination.CurrentItem.stackLimit - destination.Items.Count;
        var count = source.Items.Count < max ? source.Items.Count : max;

        for (var i = 0; i < count; i++)
        {
            destination.AddItem(source.RemoveItem());
            if (!InventoryManager.Instance.MovingSlot.IsEmpty)
                InventoryManager.Instance.HoverObject.transform.Find("ItemButton/Amount").GetComponent<Text>().text =
                    InventoryManager.Instance.MovingSlot.Items.Count.ToString();
            else
                InventoryManager.Instance.HoverObject.transform.Find("ItemButton/Amount").GetComponent<Text>().text = 
                    source.Items.Count.ToString();
        }

        if (source.Items.Count == 0)
        {
            source.ClearSlot();
            Destroy(GameObject.Find("Hover"));
        }
    }

    private void CreateHoverIcon()
    {
        InventoryManager.Instance.HoverObject = Instantiate(InventoryManager.Instance.iconPrefab, GameObject.Find("Inventory Canvas").transform, true);
        InventoryManager.Instance.HoverObject.transform.Find("ItemButton/Icon").GetComponent<Image>().sprite = InventoryManager.Instance.clicked == null
            ? InventoryManager.Instance.Clicked.transform.Find("ItemButton/Icon").GetComponent<Image>().sprite
            : InventoryManager.Instance.clicked.transform.Find("ItemButton/Icon").GetComponent<Image>().sprite;
        
        InventoryManager.Instance.HoverObject.transform.Find("ItemButton/Icon").gameObject.SetActive(true);
        InventoryManager.Instance.HoverObject.name = "Hover";

        var hoverTransform = InventoryManager.Instance.HoverObject.GetComponent<RectTransform>();
        var clickedTransform = InventoryManager.Instance.clicked == null
            ? InventoryManager.Instance.Clicked.GetComponent<RectTransform>()
            : InventoryManager.Instance.clicked.GetComponent<RectTransform>();

        hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, clickedTransform.sizeDelta.x - 50f);
        hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, clickedTransform.sizeDelta.y - 50f);

        InventoryManager.Instance.HoverObject.transform.localScale = InventoryManager.Instance.clicked == null
            ? InventoryManager.Instance.Clicked.gameObject.transform.localScale
            : InventoryManager.Instance.clicked.gameObject.transform.localScale;

        InventoryManager.Instance.HoverObject.transform.Find("ItemButton/Amount").gameObject.SetActive(true);
        if (!InventoryManager.Instance.MovingSlot.IsEmpty)
            InventoryManager.Instance.HoverObject.transform.Find("ItemButton/Amount").GetComponent<Text>().text =
                InventoryManager.Instance.MovingSlot.Items.Count > 1 ? InventoryManager.Instance.MovingSlot.Items.Count.ToString() : string.Empty;
        else
            InventoryManager.Instance.HoverObject.transform.Find("ItemButton/Amount").GetComponent<Text>().text =
                InventoryManager.Instance.From.Items.Count > 1 ? InventoryManager.Instance.From.Items.Count.ToString() : string.Empty;
    }

    private void PutItemBack()
    {
        if (EmptySlots == 0)
        {
            DropItem();
        }
        else
        {
            if (!InventoryManager.Instance.MovingSlot.IsEmpty)
            {
                Destroy(GameObject.Find("Hover"));
                foreach (var item in InventoryManager.Instance.MovingSlot.Items)
                {
                    if (InventoryManager.Instance.clicked != null)
                        InventoryManager.Instance.clicked.GetComponent<Slot>().AddItem(item);
                    else if (InventoryManager.Instance.Clicked != null)
                        InventoryManager.Instance.Clicked.GetComponent<Slot>().AddItem(item);
                }

                EmptySlots--;
                InventoryManager.Instance.MovingSlot.ClearSlot();
            }
        }

        InventoryManager.Instance.selectStackSize.SetActive(false);
    }

    public void SplitStack()
    {
        Debug.Log(EmptySlots);
        InventoryManager.Instance.selectStackSize.SetActive(false);

        if (InventoryManager.Instance.SplitAmount == InventoryManager.Instance.MaxStackCount)
        {
            InventoryManager.Instance.MovingSlot.Items = InventoryManager.Instance.clicked.GetComponent<Slot>().RemoveItems(InventoryManager.Instance.SplitAmount);
            CreateHoverIcon();
            InventoryManager.Instance.clicked.GetComponent<Slot>().ClearSlot();
            EmptySlots++;
        }
        else if (InventoryManager.Instance.SplitAmount > 0)
        {
            InventoryManager.Instance.MovingSlot.Items = InventoryManager.Instance.clicked.GetComponent<Slot>().RemoveItems(InventoryManager.Instance.SplitAmount);
            CreateHoverIcon();
        }
        
        InventoryManager.Instance.splitSlider.value = 0;
    }

    public void ChangeStackText()
    {
        InventoryManager.Instance.SplitAmount = (int)InventoryManager.Instance.splitSlider.value;

        if (InventoryManager.Instance.SplitAmount < 0) InventoryManager.Instance.SplitAmount = 0;
        if (InventoryManager.Instance.SplitAmount > InventoryManager.Instance.MaxStackCount) InventoryManager.Instance.SplitAmount = InventoryManager.Instance.MaxStackCount;

        InventoryManager.Instance.stackTxt.text = InventoryManager.Instance.SplitAmount.ToString();
    }

    public void ShowToolTip(GameObject slot)
    {
        var tmpSlot = slot.GetComponent<Slot>();

        if (!tmpSlot.IsEmpty && InventoryManager.Instance.HoverObject == null && !InventoryManager.Instance.selectStackSize.activeSelf)
        {
            InventoryManager.Instance.visualTextObject.text = tmpSlot.CurrentItem.GetTooltip();
            InventoryManager.Instance.sizeTextObject.text = InventoryManager.Instance.visualTextObject.text;

            InventoryManager.Instance.toolTipObject.SetActive(true);

            var xPos = slot.transform.position.x;
            var yPos = slot.transform.position.y + slot.GetComponent<RectTransform>().sizeDelta.y - 100f;

            InventoryManager.Instance.toolTipObject.transform.position = new Vector2(xPos, yPos);
        }
    }

    public void HideToolTip()
    {
        InventoryManager.Instance.toolTipObject.SetActive(false);
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
            var tmp = _slots[i].GetComponent<Slot>();

            if (!tmp.IsEmpty) content += i + "-" + tmp.CurrentItem.type + "-" + tmp.Items.Count + ";";
        }

        PlayerPrefs.SetString("content", content);
        PlayerPrefs.SetInt("slots", slots);
        PlayerPrefs.SetFloat("xPos", inventoryUI.transform.position.x);
        PlayerPrefs.SetFloat("yPos", inventoryUI.transform.position.y);

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