using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
[DisallowMultipleComponent]
public class EnemyMovementAI : MonoBehaviour
{
    public float moveSpeed = 8f;
    private Enemy enemy;
    private readonly Vector3 initialTargetPosition = Vector3.zero;
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
        // Бежим в сторону главной башни

        if (Vector3.Distance(transform.position, movePosition) < 0.2f)
        {
            // Idle();
            return;
        }

        Vector2 unitVector = Vector3.Normalize(movePosition - transform.position);

        enemy.rigidBody2D.MovePosition(enemy.rigidBody2D.position + (unitVector * moveSpeed * Time.fixedDeltaTime));

        // передаем инфу аниматору и другим ... 

    }

    private void Idle()
    {
        // сигнал аниматору
    }
}
