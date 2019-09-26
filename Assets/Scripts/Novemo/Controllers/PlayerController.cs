using System.Collections;
using Novemo.Classes;
using Novemo.Items;
using Novemo.Player;
using Novemo.Stats;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Novemo.Controllers
{
	public class PlayerController : MonoBehaviour
	{
		public float Gold { get; set; }

		public Quest.Quest quest;

		public ClassManager playerClass;

		public Animator animator;
		
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
			
			transform.Translate(Time.deltaTime * _myStats.stats[6].GetValue() *
			                    (transform.up * _movement.y + transform.right * _movement.x).normalized);
	    
			if (EventSystem.current.IsPointerOverGameObject())
				return;

			Quest();
		
			if (Input.GetKeyDown(KeyCode.L))
				playerClass.LevelUp();
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
		}

		IEnumerator Equip()
		{
			yield return new WaitForSeconds(.1f);
			EquipmentManager.Instance.Equip(playerClass.defaultWeapon);
		}
	}
}
