using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public abstract class Tower : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private float cooldown;
    private Enemy target;
    private float timer;
    private float offset;

    private Queue<Enemy> enemyQueue;

    private void Start()
    {
        timer = 0f;
        target = null;
        enemyQueue = new Queue<Enemy>();
    }

    private void Update()
    {
        if (timer > 0f)
            timer -= Time.deltaTime;
        if (enemyQueue.Count > 0)
            Shoot();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
            enemyQueue.Enqueue(collision.gameObject.GetComponent<Enemy>());
    }

    private void Aim()
    {
        float distX = target.transform.position.x - transform.position.x;
        float distY = target.transform.position.y - transform.position.y;
        transform.rotation = new Quaternion(0, 0, 0, Mathf.Atan2(distY, distX) * Mathf.Rad2Deg); // Проверить, напистаь без оффсета
    }

    private void Shoot()
    {
        Aim();
        if (target = null)
            target = enemyQueue.Dequeue();
        if (timer == 0f)
        {
            Quaternion rotation = transform.rotation;
            Instantiate(bulletPrefab, transform.position, rotation);
            timer = cooldown;
        }
    }
}
