using Novemo.Characters.Player;
using Novemo.Items;
using Novemo.StatusEffects;
using UnityEngine;

namespace Novemo.SkillTree
{
	[System.Serializable]
	public class Skill : MonoBehaviour
	{
		public string skillName;
		[Multiline]
		public string skillDescription;

		public GameObject[] connections;
		
		public Skill[] children;
		public Skill[] parents;
		
		public int level;
		public int requirement;
		public Scroll scroll;

		public bool acquired;
		public bool discovered;
		public StatusEffect PassiveEffect { get; set; }

		private void OnValidate()
		{
			if (scroll != null) PassiveEffect = scroll.statusEffect;
		}

		public void Acquire()
		{
			if (level <= PlayerManager.Instance.player.GetComponent<Player>().level) return;
			
			foreach (var parent in parents)
			{
				parent.Discover();
			}
		}

		private void Discover()
		{
			foreach (var child in children)
			{
				if (!child.discovered) break;
				
				gameObject.SetActive(true);

				foreach (var connection in connections)
				{
					connection.SetActive(true);
				}
			}
		}
	}
}