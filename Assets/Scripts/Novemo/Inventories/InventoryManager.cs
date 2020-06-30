using System.Collections;
using System.Linq;
using Novemo.Characters.Player;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Novemo.Inventories
{
    public class InventoryManager : MonoBehaviour
    {
        #region Singleton

        public static InventoryManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        #endregion
        
        public Image blur;

        public GameObject selectStackSize;
        public GameObject iconPrefab;
        public GameObject toolTipObject;
        public GameObject dropItem;
        
        private static GameObject _playerRef;
        
        public GameObject HoverObject { get; set; }
        public GameObject Clicked { get; set; }
        
        public TextMeshProUGUI sizeTextObject;
        public TextMeshProUGUI visualTextObject;
        public TextMeshProUGUI stackTxt;

        public Slider splitSlider;
        
        public Canvas canvas;
        public CanvasScaler canvasScaler;

        public EventSystem eventSystem;
        
        public Slots.Slot movingSlotPlaceholder;
        public Slots.Slot MovingSlot { get; private set; }
        public Slots.Slot From { get; set; }
        public Slots.Slot To { get; set; }

        public int MaxStackCount { get; private set; }
        public int SplitAmount { get; set; }
        
        public float fadeTime;
        private float _widthRatio;
        private float _heightRatio;
        private const float HoverYOffset = 100f * 0.01f;

        private Transform inventoryTransform;
        private RectTransform visualTextRect;
        
        private static readonly int Size = Shader.PropertyToID("_Size");

        private void Start()
        {
            _playerRef = PlayerManager.Instance.player;
            
            MovingSlot = movingSlotPlaceholder;
            blur.material.SetFloat(Size, 0);
            inventoryTransform = canvas.transform.Find("Inventory").transform;
            visualTextRect = visualTextObject.GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (HoverObject != null)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvas.transform as RectTransform,
                    Input.mousePosition,
                    canvas.worldCamera, out var position);
                position.Set(position.x, position.y - HoverYOffset);
                
                HoverObject.transform.position =
                    canvas.transform.TransformPoint(position);
            }
            
            if (toolTipObject.activeSelf)
            {
                var xPos = Input.mousePosition.x;
                var yPos = Input.mousePosition.y;

                //TODO modify it only on resolution change
                _widthRatio = Screen.width / canvasScaler.referenceResolution.x;
                _heightRatio = Screen.height / canvasScaler.referenceResolution.y;

                var localScale = inventoryTransform.localScale;
                toolTipObject.transform.position = new Vector2(
                    Mathf.Clamp(xPos, 14 * _widthRatio,
                        Screen.width - (visualTextRect.rect.width + 22) * _widthRatio * localScale.x),
                    Mathf.Clamp(yPos, (visualTextRect.rect.height + 22) * _heightRatio * localScale.y, 
                        Screen.height - 14 * _heightRatio * localScale.y));
            }
            
            if (Input.GetMouseButtonUp(0) && !eventSystem.IsPointerOverGameObject(-1) && !MovingSlot.IsEmpty)
            {
                Inventory.Instance.DropItems(MovingSlot.Items.ToList(), _playerRef.transform);
                
                MovingSlot.ClearSlot();
                Destroy(GameObject.Find("Hover"));
            }

            if (!MovingSlot.IsEmpty)
            {
                toolTipObject.SetActive(false);
            }
        }
        
        public void SetStackInfo(int maxStackCount)
        {
            selectStackSize.SetActive(true);
            toolTipObject.SetActive(false);
            splitSlider.value = 0;
            SplitAmount = 0;
            MaxStackCount = maxStackCount;
            splitSlider.maxValue = maxStackCount;
            stackTxt.text = SplitAmount.ToString();
        }
        
        public IEnumerator FadeOut(CanvasGroup cGroup)
        {
            if (Metrics.EqualFloats(cGroup.alpha, 0, 0.01)) yield return null;
            
            StopCoroutine(FadeIn(cGroup));
            
            if (!MovingSlot.IsEmpty)
            {
                Inventory.Instance.DropItems(MovingSlot.Items.ToList(), PlayerManager.Instance.player.GetComponent<Player>().transform);
                    
                MovingSlot.ClearSlot();
                Destroy(GameObject.Find("Hover"));
            }

            var startBlurAlpha = blur.material.GetFloat(Size);
            var startAlpha = cGroup.alpha;
            var rate = 1.0f / fadeTime;
            var progress = 0.0f;
                
            while (progress < 1.0)
            {
                blur.material.SetFloat(Size, Mathf.Lerp(startBlurAlpha, 0, progress));
                cGroup.alpha = Mathf.Lerp(startAlpha, 0, progress);

                progress += rate * Time.deltaTime;
                yield return null;
            }
            cGroup.blocksRaycasts = false;
            cGroup.alpha = 0;
            
            blur.material.SetFloat(Size, 0);
        }

        public IEnumerator FadeIn(CanvasGroup cGroup)
        {
            if (Metrics.EqualFloats(cGroup.alpha, 1, 0.01)) yield return null;
            
            StopCoroutine(FadeOut(cGroup));

            var startBlurAlpha = blur.material.GetFloat(Size);
            var startAlpha = cGroup.alpha;
            var rate = 1.0f / fadeTime;
            var progress = 0.0f;
            
            cGroup.blocksRaycasts = true;

            while (progress < 1.0)
            {
                blur.material.SetFloat(Size, Mathf.Lerp(startBlurAlpha, 2, progress));

                cGroup.alpha = Mathf.Lerp(startAlpha, 1, progress);

                progress += rate * Time.deltaTime;
                yield return null;
            }

            cGroup.alpha = 1;
            blur.material.SetFloat(Size, 2);
        }
    }
}
