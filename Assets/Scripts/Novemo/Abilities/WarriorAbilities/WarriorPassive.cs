namespace Novemo.Abilities.WarriorAbilities
{
	public class WarriorPassive : Ability
	{
		public override void Passive(Characters.Character characterStats)
		{
			characterStats.stats[0].wholeModifierValue += 0.03f;
			characterStats.stats[0].AddBaseValue(25);
			characterStats.ModifyHealth(25);
		}
	}
}
