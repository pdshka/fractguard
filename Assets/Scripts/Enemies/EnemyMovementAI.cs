using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(Enemy))]
[DisallowMultipleComponent]
public class EnemyMovementAI : MonoBehaviour
{
    public LayerMask targetsLayerMask;
    public float moveSpeed = 8f;
    private Enemy enemy;
    [SerializeField] private float enemySightRadius = 5f;
    [SerializeField] private float enemySightAngle;
    private readonly Vector3 initialTargetPosition = Vector3.zero;
    private Vector3 previousTargetPosition;
    [HideInInspector] public Vector3 targetPosition;
    [HideInInspector] public bool targetReached = false;
    private const float pathRebuildCooldown = 1f;
    private const int targetFrameRateToSpreadPathfindingOver = 60;
    private float pathRebuildCooldownTimer;
    //private bool pathRebuildNeeded = false;
    private int updateFrameNumber = 1;
    private PolygonCollider2D enemySightCollider;
    private AudioSource audioSource;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        enemySightCollider = GetComponentInChildren<PolygonCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        targetPosition = initialTargetPosition;
        updateFrameNumber = EnemySpawner.Instance.enemiesSpawnedSoFar % targetFrameRateToSpreadPathfindingOver;
    }

    private void Update()
    {
        //enemySightCollider.transform.right = initialTargetPosition - transform.position;
        if (pathRebuildCooldownTimer > 0f)
        {
            pathRebuildCooldownTimer -= Time.deltaTime;
        }
        MoveEnemy(targetPosition);
    }

    /// <summary>
    /// Handle enemy movement, while enemy is alive
    /// </summary>
    private void MoveEnemy(Vector3 movePosition)
    {
        // Бежим в сторону главной башни

        // Если видим заборчик, то бежим к заборчику
        if (pathRebuildCooldownTimer <= 0f && Time.frameCount % targetFrameRateToSpreadPathfindingOver == updateFrameNumber)
        {
            CheckForNewTarget();
            pathRebuildCooldownTimer = pathRebuildCooldown;
        }

        if (Vector3.Distance(transform.position, movePosition) < 0.2f)
        {
            // Idle();
            targetReached = true;
            enemy.animator.SetBool("isMoving", false);
            audioSource.Stop();
            return;
        }
        targetReached = false;

        Vector3 moveDirection = (movePosition - transform.position).normalized;
        Vector2 unitVector = Vector3.Normalize(movePosition - transform.position);

        enemy.rigidBody2D.MovePosition(enemy.rigidBody2D.position + (unitVector * moveSpeed * Time.fixedDeltaTime));

        enemy.animator.SetBool("isMoving", true);
        if (!audioSource.isPlaying)
            audioSource.Play();

        // передаем инфу аниматору и другим ... 
        InitializeLookAnimationParameters();
        
        float moveAngle = HelperUtilities.GetAngleFromVector(moveDirection);
        LookDirection lookDirection = HelperUtilities.GetLookDirection(moveAngle);
        SetLookAnimationParameters(lookDirection);
    }

    private void CheckForNewTarget()
    {
        Collider2D[] objectsInLineOfSight = Physics2D.OverlapCircleAll(transform.position, enemySightRadius, targetsLayerMask);

        List<Collider2D> filteredObjects = new List<Collider2D>();

        foreach (Collider2D obj in objectsInLineOfSight)
        {
            Vector3 directionToTarget = (obj.transform.position - transform.position).normalized;
            float angle = Vector2.Angle(-transform.position, directionToTarget);
            if ((angle <= enemySightAngle / 2 || 360f - angle <= enemySightAngle / 2))
            {
                RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, obj.transform.position - transform.position, enemySightRadius, targetsLayerMask);
                
                if (!obj.isTrigger && hits.FirstOrDefault(hit => !hit.collider.isTrigger).collider == obj 
                    || hits.TakeWhile(hit => hit.collider.isTrigger).Any(hit => hit.collider == obj))
                {
                    filteredObjects.Add(obj);
                }
            }
        }
        Collider2D closestCollider = null;
        if (filteredObjects.Count > 0)
        {
            
            float minDistance = float.MaxValue;
            foreach (Collider2D obj in filteredObjects)
            {
                // пропускаем текущий коллайдер
                if (obj.OverlapPoint(transform.position))
                {
                    continue;
                }
                // Главное здание
                if (obj.gameObject.tag == "MainBuilding")
                {
                    closestCollider = obj;
                    break;
                }
                
                // Приоритет для пробитых стен
                float destroyedPriorityContribution = 0f;
                if (obj.isTrigger)
                {
                    destroyedPriorityContribution = -6f;
                }
                
                float currentDistance = Vector3.Distance(obj.transform.position, transform.position) 
                    + Vector3.Magnitude(obj.transform.position) + destroyedPriorityContribution; // 2-е - расстояние до центра карты от стены
                if (currentDistance < minDistance)
                {
                    minDistance = currentDistance;
                    closestCollider = obj;
                }
            }
        }
        if (closestCollider == null)
        {
            targetPosition = initialTargetPosition;
        }
        else
        {
            targetPosition = closestCollider.isTrigger ? closestCollider.transform.position : closestCollider.ClosestPoint(transform.position);
        }
    }

    /// <summary>
    /// Initialise look animation parameters
    /// </summary>
    private void InitializeLookAnimationParameters()
    {
        enemy.animator.SetBool("lookUp", false);
        enemy.animator.SetBool("lookRight", false);
        enemy.animator.SetBool("lookLeft", false);
        enemy.animator.SetBool("lookDown", false);
    }

    /// <summary>
    /// Set look animation parameters
    /// </summary>
    private void SetLookAnimationParameters(LookDirection lookDirection)
    {
        // Set look direction
        switch (lookDirection)
        {
            case LookDirection.Up:
                enemy.animator.SetBool("lookUp", true);
                break;

            case LookDirection.Right:
                enemy.animator.SetBool("lookRight", true);
                break;

            case LookDirection.Left:
                enemy.animator.SetBool("lookLeft", true);
                break;

            case LookDirection.Down:
                enemy.animator.SetBool("lookDown", true);
                break;

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetPosition, 0.1f);
        Gizmos.DrawWireSphere(transform.position, enemySightRadius);
        // Gizmos.DrawLine(HelperUtilities.GetDirectionVectorFromAngle(enemySightAngle / 2) + transform.position, transform.position);
    }

    private void Idle()
    {
        // сигнал аниматору
    }
}
