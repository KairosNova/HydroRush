using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Health for any enemy. Aqua's shots call TakeDamage; at zero health the enemy plays its death
/// animation, stops being simulated and disappears.
/// Needs the animator parameters "Hit" (trigger) and "IsDead" (bool).
/// </summary>
[RequireComponent(typeof(Animator))]
public class EnemyHealth : MonoBehaviour
{
    // set these per enemy in the Inspector, not in code.
    [Header("Health (set these per enemy)")]
    [Tooltip("Hit points. With Aqua's shot doing 10 damage, 30 means three hits.")]
    [SerializeField] private int maxHealth = 30;
    [Tooltip("Seconds between dying and disappearing. The slime death animation lasts about 0.63 s.")]
    [SerializeField] private float disappearDelay = 0.8f;

    [Header("Events (optional)")]
    [SerializeField] private UnityEvent onDamage;
    [SerializeField] private UnityEvent onDeath;

    private static readonly int HitHash = Animator.StringToHash("Hit");
    private static readonly int IsDeadHash = Animator.StringToHash("IsDead");

    private Animator animator;
    private int currentHealth;

    public int CurrentHealth => currentHealth;
    public bool IsDead => currentHealth <= 0;

    private void Awake()
    {
        // get the components to been use each time the game is awaken
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (IsDead)
        {
            return;
        }

        currentHealth = Mathf.Max(currentHealth - damage, 0);

        if (IsDead)
        {
            Die();
        }
        else
        {
            animator.SetTrigger(HitHash);   // short hit reaction
            onDamage?.Invoke();
        }
    }

    private void Die()
    {
        animator.SetBool(IsDeadHash, true);   // Any State -> Die_Down / Die_Up

        // Stop the physics body: a dead enemy no longer moves, blocks Aqua or catches shots
        Rigidbody2D body = GetComponent<Rigidbody2D>();
        if (body != null)
        {
            body.linearVelocity = Vector2.zero;
            body.simulated = false;
        }

        onDeath?.Invoke();
        Destroy(gameObject, disappearDelay);
    }
}