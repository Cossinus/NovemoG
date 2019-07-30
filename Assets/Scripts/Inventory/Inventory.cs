using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    #region Singleton
    
    public static Inventory Instance;

    void Awake()
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
    
    public Transform itemsParent;
    public GameObject inventoryUI;
    public Canvas canvas;

    public int slots;

    private Slot[] _slots;

    private static Slot _from, _to;

    public GameObject iconPrefab;
    private static GameObject _hoverObject;

    private float _hoverYOffset;

    public EventSystem eventSystem;

    public CanvasGroup canvasGroup;

    private bool fadingIn;
    private bool fadingOut;
    public float fadeTime;

    private static int _emptySlots;
    public static int EmptySlots {
        get => _emptySlots;
        set => _emptySlots = value;
    }

    void Start()
    {
        inventoryUI.gameObject.SetActive(true);

        _slots = itemsParent.GetComponentsInChildren<Slot>();

        _emptySlots = slots;

        _hoverYOffset = 100f * 0.01f;

        inventoryUI.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown("Inventory")) {
            if (canvasGroup.alpha > 0)
            {
                StartCoroutine(nameof(FadeOut));
                PutItemBack();
            }
            else
            {
                StartCoroutine(nameof(FadeIn));
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (!eventSystem.IsPointerOverGameObject(-1) && _from != null)
            {
                _from.icon.color = Color.white;
                _from.ClearSlot();
                Destroy(GameObject.Find("Hover"));
                _to = null;
                _from = null;
                EmptySlots++;
            }
        }

        if (_hoverObject != null)
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition,
                canvas.worldCamera, out position);
            position.Set(position.x, position.y - _hoverYOffset);
            _hoverObject.transform.position = canvas.transform.TransformPoint(position);
        }
    }
    
    public bool AddItem(Item item)
    {
        if (item.stackLimit == 1 || !item.isStackable) {
            PlaceEmpty(item);
            return true;
        }

        if (item.isStackable && item.stackLimit > 1) {
            foreach (Slot slot in _slots) {
                Slot tmp = slot.GetComponent<Slot>();

                if (tmp.IsNullOrEmpty) continue;
                if (tmp.CurrentItem.type != item.type || !tmp.IsAvailable) continue;
                tmp.AddItem(item);
                return true;
            }

            if (_emptySlots > 0) {
                PlaceEmpty(item);
                return true;
            }
        }
        
        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
        
        return false;
    }

    private bool PlaceEmpty(Item item)
    {
        if (_emptySlots > 0) {
            foreach (Slot slot in _slots) {
                Slot tmp = slot.GetComponent<Slot>();
                if (tmp.IsNullOrEmpty) {
                    tmp.AddItem(item);
                    _emptySlots--;
                    return true;
                }
            }
        }
        
        return false;
    }

    public void MoveItem(GameObject clicked)
    {
        if (_from == null && canvasGroup.alpha == 1)
        {
            if (!clicked.GetComponent<Slot>().IsNullOrEmpty)
            {
                _from = clicked.GetComponent<Slot>();
                _from.icon.color = Color.gray;

                _hoverObject = Instantiate(iconPrefab, GameObject.Find("Inventory Canvas").transform, true);
                _hoverObject.GetComponent<Image>().sprite = clicked.transform.Find("ItemButton/Icon").GetComponent<Image>().sprite;
                _hoverObject.name = "Hover";
                
                RectTransform hoverTransform = _hoverObject.GetComponent<RectTransform>();
                RectTransform clickedTransform = clicked.GetComponent<RectTransform>();
                
                hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, clickedTransform.sizeDelta.x - 30f);
                hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, clickedTransform.sizeDelta.y - 30f);
                _hoverObject.transform.localScale = _from.gameObject.transform.localScale;
            }
        }
        else if (_to == null)
        {
            _to = clicked.GetComponent<Slot>();
            Destroy(GameObject.Find("Hover"));
        }

        if (_to != null && _from != null)
        {
            Stack<Item> tmpTo = new Stack<Item>(_to.Items);
            _to.AddItems(_from.Items);

            if (tmpTo.Count == 0)
            {
                _from.ClearSlot();
            }
            else
            {
                _from.AddItems(tmpTo);
            }

            _from.icon.color = Color.white;
            _to = null;
            _from = null;
            Destroy(GameObject.Find("Hover"));
        }
    }

    private void PutItemBack()
    {
        if (_from != null)
        {
            Destroy(GameObject.Find("Hover"));
            _from.icon.color = Color.white;
            _from = null;
        }
    }

    private IEnumerator FadeOut()
    {
        if (!fadingOut)
        {
            fadingOut = true;
            fadingIn = false;
            StopCoroutine(nameof(FadeIn));

            float startAlpha = canvasGroup.alpha;
            float rate = 1.0f / fadeTime;
            float progress = 0.0f;

            while (progress < 1.0)
            {
                canvasGroup.alpha = Mathf.Lerp(startAlpha, 0, progress);
                progress += rate * Time.deltaTime;
                yield return null;
            }

            inventoryUI.SetActive(!inventoryUI.activeSelf);
            canvasGroup.alpha = 0;
            fadingOut = false;
        }
    }
    
    private IEnumerator FadeIn()
    {
        if (!fadingIn)
        {
            fadingOut = false;
            fadingIn = true;
            StopCoroutine(nameof(FadeOut));

            float startAlpha = canvasGroup.alpha;
            float rate = 1.0f / fadeTime;
            float progress = 0.0f;

            inventoryUI.SetActive(!inventoryUI.activeSelf);
            while (progress < 1.0)
            {
                canvasGroup.alpha = Mathf.Lerp(startAlpha, 1, progress);
                progress += rate * Time.deltaTime;
                yield return null;
            }
            
            canvasGroup.alpha = 1;
            fadingIn = false;
        }
    }
}
