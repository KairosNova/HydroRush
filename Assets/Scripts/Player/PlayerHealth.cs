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
    public float HealthPercent => currentHealth / maxHealth; // logic to detect the percentage of the life for the UI

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
        onDamage?.Invoke();                                 // hit animation, sound, etc.

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
        onDeath?.Invoke();

    }

    
}