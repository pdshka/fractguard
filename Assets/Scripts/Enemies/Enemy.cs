using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int awardForDefeating;
    [HideInInspector] public HealthEvent healthEvent;
    private Health health;
    [HideInInspector] public EnemyMovementAI enemyMovementAI;
    [HideInInspector] public Rigidbody2D rigidBody2D;
    private BoxCollider2D boxCollider2D;
    private PolygonCollider2D polygonCollider2D;
    [HideInInspector] public Animator animator;


    private void Awake()
    {
        healthEvent = GetComponent<HealthEvent>();
        health = GetComponent<Health>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        enemyMovementAI = GetComponent<EnemyMovementAI>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        polygonCollider2D = GetComponent<PolygonCollider2D>();
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Initialise the enemy
    /// </summary>
    public void EnemyInitialization(int enemySpawnNumber)
    {
        /*SetEnemyMovementUpdateFrame(enemySpawnNumber);


        SetEnemyStartingHealth();

        SetEnemyStartingWeapon();*/

        // SetEnemyAnimationSpeed();

        // Materialise enemy
        //StartCoroutine(MaterializeEnemy());
    }

    private void OnEnable()
    {
        //subscribe to health event
        healthEvent.OnHealthChanged += HealthEvent_OnHealthLost;
    }

    private void OnDisable()
    {
        // unsubscribe from health event
        healthEvent.OnHealthChanged -= HealthEvent_OnHealthLost;
    }

    /// <summary>
    /// Handle health lost event
    /// </summary>
    private void HealthEvent_OnHealthLost(HealthEvent healthEvent, HealthEventArgs healthEventArgs)
    {
        if (healthEventArgs.healthAmount <= 0)
        {
            EnemyDestroyed();
        }
    }

    /// <summary>
    /// Enemy destroyed
    /// </summary>
    private void EnemyDestroyed()
    {
        DestroyedEvent destroyedEvent = GetComponent<DestroyedEvent>();
        destroyedEvent.CallDestroyedEvent(awardForDefeating);

    }

    /// <summary>
    /// Set enemy animator speed to match movement speed
    /// </summary>
    private void SetEnemyAnimationSpeed()
    {
        // Set animator speed to match movement speed
        // animator.speed = enemyMovementAI.moveSpeed / Settings.baseSpeedForEnemyAnimations;
    }

}
