using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 direction;
    [SerializeField] private float velocity;
    [SerializeField] private float damage;
    private float timer;

    private void Start()
    {
        timer = 10f;
    }

    private void Update()
    {
        transform.Translate(Vector2.right * velocity * Time.deltaTime);
        timer -= Time.deltaTime;
        if(timer <= 0)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("lalala");
        if (collision.gameObject.tag == "Enemy")
        {
            //Damage(collision.gameObject.GetComponent<Enemy>());
            Destroy(gameObject);
        }
    }

    virtual protected void Damage(Enemy target)
    {
        // TODO: Сделать функцию получения урона в Enemy и вызвать ее
    }
}
