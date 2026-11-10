using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Holds the player's health. Other scripts call TakeDamage; the UI listens to onHealthPercentageChanged.
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private UnityEvent onDeath;
    [SerializeField] private UnityEvent onDamage;

    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    [Tooltip("Seconds without taking damage after a hit, so one burst of bullets does not drain the bar at once.")]
    [SerializeField] private float invulnerabilityTime = 0.5f;

    private bool isInvulnerable;
    private float invulnerableUntil;

    /// <summary>Raised every time the health changes. The value is 0 - 1.</summary>
    public static event Action<float> onHealthPercentageChanged;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public bool IsDead => currentHealth <= 0f;
    public float HealthPercent => currentHealth / maxHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Start()
    {
        // Send the starting value once, after every UI script had time to subscribe in OnEnable
        onHealthPercentageChanged?.Invoke(HealthPercent);
    }

    /// <summary>Turns invulnerability on or off by hand (for example during a dash).</summary>
    public void SetInvulnerable(bool invulnerable)
    {
        isInvulnerable = invulnerable;
    }

    public virtual void TakeDamage(int damage)
    {
        if (IsDead || isInvulnerable || Time.time < invulnerableUntil)
        {
            return;
        }

        currentHealth = Mathf.Max(currentHealth - damage, 0f);
        invulnerableUntil = Time.time + invulnerabilityTime;
        Debug.Log(gameObject.name + " took " + damage + " damage. HP: " + currentHealth + "/" + maxHealth);

        onHealthPercentageChanged?.Invoke(HealthPercent);   // update the life bar
        onDamage?.Invoke();                                 // hit animation, sound, etc.

        if (IsDead)
        {
            Death();
        }
    }

    protected virtual void Death()
    {
        Debug.Log(gameObject.name + " died");
        onDeath?.Invoke();
        Destroy(gameObject);
    }

    
}