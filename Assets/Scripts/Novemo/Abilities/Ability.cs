using System.Collections;
using Novemo.Player;
using Novemo.Stats;
using UnityEngine;

namespace Novemo.Abilities
{
    public class Ability : MonoBehaviour
    {
	    public float Delay { get; set; }
        public float Cooldown { get; set; }
		public float CastTime { get; set; }
		public float Cost { get; set; }

		public float AbilityRadius { get; set; }
		
		public bool CanCastOnSelf { get; set; }
		public bool RequiresTarget { get; set; }

		public AbilityType Type { get; set; }

		protected GameObject Target { get; set; }

		//private Stopwatch durationTimer = new Stopwatch(); For AoE
		//durationTimer.Start();
		//while (durationTimer.Elapsed.TotalSeconds <= effectDuration) { }
		//durationTimer.Stop();
		//durationTimer.Reset();
		//yield return null;

		public virtual IEnumerator Active() { yield return null; }

		public enum AbilityType
		{
			
		}
    }
}