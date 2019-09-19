using Novemo.Stats;

namespace Novemo.Enemies
{
    public class EnemyStats : CharacterStats
    {
        public override void Die()
        {
            base.Die();
            
            // death animation
            // drop loot
            
            Destroy(gameObject);
        }
    }
}
