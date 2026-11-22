using UnityEngine;

/// <summary>
/// A straight-flying shot, used both by Aqua (water drops) and by enemies (toxic bubbles).
/// It disappears when its lifetime ends or when it touches a wall or a target.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [Tooltip("Default speed. A shooter can override it when it calls Launch (see SlimeAttacker).")]
    [SerializeField] private float speed = 12f;
    [SerializeField] private float lifetime = 3f;
    [Tooltip("Default damage to Aqua. A shooter can override it when it calls Launch (see SlimeAttacker).")]
    [SerializeField] private int damage = 10;

    [Tooltip("If true, this shot damages Aqua. If false, it damages enemies (these are Aqua's own shots).")]
    [SerializeField] private bool hurtsPlayer;

    [Tooltip("Rotate the sprite so its top points where the shot is travelling.")]
    [SerializeField] private bool rotateToDirection = true;

    private Rigidbody2D body;

    // NOTE: read by other scripts (graze, purifying wave) to tell enemy shots from Aqua's shots.
    public bool HurtsPlayer
    {
        get { return hurtsPlayer; }
    }

    // NOTE: set by GrazeZone the first time this bullet passes close to Aqua, so each bullet only gives charge once.
    public bool HasGrazed { get; set; }

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Safety net: shots never live forever, even if they never hit anything
        Destroy(gameObject, lifetime);
    }

    /// <summary>Launches the shot with the prefab's default speed and damage.</summary>
    public void Launch(Vector2 direction)
    {
        Launch(direction, speed, damage);
    }

    /// <summary>
    /// Launches the shot with the shooter's own speed and damage, so one prefab can serve several enemies.
    /// Call it right after Instantiate.
    /// </summary>
    public void Launch(Vector2 direction, float launchSpeed, int launchDamage)
    {
        speed = launchSpeed;
        damage = launchDamage;

        Vector2 normalized = direction.normalized;
        body.linearVelocity = normalized * speed;

        if (rotateToDirection)
        {
            transform.up = normalized;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Walls (the Walls tilemap collider) stop every shot
        if (other.GetComponent<UnityEngine.Tilemaps.TilemapCollider2D>() != null)
        {
            Destroy(gameObject);
            return;
        }

        if (hurtsPlayer)
        {
            // Enemy shots damage Aqua. While she is invulnerable (dash) they fly THROUGH her and keep going.
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null && !health.IsInvulnerable)
            {
                health.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
        else
        {
            // Aqua's shots damage enemies.
            // NOTE: the collider that is hit must be on the same object as the EnemyHealth component (the slime root).
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}
