using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class Tower : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private float cooldown;
    [SerializeField] private GameObject target;
    [SerializeField] Transform shootPosition;
    private float timer;
    private float offset;

    private Queue<GameObject> enemyQueue;

    private void Start()
    {
        timer = 0f;
        target = null;
        enemyQueue = new Queue<GameObject>();
    }

    private void Update()
    {
        if (timer > 0f)
            timer -= Time.deltaTime;
        if (target != null)
            Shoot();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (enemyQueue.Count == 0)
                target = collision.gameObject;
            else
                enemyQueue.Enqueue(collision.gameObject);
        }
    }

    private void Aim()
    {
        float distX = target.transform.position.x - transform.position.x;
        float distY = target.transform.position.y - transform.position.y;
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(distY, distX) * Mathf.Rad2Deg);
    }

    private void Shoot()
    {
        if (target == null && enemyQueue.Count > 0)
            target = enemyQueue.Dequeue();
        if (target != null) Aim();
        if (timer <= 0f)
        {
            Instantiate(bulletPrefab, shootPosition.position, transform.rotation);
            timer = cooldown;
        }
    }
}
