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
    [Tooltip("Seconds between dying and disappearing. The Die animation lasts about 0.63 s.")]
    [SerializeField] private float disappearDelay = 1f;

    private bool isInvulnerable;
    private float invulnerableUntil;

    /// <summary>Raised every time the health changes. The value is 0 - 1.</summary>
    public static event Action<float> onHealthPercentageChanged;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public bool IsDead => currentHealth <= 0f;
    public float HealthPercent => currentHealth / maxHealth; // logic to detect the percentage of the life for the UI

    /// <summary>True while Aqua cannot be hurt: during a dash, or in the short pause after a hit.</summary>
    // NOTE: Projectile reads this, so enemy bullets fly through Aqua instead of disappearing while she is invulnerable.
    public bool IsInvulnerable
    {
        get { return isInvulnerable || Time.time < invulnerableUntil; }
    }

    private void Awake()
    {
        currentHealth = maxHealth;
        //verifies the health of the player in order that every time that the game starts the current health is the maximun health
        
    }

    private void Start()
    {
        // Send the starting value once, after every UI script had time to subscribe in OnEnable
        onHealthPercentageChanged?.Invoke(HealthPercent);
    }

    //Turns invulnerability on or off by hand (for example during a dash).</summary>
    public void SetInvulnerable(bool invulnerable)
    {
        isInvulnerable = invulnerable;
    }

    /// <summary>Restores health (used by the Purifying Wave). It never goes above the maximum.</summary>
    public void Heal(float amount)
    {
        if (IsDead)
        {
            return;   // a dead Aqua cannot be healed
        }

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        onHealthPercentageChanged?.Invoke(HealthPercent);   // update the life bar
    }

    public virtual void TakeDamage(int damage)
    {
        if (IsDead || isInvulnerable || Time.time < invulnerableUntil)//detect if it is invulerable or if its dead
        {
            return;
        }

        currentHealth = Mathf.Max(currentHealth - damage, 0f);//verifies the current health
        invulnerableUntil = Time.time + invulnerabilityTime;
        Debug.Log(gameObject.name + " took " + damage + " damage. HP: " + currentHealth + "/" + maxHealth);

        onHealthPercentageChanged?.Invoke(HealthPercent);   // update the life bar

        if (IsDead)
        {
            Death();
        }
        else
        {
            onDamage?.Invoke(); 
        }
        
    }

    

    protected virtual void Death()
    {
        //logic for the dead of the player
        Debug.Log(gameObject.name + " died");
        onDeath?.Invoke();                       // plays the Die animation (PlayerController.PlayDeathAnimation)
        Destroy(gameObject, disappearDelay);     // disappears once the animation has finished

    }

    
}