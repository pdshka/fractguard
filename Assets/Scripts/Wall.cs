using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] private int defence = 100;

    public void TakeDamage(int damage)
    {
        defence -= damage;
        if (defence < 0)
        {
            Break();
        }
    }

    public void Break()
    {
        GameObject.Destroy(gameObject);
    }
}
