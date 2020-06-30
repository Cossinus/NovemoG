using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Novemo.SkillTree
{
	public class Tree : MonoBehaviour
	{
		public List<Skill> skills = new List<Skill>();

		private void Start()
		{
			skills = GetComponentsInChildren<Skill>().ToList();
		}
	}
}