using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
[DisallowMultipleComponent]
public class EnemyMovementAI : MonoBehaviour
{
    public float moveSpeed = 8f;
    private Enemy enemy;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    private void Update()
    {
        MoveEnemy();
    }

    /// <summary>
    /// Handle enemy movement, while enemy is alive
    /// </summary>
    private void MoveEnemy()
    {
        // Бежим в сторону главной башни

        /*Vector2 unitVector = Vector3.Normalize(movePosition - transform.position);

        enemy.rigidBody2D.MovePosition(enemy.rigidBody2D.position + (unitVector * moveSpeed * Time.fixedDeltaTime));

        // передаем инфу аниматору и другим ... */

    }
}
