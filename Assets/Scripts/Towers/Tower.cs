using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class Tower : MonoBehaviour
{
    [Header("Cost settings")]
    public int money;
    public int stone;
    public int wood;
    [Header("Tower settings")]
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private float cooldown;
    [SerializeField] private GameObject target;
    [SerializeField] Transform shootPosition;
    private float timer;
    private float offset;

    private Queue<GameObject> enemyQueue;

    private AudioSource audioSource;

    [Header("Sound settings")]
    [SerializeField] private AudioClip shoot;
    [SerializeField] private float volume;

    private void Start()
    {
        timer = 0f;
        target = null;
        enemyQueue = new Queue<GameObject>();
        audioSource = GameObject.Find("Sounds").GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (timer > 0f)
            timer -= Time.deltaTime;
        if (enemyQueue.Count > 0 || target != null)
            Shoot();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
            enemyQueue.Enqueue(collision.gameObject);
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
        Aim();
        if (timer <= 0f)
        {
            Instantiate(bulletPrefab, shootPosition.position, transform.rotation);
            audioSource.PlayOneShot(shoot, volume);
            timer = cooldown;
        }
    }
}
