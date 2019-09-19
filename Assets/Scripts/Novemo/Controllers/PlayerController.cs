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
		private float Gold = 10;

		public Quest.Quest quest;

		public ClassManager playerClass;

		public Rigidbody2D rb;

		private Vector2 movement;
	
		private Camera cam;

		private CharacterStats myStats;

		private ItemPickup pickUp;

		void Awake()
		{
			myStats = gameObject.GetComponent<CharacterStats>();
		}
	
		void Start()
		{
			cam = Camera.main;

			StartCoroutine(Equip());
		}
    
		void Update()
		{
			transform.Translate(
				Time.deltaTime * myStats.stats[6].GetValue() *
				(transform.up * Input.GetAxisRaw("Vertical") + 
				 transform.right * Input.GetAxisRaw("Horizontal")).normalized);
	    
			if (EventSystem.current.IsPointerOverGameObject())
				return;

			if (quest.isActive)
			{
				quest.goal.EnemyKilled();
				if (quest.goal.IsReached())
				{
					//REWARDS
					quest.Complete();
				}
			}
		
			if (Input.GetKeyDown(KeyCode.L))
				playerClass.LevelUp();
		}

		void FixedUpdate()
		{
			rb.MovePosition(rb.position + Time.fixedDeltaTime * myStats.stats[6].GetValue() * movement);
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
