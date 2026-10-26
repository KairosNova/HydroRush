using UnityEngine;

/// <summary>
/// A straight-flying shot, used both by Aqua (water drops) and by enemies (toxic bubbles).
/// It disappears when its lifetime ends or when it touches a wall.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 12f;
    [SerializeField] private float lifetime = 3f;

    [Tooltip("If true, touching Aqua makes her play the hit reaction and the shot disappears.")]
    [SerializeField] private bool hurtsPlayer;

    [Tooltip("Rotate the sprite so its top points where the shot is travelling.")]
    [SerializeField] private bool rotateToDirection = true;

    private Rigidbody2D body;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Safety net: shots never live forever, even if they never hit anything
        Destroy(gameObject, lifetime);
    }

    /// <summary>Sets the flight direction and starts moving. Call it right after Instantiate.</summary>
    public void Launch(Vector2 direction)
    {
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

        // Enemy shots react with Aqua; her own shots ignore her
        if (hurtsPlayer)
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeHit();
                Destroy(gameObject);
            }
        }
    }
}
