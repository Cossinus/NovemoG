using Novemo.Stats;

namespace Novemo.Enemies
{
    public class EnemyStats : CharacterStats
    {
        protected override void Die()
        {
            base.Die();
            
            // death animation
            // drop loot
            
            Destroy(gameObject);
        }
    }
}
