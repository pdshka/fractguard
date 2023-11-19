using UnityEngine;

public class EnemyAttackAI : MonoBehaviour
{
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius;
    [SerializeField] private int enemyDamage;
    private Enemy enemy;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    private void Attack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

        foreach (Collider2D hit in hits)
        {
            Health health = hit.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(enemyDamage);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }
}
