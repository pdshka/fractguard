using UnityEngine;

public class EnemyAttackAI : MonoBehaviour
{
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius;
    [SerializeField] private int enemyDamage;
    private Enemy enemy;
    [SerializeField] private float attackCooldown = 1f;
    private float attackCooldownTimer = 0f;

    private AudioSource audioSource;
    [SerializeField] private AudioClip bite;
    [SerializeField] private float volume;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        audioSource = GameObject.Find("Sounds").GetComponent<AudioSource>();
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
                Attack();
                attackCooldownTimer = attackCooldown;
            }
        }
    }

    private void Attack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius, enemy.enemyMovementAI.targetsLayerMask);

        enemy.animator.SetTrigger("attackTrigger");

        foreach (Collider2D hit in hits)
        {
            Health health = hit.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(enemyDamage);
                audioSource.PlayOneShot(bite, volume);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }
}
