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
    public float moveSpeed = 8f;
    private Enemy enemy;
    [SerializeField] private float enemySightRadius = 5f;
    private readonly Vector3 initialTargetPosition = Vector3.zero;
    public Vector3 targetPosition;
    public bool targetReached = false;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    private void Update()
    {
        MoveEnemy(initialTargetPosition);
    }

    /// <summary>
    /// Handle enemy movement, while enemy is alive
    /// </summary>
    private void MoveEnemy(Vector3 movePosition)
    {
        // ����� � ������� ������� �����

        // ���� ����� ��������, �� ����� � ���������
        CheckForNewTarget();

        if (Vector3.Distance(transform.position, movePosition) < 0.2f)
        {
            // Idle();
            targetReached = true;
            return;
        }
        targetReached = false;

        Vector3 moveDirection = (movePosition - transform.position).normalized;
        Vector2 unitVector = Vector3.Normalize(movePosition - transform.position);

        enemy.rigidBody2D.MovePosition(enemy.rigidBody2D.position + (unitVector * moveSpeed * Time.fixedDeltaTime));

        // �������� ���� ��������� � ������ ... 
        InitializeLookAnimationParameters();
        
        float moveAngle = HelperUtilities.GetAngleFromVector(moveDirection);
        LookDirection lookDirection = HelperUtilities.GetLookDirection(moveAngle);
        SetLookAnimationParameters(lookDirection);
    }

    private void CheckForNewTarget()
    {
        Collider2D[] objectsInLineOfSight = Physics2D.OverlapCircleAll(transform.position, enemySightRadius, LayerMask.NameToLayer("Buildings"));

        if (objectsInLineOfSight.Length > 0)
        {
            Collider2D closestCollider = null;
            float minDistance = float.MaxValue;
            foreach (Collider2D obj in objectsInLineOfSight)
            {
                float currentDistance = Vector3.Distance(obj.transform.position, transform.position);
                if (currentDistance < minDistance)
                {
                    minDistance = currentDistance;
                    closestCollider = obj;
                }
            }
            targetPosition = closestCollider.transform.position;
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

    private void Idle()
    {
        // ������ ���������
    }
}
