using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 direction;
    [SerializeField] private float velocity;
    [SerializeField] private float damage;

    void Start()
    {
        // TODO: расчет направления полета пули (нормированный ветор)
    }

    void Update()
    {
        // TODO: перемещение пули
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
            Damage(collision.gameObject.GetComponent<Enemy>());
    }

    virtual protected void Damage(Enemy target)
    {
        // TODO: Сделать функцию получения урона в Enemy и вызвать ее
    }
}
