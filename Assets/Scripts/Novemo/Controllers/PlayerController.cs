using System;
using Novemo.Classes;
using Novemo.Items;
using Novemo.Interactables;
using Novemo.Inventories;
using Novemo.Items.Equipments;
using UnityEngine;

namespace Novemo.Controllers
{
	public class PlayerController : MonoBehaviour
	{
		//Currencies
		public int Cossbucks { get; set; }
		public int FutureCoin { get; set; }
		public int ShadowCoin { get; set; }
		public int PlagueCoin { get; set; }
		public int MagmaCoin { get; set; }

		public Item iron;
		public Item steel;

		//Player's class
		public Class PlayerClass { get; set; }

		//Player's animator
		public Animator animator;

		//Actual quest
		public Quest.Quest quest;

		//Player's inventory
		public Inventory inventory;
		public GameObject playerStatsObject;

		public Crafting.Crafting craftingBench;

		//Player's weapon
		public GameObject weapon;
		private SpriteRenderer _weaponSprite;
		
		//Player's stats
		private Characters.Character _myStats;
		
		//Player's chest
		private Inventory _chest;
		
		//Animator values
		private Vector2 _movement;
		private Vector2 _lastMovement;

		private static Rigidbody2D _rb2d;

		//Animator values
		private static readonly int Speed = Animator.StringToHash("Speed");
		private static readonly int Horizontal = Animator.StringToHash("Horizontal");
		private static readonly int Vertical = Animator.StringToHash("Vertical");
		private static readonly int LastHorizontal = Animator.StringToHash("LastHorizontal");
		private static readonly int LastVertical = Animator.StringToHash("LastVertical");

		public event Action<bool> OnPlayerMovement;

		private void Awake()
		{
			PlayerClass = FindObjectOfType<ClassHandler>().classes[0];
			_myStats = GetComponent<Characters.Character>();
			Cossbucks = 10;
		}
	
		private void Start()
		{
			_weaponSprite = weapon.GetComponent<SpriteRenderer>();
			_rb2d = GetComponent<Rigidbody2D>();
			PlayerClass.myStats = _myStats;
			PlayerClass.AddComponents();
			PlayerClass.InitializeValues();
			Invoke(nameof(Equip), 0.1f);
		}
    
		private void Update()
		{
			SetAnimatorValues();
			
			if (Input.GetKey(KeyCode.X))
            {
                inventory.AddItem(iron);
                inventory.AddItem(steel);
            }

			Quest();
		
			if (Input.GetKeyDown(KeyCode.L))
			{
				_myStats.LevelUp();
			}

			SetWeaponSortingLayer();

			OpenInventory();

			OpenCrafting();
			
			OpenChest();
		}

		private void FixedUpdate()
		{
			if (!_myStats.CanMove) return;

			_rb2d.MovePosition(_rb2d.position + new Vector2(Mathf.Lerp(0, Input.GetAxis("Horizontal") * _myStats.stats[6].GetValue(), 1f),
				                  Mathf.Lerp(0, Input.GetAxis("Vertical") * _myStats.stats[6].GetValue(), 1f)) * Time.fixedDeltaTime);
		}

		private void OpenCrafting()
		{
			if (Input.GetButtonDown("Crafting"))
			{
				StartCoroutine(InventoryManager.Instance.FadeOut(playerStatsObject.GetComponent<CanvasGroup>()));
				
				if (!inventory.IsOpen && !craftingBench.IsOpen)
				{
					craftingBench.Open();
					inventory.Open();
				}
				else if (inventory.IsOpen && !craftingBench.IsOpen)
				{
					craftingBench.Open();
				}
				else if (inventory.IsOpen && craftingBench.IsOpen)
				{
					craftingBench.Open();
					inventory.Open();
				}
			}
		}

		private void OpenChest()
		{
			if (Input.GetButtonDown("Chest"))
			{
				if (_chest == null) return;
				
				StartCoroutine(InventoryManager.Instance.FadeOut(playerStatsObject.GetComponent<CanvasGroup>()));
					
				if (!inventory.IsOpen && !_chest.IsOpen)
				{
					_chest.Open();
					inventory.Open();
				}
				else if (inventory.IsOpen && !_chest.IsOpen)
				{
					_chest.Open();
				}
				else if (inventory.IsOpen && _chest.IsOpen)
				{
					_chest.Open();
					inventory.Open();
				}
			}
		}

		private void OpenInventory()
		{
			if (Input.GetButtonDown("Inventory"))
			{
				if (!inventory.IsOpen)
				{
					playerStatsObject.GetComponent<CanvasGroup>().alpha = 1;
					playerStatsObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
					playerStatsObject.SetActive(true);
				}
				
				inventory.Open();
				if (_chest != null && _chest.IsOpen)
				{
					_chest.Open();
					playerStatsObject.SetActive(false);
				}
				if (craftingBench.IsOpen)
				{
					craftingBench.Open();
				}
			}
		}

		private void Quest()
		{
			if (quest.isActive)
			{
				quest.goal.EnemyKilled();
				if (quest.goal.IsReached())
				{
					//REWARDS
					Cossbucks += 7;
					quest.Complete();
				}
			}
		}

		private void SetWeaponSortingLayer()
		{
			if (_lastMovement.y > 0.1f)
			{
				_weaponSprite.sortingOrder = 1;
			}
			else
			{
				_weaponSprite.sortingOrder = -1;
			}
		}

		private void SetAnimatorValues()
		{
			_movement.x = Input.GetAxisRaw("Horizontal");
			_movement.y = Input.GetAxisRaw("Vertical");

			if (Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f)
			{
				_lastMovement = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);
				OnPlayerMovement?.Invoke(true);
			}
			if (Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f)
			{
				_lastMovement = new Vector2(0f, Input.GetAxisRaw("Vertical"));
				OnPlayerMovement?.Invoke(true);
			}
			
			animator.SetFloat(Speed, _movement.sqrMagnitude);
			animator.SetFloat(Horizontal, _movement.x);
			animator.SetFloat(Vertical, _movement.y);
			animator.SetFloat(LastHorizontal, _lastMovement.x);
			animator.SetFloat(LastVertical, _lastMovement.y);
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.gameObject.CompareTag("Item"))
			{
				other.GetComponent<ItemPickup>().Interact();
			}

			if (other.gameObject.CompareTag("Chest"))
			{
				_chest = other.GetComponent<ChestScript>().chestInventory;
			}
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			if (other.gameObject.CompareTag("Chest"))
			{
				if (_chest.IsOpen)
				{
					_chest.Open();
					inventory.Open();
				}
				_chest = null;
			}
		}

		private void OnTriggerStay2D(Collider2D other)
		{
			if (other.gameObject.CompareTag("Interact"))
			{
				if (Input.GetButtonDown("Interact"))
				{
					other.GetComponent<Interactable>().Interact();
				}
			}
		}

		private void Equip()
		{
			EquipmentManager.Instance.Equip(Instantiate(PlayerClass.defaultWeapon));
		}
	}
}
