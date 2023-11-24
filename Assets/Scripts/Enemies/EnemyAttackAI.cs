using UnityEngine;

public class EnemyAttackAI : MonoBehaviour
{
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius;
    [SerializeField] private int enemyDamage;
    private Enemy enemy;
    private const float attackCooldown = 1f;
    private float attackCooldownTimer = 0f;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    private void Update()
    {
        if (attackCooldownTimer > 0f)
        {
            attackCooldownTimer -= Time.deltaTime;
        }

        if (enemy.enemyMovementAI.targetReached)
        {
            if (attackCooldownTimer <= 0f)
            {
                // Debug.Log("Attack");
                Attack();
                attackCooldownTimer = attackCooldown;
            }
        }
    }

    private void Attack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius, LayerMask.NameToLayer("Buildings"));

        enemy.animator.SetTrigger("attackTrigger");

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
