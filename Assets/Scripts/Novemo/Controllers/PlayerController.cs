using System.Collections;
using Novemo.Classes;
using Novemo.Inventory;
using Novemo.Items;
using Novemo.Player;
using Novemo.Stats;
using UnityEngine;

namespace Novemo.Controllers
{
	public class PlayerController : MonoBehaviour
	{
		public Item iron;
		
		public float Gold { get; set; }

		public Quest.Quest quest;

		public ClassManager playerClass;

		public Inventory.Inventory inventory;

		public Crafting.Crafting craftingBench;
		
		public Animator animator;

		private Inventory.Inventory chest;
		
		private Vector2 _movement;
		private Vector2 _lastMovement;

		private CharacterStats _myStats;

		private ItemPickup _pickUp;

		void Awake()
		{
			_myStats = gameObject.GetComponent<CharacterStats>();
			Gold = 10;
		}
	
		void Start()
		{
			StartCoroutine(Equip());
		}
    
		void Update()
		{
			SetAnimatorValues();
			
			if (Input.GetKey(KeyCode.X))
            {
                inventory.AddItem(iron);
            }
			
			transform.Translate(Time.deltaTime * _myStats.stats[6].GetValue() *
			                    (transform.up * _movement.y + transform.right * _movement.x).normalized);

			Quest();
		
			if (Input.GetKeyDown(KeyCode.L))
				playerClass.LevelUp();

			OpenInventory();

			OpenCrafting();
			
			OpenChest();
		}

		private void OpenCrafting()
		{
			if (Input.GetButtonDown("Crafting"))
			{
				inventory.Open();
				craftingBench.Open();
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
					quest.Complete();
				}
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

		IEnumerator Equip()
		{
			yield return new WaitForSeconds(.1f);
			EquipmentManager.Instance.Equip(playerClass.defaultWeapon);
		}
	}
}
