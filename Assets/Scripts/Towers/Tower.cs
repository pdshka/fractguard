using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    [SerializeField] protected Bullet bulletPrefab;
    [SerializeField] protected float cooldown;
    protected Enemy target;
    protected float timer;

    Queue<Enemy> enemyQueue;

    protected void Start()
    {
        timer = 0f;
        target = null;
        enemyQueue = new Queue<Enemy>();
    }

    protected void Update()
    {
        if (enemyQueue.Count > 0)
            Shoot();
        if (timer > 0f)
            timer -= Time.deltaTime;
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
            enemyQueue.Enqueue(collision.gameObject.GetComponent<Enemy>());
    }

    protected void Shoot()
    {
        if (target = null)
            target = enemyQueue.Dequeue();
        if (timer == 0f)
        {
            Instantiate(bulletPrefab, transform);
            timer = cooldown;
        }
    }
}
