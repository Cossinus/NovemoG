using System.Collections;
using Novemo.Character.Player;
using Novemo.Characters.Player;
using Novemo.Controllers;
using Novemo.Items;
using Novemo.UI;
using UnityEngine;

namespace Novemo.Interactable
{
	public class Ore : Interactable
	{
		public Gem gem;

		public Sprite oreSprite;

		public SpriteRenderer gfx;
		
		private static Inventories.Inventory _inventory;
		private static ProgressBar _progressBar;
		private static PlayerController _player;

		private Pickaxe playerPickaxe;

		private Coroutine _coroutine;

		private void Start()
		{
			_inventory = Inventories.Inventory.Instance;
			_player = PlayerManager.Instance.player.GetComponent<PlayerController>();
			_progressBar = PlayerManager.Instance.uiCanvas.GetComponent<ProgressBar>();

			gfx.sprite = oreSprite;
			gfx.sortingLayerName = "Interactable";
			gfx.sortingOrder = -1;
		}

		public override void Interact()
		{
			playerPickaxe = (Pickaxe) _inventory.GetItemWithName("Pickaxe");
			if (_player.GetComponent<Player>().Stunned || _player.GetComponent<Player>().Silenced ||
			    playerPickaxe == null || playerPickaxe.hardness < gem.hardness ||
			    _player.GetComponent<Player>().level < playerPickaxe.level ||
			    playerPickaxe.currentDurability < (int) gem.hardness)
			{
				//EventLog
				return;
			}

			if (_coroutine != null) StopCoroutine(_coroutine);
			_progressBar.FillSlider((float) gem.hardness * 1.5f);
			_coroutine = StartCoroutine(Collect());
		}

		private IEnumerator Collect()
		{
			_player.OnPlayerMovement += OnPlayerMovement;

			var rate = 1.0f / ((float) gem.hardness * 1.5f);
			var progress = 0.0f;

			while (progress < 1.0)
			{
				if (_player.GetComponent<Characters.Character>().Stunned || _player.GetComponent<Characters.Character>().Silenced)
				{
					Disable();
				}

				progress += rate * Time.deltaTime;
				yield return null;
			}

			_player.OnPlayerMovement -= OnPlayerMovement;

			_inventory.DropItem(gem, transform);
			
			playerPickaxe.currentDurability -= (int) gem.hardness;
			playerPickaxe.SetDescription();

			//Play animation
			
			Destroy(gameObject);
		}

		private void OnPlayerMovement(bool isMoving)
		{
			if (!isMoving) return;
			Disable();
		}

		private void Disable()
		{
			_progressBar.DisableSlider();
			_player.OnPlayerMovement -= OnPlayerMovement;
			StopCoroutine(_coroutine);
		}
	}
}