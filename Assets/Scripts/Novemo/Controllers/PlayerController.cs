using Novemo.Classes;
using Novemo.Inventory;
using Novemo.Items;
using Novemo.Player;
using Novemo.Stats;
using System.Collections;
using UnityEngine;

namespace Novemo.Controllers
{
	public class PlayerController : MonoBehaviour
	{
		//Currencies
		public int Currency1 { get; set; }
		public int Currency2 { get; set; }
		public int Currency3 { get; set; }
		public int Currency4 { get; set; }
		public int Currency5 { get; set; }

		public Item iron;
		public Item steel;

		//Player's class
		public ClassManager playerClass;

		//Player's animator
		public Animator animator;

		//Actual quest
		public Quest.Quest quest;

		//Player's stats
		private CharacterStats _myStats;

		//Player's inventory
		public Inventory.Inventory inventory;
		public GameObject playerStats;

		//Player's bench
		public Crafting.Crafting craftingBench;

		//Player's chest
		private Inventory.Inventory chest;
		
		//Player's weapon
		public GameObject weapon;
		private SpriteRenderer _weaponSprite;
		
		//Animator values
		private Vector2 _movement;
		private Vector2 _lastMovement;

		private void Awake()
		{
			_myStats = gameObject.GetComponent<CharacterStats>();
			Currency1 = 10;
		}
	
		private void Start()
		{
			Invoke(nameof(Equip), 0.1f);
			_weaponSprite = weapon.GetComponent<SpriteRenderer>();
		}
    
		private void Update()
		{
			SetAnimatorValues();
			
			if (Input.GetKey(KeyCode.X))
            {
                inventory.AddItem(iron);
                inventory.AddItem(steel);
            }
			
			transform.Translate(Time.deltaTime * _myStats.stats[6].GetValue() * (transform.up * _movement.y + transform.right * _movement.x).normalized);

			Quest();
		
			if (Input.GetKeyDown(KeyCode.L))
				playerClass.LevelUp();

			SetWeaponSortingLayer();

			OpenInventory();

			OpenCrafting();
			
			OpenChest();
		}

		private void OpenCrafting()
		{
			if (Input.GetButtonDown("Crafting"))
			{
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
				}
			}
		}

		private void OpenChest()
		{
			if (Input.GetButtonDown("Chest"))
			{
				if (chest != null)
				{
					if (!inventory.IsOpen && !chest.IsOpen)
					{
						chest.Open();
						inventory.Open();
					}
					else if (inventory.IsOpen && !chest.IsOpen)
					{
						chest.Open();
					}
					else if (inventory.IsOpen && chest.IsOpen)
					{
						chest.Open();
						inventory.Open();
					}
				}
			}
		}

		private void OpenInventory()
		{
			if (Input.GetButtonDown("Inventory"))
			{
				inventory.Open();
				if (chest != null && chest.IsOpen)
				{
					chest.Open();
					playerStats.SetActive(false);
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
					Currency1 += 7;
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
			}
			if (Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f)
			{
				_lastMovement = new Vector2(0f, Input.GetAxisRaw("Vertical"));
			}
			
			animator.SetFloat("Speed", _movement.sqrMagnitude);
			animator.SetFloat("Horizontal", _movement.x);
			animator.SetFloat("Vertical", _movement.y);
			animator.SetFloat("LastHorizontal", _lastMovement.x);
			animator.SetFloat("LastVertical", _lastMovement.y);
		}

		void OnTriggerEnter2D(Collider2D other)
		{
			if (other.gameObject.CompareTag("Item"))
			{
				other.gameObject.GetComponent<ItemPickup>().Interact();
			}

			if (other.gameObject.CompareTag("Chest"))
			{
				chest = other.GetComponent<ChestScript>().chestInventory;
			}
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			if (other.gameObject.CompareTag("Chest"))
			{
				if (chest.IsOpen)
				{
					chest.Open();
					inventory.Open();
				}
				chest = null;
			}
		}

		private void Equip()
		{
			EquipmentManager.Instance.Equip(playerClass.defaultWeapon);
		}
	}
}
