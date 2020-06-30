namespace Novemo.StatusEffects.Debuffs
{
	public class Stun : StatusEffect
	{
		public override void ApplyEffect()
		{
			base.ApplyEffect();

			TargetStats.Stunned = true;
			(TargetStats.CanAttack, TargetStats.CanMove, TargetStats.CanUseSpells) = (false, false, false);
		}

		public override void RemoveEffect() 
		{
			(TargetStats.CanAttack, TargetStats.CanMove, TargetStats.CanUseSpells) = (true, true, true);
			TargetStats.Stunned = false;

			base.RemoveEffect();
		}
	}
}