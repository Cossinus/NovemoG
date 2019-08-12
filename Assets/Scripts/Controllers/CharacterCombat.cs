using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
public class CharacterCombat : MonoBehaviour
{
    private CharacterStats myStats;

    private float attackCooldown = 0f;

    public float attackDelay = .6f;

    public event System.Action OnAttack;

    void Start()
    {
        myStats = GetComponent<CharacterStats>();
    }

    void Update()
    {
        attackCooldown -= Time.deltaTime;
    }
    
    public void Attack(CharacterStats targetStats)
    {
        if (attackCooldown <= 0f)
        {
            StartCoroutine(DoDamage(targetStats, attackDelay));

            OnAttack?.Invoke();

            attackCooldown = 1f / myStats.stats[5].GetValue();
        }
    }

    IEnumerator DoDamage(CharacterStats stats, float delay)
    {
        yield return new WaitForSeconds(delay);
        stats.TakeDamage(myStats.stats[2].GetValue(), myStats.stats[9].GetValue(), myStats.stats[10].GetValue(), myStats.stats[9].GetValue());
        // TODO Change myStats.stats[9].GetValue() with spell damage nad lethal spell damage
    }
}
