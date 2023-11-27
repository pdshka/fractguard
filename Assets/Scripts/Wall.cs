using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [HideInInspector] public HealthEvent healthEvent;
    private Health health;
    public GameObject tower;
    public Sprite wallFixed;
    public Sprite wallDestroyed;

    private SpriteRenderer spriteRenderer;
    public Collider2D wallCollider;
    public bool destroyed = false;

    private AudioSource audioSource;
    [SerializeField] private AudioClip destroy;

    private void Awake()
    {
        healthEvent = GetComponent<HealthEvent>();
        health = GetComponent<Health>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        wallCollider = GetComponent<Collider2D>();
        audioSource = GameObject.Find("Sounds").GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        //subscribe to health event
        healthEvent.OnHealthChanged += HealthEvent_OnHealthLost;
    }

    private void OnDisable()
    {
        // unsubscribe from health event
        healthEvent.OnHealthChanged -= HealthEvent_OnHealthLost;
    }

    private void HealthEvent_OnHealthLost(HealthEvent healthEvent, HealthEventArgs healthEventArgs)
    {
        if (healthEventArgs.healthAmount <= 0)
        {
            WallDestroyed();
        }
    }

    private void WallDestroyed()
    {

        if (tower != null)
        {
            GameObject.Destroy(tower);
            audioSource.PlayOneShot(destroy, 0.3f);
        }
        spriteRenderer.sprite = wallDestroyed;
        wallCollider.isTrigger = true;
        destroyed = true;
    }

    public void AttachTower(GameObject tower)
    {
        this.tower = tower;
        spriteRenderer.sprite = null;
    }

    public void Fix()
    {
        destroyed = false;
        spriteRenderer.sprite = wallFixed;
        health.ResetHealth();
    }
}
