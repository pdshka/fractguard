using System.Collections;
using UnityEngine;

[RequireComponent(typeof(HealthEvent))]
[DisallowMultipleComponent]
public class Health : MonoBehaviour
{
    #region Header References
    [Space(10)]
    [Header("References")]
    #endregion
    #region Tooltip
    [Tooltip("Populate with the HealthBar component on the HealthBar gameobject")]
    #endregion
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private int initialHealth;
    public int currentHealth;
    [HideInInspector] public HealthEvent healthEvent;
    private Coroutine effectCoroutine;
    protected bool hasHitEffect = false;
    protected float effectTime = 0f;
    protected SpriteRenderer spriteRenderer = null;
    private const float spriteFlashInterval = 0.33f;
    private WaitForSeconds waitForSecondsSpriteFlashInterval = new WaitForSeconds(spriteFlashInterval);

    [HideInInspector] public bool isDamageable = true;

    private void Awake()
    {
        // Load components
        healthEvent = GetComponent<HealthEvent>();
    }

    protected virtual void Start()
    {
        currentHealth = initialHealth;
        
        // Trigger a health event for UI update
        CallHealthEvent(0);
    }

    /// <summary>
    /// Public method called when damage is taken
    /// </summary>
    public virtual void TakeDamage(int damageAmount)
    {

        if (isDamageable)
        {

            if (damageAmount == 0)
                return;

            currentHealth -= damageAmount;
            CallHealthEvent(damageAmount);

            // Set health bar as the percentage of health remaining
            if (healthBar != null)
            {
                healthBar.SetHealthBarValue((float)currentHealth / (float)initialHealth);
            }

            if (hasHitEffect)
                PostHitEffect();
        }

    }

    protected void CallHealthEvent(int damageAmount)
    {
        // Trigger health event
        healthEvent.CallHealthChangedEvent((float)currentHealth / (float)initialHealth, currentHealth, initialHealth, damageAmount);
    }

    /// <summary>
    /// Indicate a hit and give some post hit effect
    /// </summary>
    private void PostHitEffect()
    {
        // Check if gameobject is active - if not return
        if (gameObject.activeSelf == false)
            return;

        if (effectCoroutine != null)
            StopCoroutine(effectCoroutine);

        // flash red and give period of immunity
        effectCoroutine = StartCoroutine(PostHitEffectRoutine(effectTime, spriteRenderer));

    }

    /// <summary>
    /// Coroutine to indicate a hit and give some post hit effect
    /// </summary>
    private IEnumerator PostHitEffectRoutine(float effectTime, SpriteRenderer spriteRenderer)
    {
        int iterations = Mathf.RoundToInt(effectTime / spriteFlashInterval / 2f);

        while (iterations > 0)
        {
            spriteRenderer.color = Color.red;

            yield return waitForSecondsSpriteFlashInterval;

            spriteRenderer.color = Color.white;

            yield return waitForSecondsSpriteFlashInterval;

            iterations--;

            yield return null;

        }

    }

    /// <summary>
    /// Increase health by specified percent
    /// </summary>
    public void AddHealth(float healthPercent)
    {
        int healthIncrease = Mathf.RoundToInt((initialHealth * healthPercent) / 100f);

        int totalHealth = currentHealth + healthIncrease;

        if (totalHealth > initialHealth)
        {
            currentHealth = initialHealth;
        }
        else
        {
            currentHealth = totalHealth;
        }

        CallHealthEvent(0);
    }

    /// <summary>
    /// Increase health by specified value
    /// </summary>
    public void AddHealth(int healthBoost)
    {
        int totalHealth = currentHealth + healthBoost;

        if (totalHealth > initialHealth)
        {
            currentHealth = initialHealth;
        }
        else
        {
            currentHealth = totalHealth;
        }

        CallHealthEvent(0);
    }

}
