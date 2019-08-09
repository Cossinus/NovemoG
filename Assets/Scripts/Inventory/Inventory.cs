using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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

    public EventSystem eventSystem;

    private static TextMeshProUGUI _sizeText;
    private static TextMeshProUGUI _visualText;
    public TextMeshProUGUI sizeTextObject;
    public TextMeshProUGUI visualTextObject;
    public TextMeshProUGUI stackTxt;

    public Transform itemsParent;

    public Slider splitSlider;

    public CanvasGroup canvasGroup;
    public Canvas canvas;

    private static GameObject _hoverObject;
    private static GameObject _toolTip;
    private static GameObject _clicked;
    private static GameObject _staticSelectStackSize;
    private static GameObject _playerRef;
    public GameObject clicked;
    public GameObject selectStackSize;
    public GameObject inventoryUI;
    public GameObject iconPrefab;
    public GameObject toolTipObject;
    public GameObject dropItem;

    private static Slot _from, _to;
    private static Slot _movingSlot;
    private Slot[] _slots;

    private int _splitAmount;
    private int _maxStackCount;
    public int slots;

    private float _hoverYOffset;
    public float fadeTime;

    private bool _fadingIn;
    private bool _fadingOut;

    public static int EmptySlots { get; set; }

    private void Start()
    {
        _toolTip = toolTipObject;
        _sizeText = sizeTextObject;
        _playerRef = GameObject.Find("Player");
        _visualText = visualTextObject;

        _staticSelectStackSize = selectStackSize;

        if (_slots != null)
            foreach (var go in _slots)
                Destroy(go);

        _slots = itemsParent.GetComponentsInChildren<Slot>();

        EmptySlots = slots;

        _hoverYOffset = 100f * 0.01f;

        _movingSlot = GameObject.Find("MovingSlot").GetComponent<Slot>();

        inventoryUI.SetActive(false);
    }

    private void Update()
    {
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
            if (!eventSystem.IsPointerOverGameObject(-1) && _from != null)
            {
                _from.icon.color = Color.white;

                foreach (Item item in _from.Items)
                {
                    float angle = UnityEngine.Random.Range(0.0f, Mathf.PI * 2);
                    
                    Vector3 v = new Vector3(Mathf.Sin(angle),0,Mathf.Cos(angle));
                    v *= 3;

                    GameObject tmpDrp = Instantiate(dropItem, _playerRef.transform.position - v, Quaternion.identity);
                    tmpDrp.transform.Rotate(-90f, 180f, -180f);
                    tmpDrp.GetComponent<ItemPickup>().item = _from.CurrentItem;
                }
                
                _from.ClearSlot();
                Destroy(GameObject.Find("Hover"));
                _to = null;
                _from = null;
                EmptySlots++;
            }
            else if (!eventSystem.IsPointerOverGameObject(-1) && !_movingSlot.IsEmpty)
            {
                foreach (Item item in _movingSlot.Items)
                {
                    float angle = UnityEngine.Random.Range(0.0f, Mathf.PI * 2);
                    
                    Vector3 v = new Vector3(Mathf.Sin(angle),0,Mathf.Cos(angle));
                    v *= 3;

                    GameObject tmpDrp = Instantiate(dropItem, _playerRef.transform.position - v, Quaternion.identity);
                    tmpDrp.transform.Rotate(-90f, 180f, -180f);
                    tmpDrp.GetComponent<ItemPickup>().item = _movingSlot.CurrentItem;
                }
                
                _movingSlot.ClearSlot();
                Destroy(GameObject.Find("Hover"));
            }
        }

        if (_hoverObject != null)
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
                Input.mousePosition,
                canvas.worldCamera, out position);
            position.Set(position.x, position.y - _hoverYOffset);
            _hoverObject.transform.position = canvas.transform.TransformPoint(position);
        }
    }

    public bool AddItem(Item item)
    {
        if (item.stackLimit == 1) return PlaceEmpty(item);

        if (item.stackLimit > 1)
        {
            foreach (var slot in _slots)
            {
                var tmp = slot.GetComponent<Slot>();

                if (tmp.IsEmpty) continue;
                if (tmp.CurrentItem.type != item.type || !tmp.IsAvailable) continue;
                if (!_movingSlot.IsEmpty && tmp.Items.Count == item.stackLimit - _movingSlot.Items.Count)
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

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();

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
        _clicked = clicked;
        selectStackSize.SetActive(false);

        if (!_movingSlot.IsEmpty)
        {
            var tmp = clicked.GetComponent<Slot>();

            if (tmp.IsEmpty)
            {
                tmp.AddItems(_movingSlot.Items);
                _movingSlot.Items.Clear();
                Destroy(GameObject.Find("Hover"));
            }
            else if (!tmp.IsEmpty && _movingSlot.CurrentItem.type == tmp.CurrentItem.type && tmp.IsAvailable)
            {
                MergeStacks(_movingSlot, tmp);
            }
        }
        else if (_from == null && canvasGroup.alpha == 1)
        {
            if (!clicked.GetComponent<Slot>().IsEmpty)
            {
                _from = clicked.GetComponent<Slot>();
                _from.icon.color = Color.gray;

                CreateHoverIcon();
            }
        }
        else if (_to == null)
        {
            _to = clicked.GetComponent<Slot>();
            Destroy(GameObject.Find("Hover"));
        }

        if (_to != null && _from != null)
        {
            var tmpTo = new Stack<Item>(_to.Items);
            _to.AddItems(_from.Items);

            if (tmpTo.Count == 0)
                _from.ClearSlot();
            else
                _from.AddItems(tmpTo);

            _from.icon.color = Color.white;
            _to = null;
            _from = null;
            Destroy(GameObject.Find("Hover"));
        }
    }

    private void CreateHoverIcon()
    {
        _hoverObject = Instantiate(iconPrefab, GameObject.Find("Inventory Canvas").transform, true);
        _hoverObject.transform.Find("ItemButton/Icon").GetComponent<Image>().sprite = clicked == null
            ? _clicked.transform.Find("ItemButton/Icon").GetComponent<Image>().sprite
            : clicked.transform.Find("ItemButton/Icon").GetComponent<Image>().sprite;
        
        _hoverObject.transform.Find("ItemButton/Icon").gameObject.SetActive(true);
        _hoverObject.name = "Hover";

        var hoverTransform = _hoverObject.GetComponent<RectTransform>();
        var clickedTransform = clicked == null
            ? _clicked.GetComponent<RectTransform>()
            : clicked.GetComponent<RectTransform>();

        hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, clickedTransform.sizeDelta.x - 50f);
        hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, clickedTransform.sizeDelta.y - 50f);

        _hoverObject.transform.localScale = clicked == null
            ? _clicked.gameObject.transform.localScale
            : clicked.gameObject.transform.localScale;

        _hoverObject.transform.Find("ItemButton/Amount").gameObject.SetActive(true);
        if (!_movingSlot.IsEmpty)
            _hoverObject.transform.Find("ItemButton/Amount").GetComponent<Text>().text =
                _movingSlot.Items.Count > 1 ? _movingSlot.Items.Count.ToString() : string.Empty;
        else
            _hoverObject.transform.Find("ItemButton/Amount").GetComponent<Text>().text =
                _from.Items.Count > 1 ? _from.Items.Count.ToString() : string.Empty;
    }

    private void PutItemBack()
    {
        if (_from != null)
        {
            Destroy(GameObject.Find("Hover"));
            _from.icon.color = Color.white;
            _from = null;
        }
        else if (!_movingSlot.IsEmpty)
        {
            Destroy(GameObject.Find("Hover"));
            foreach (var item in _movingSlot.Items) _clicked.GetComponent<Slot>().AddItem(item);

            _movingSlot.ClearSlot();
        }

        selectStackSize.SetActive(false);
    }

    public void SetStackInfo(int maxStackCount)
    {
        selectStackSize.SetActive(true);
        _toolTip.SetActive(false);
        _splitAmount = 0;
        _maxStackCount = maxStackCount;
        splitSlider.maxValue = maxStackCount;
        stackTxt.text = _splitAmount.ToString();
    }

    public void SplitStack()
    {
        selectStackSize.SetActive(false);

        if (_splitAmount == _maxStackCount)
        {
            MoveItem(clicked);
        }
        else if (_splitAmount > 0)
        {
            _movingSlot.Items = clicked.GetComponent<Slot>().RemoveItems(_splitAmount);
            CreateHoverIcon();
            clicked = null;
        }
    }

    public void ChangeStackText()
    {
        _splitAmount = (int)splitSlider.value;

        if (_splitAmount < 0) _splitAmount = 0;
        if (_splitAmount > _maxStackCount) _splitAmount = _maxStackCount;

        stackTxt.text = _splitAmount.ToString();
    }

    public void MergeStacks(Slot source, Slot destination)
    {
        var max = destination.CurrentItem.stackLimit - destination.Items.Count;
        var count = source.Items.Count < max ? source.Items.Count : max;

        for (var i = 0; i < count; i++)
        {
            destination.AddItem(source.RemoveItem());
            _hoverObject.transform.Find("ItemButton/Amount").GetComponent<Text>().text =
                _movingSlot.Items.Count.ToString();
        }

        if (source.Items.Count == 0)
        {
            source.ClearSlot();
            Destroy(GameObject.Find("Hover"));
        }
    }

    public void ShowToolTip(GameObject slot)
    {
        var tmpSlot = slot.GetComponent<Slot>();

        if (!tmpSlot.IsEmpty && _hoverObject == null && !_staticSelectStackSize.activeSelf)
        {
            _visualText.text = tmpSlot.CurrentItem.GetTooltip();
            _sizeText.text = _visualText.text;

            _toolTip.SetActive(true);

            var xPos = slot.transform.position.x;
            var yPos = slot.transform.position.y + slot.GetComponent<RectTransform>().sizeDelta.y - 100f;

            _toolTip.transform.position = new Vector2(xPos, yPos);
        }
    }

    public void HideToolTip()
    {
        _toolTip.SetActive(false);
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
}