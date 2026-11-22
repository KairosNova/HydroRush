using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class EnemyChase : MonoBehaviour
{
    [Header("Target")]
    [Tooltip("Leave empty to find Aqua automatically (tag 'Player').")]
    [SerializeField] private Transform target;

    [Header("Detection")]
    [SerializeField] private float detectionRange = 8f;
    [SerializeField] private float loseRange = 12f;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float stopDistance = 2f;

    [Header("Debug")]
    [SerializeField] private bool debugLog = true;

    private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");
    private static readonly int FacingUpHash = Animator.StringToHash("FacingUp");

    private Rigidbody2D body;
    private Animator animator;
    private EnemyHealth health;
    private bool isChasing;
    private Vector2 desiredVelocity;
    private float retargetTimer;

    private bool IsDead => health != null && health.IsDead;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        health = GetComponent<EnemyHealth>();
    }

    private void Start()
    {
        ResolveTarget();
    }

    private void ResolveTarget()
    {
        // A prefab asset dragged from the Project window is NOT a scene object: it never moves.
        if (target != null && !target.gameObject.scene.IsValid())
        {
            Debug.LogWarning($"{name}: Target apunta a un PREFAB, no a Aqua de la escena. Lo ignoro.", this);
            target = null;
        }

        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
        }

        if (debugLog && target != null)
        {
            // Click this message in the Console: the object it points to gets highlighted in the Hierarchy.
            Debug.Log($"{name} persigue a '{target.name}' en {target.position}", target);
        }
    }

    private void Update()
    {
        if (target == null)
        {
            // Aqua may not exist yet (or just respawned): retry twice per second
            retargetTimer -= Time.deltaTime;
            if (retargetTimer <= 0f)
            {
                retargetTimer = 0.5f;
                ResolveTarget();
            }
        }

        if (IsDead || target == null)
        {
            isChasing = false;
            desiredVelocity = Vector2.zero;
            animator.SetBool(IsMovingHash, false);
            return;
        }

        Vector2 toTarget = (Vector2)target.position - (Vector2)transform.position;
        float distance = toTarget.magnitude;

        if (!isChasing && distance <= detectionRange)
        {
            isChasing = true;
        }
        else if (isChasing && distance > loseRange)
        {
            isChasing = false;
        }

        bool shouldMove = isChasing && distance > stopDistance;
        desiredVelocity = shouldMove ? toTarget.normalized * moveSpeed : Vector2.zero;

        animator.SetBool(IsMovingHash, shouldMove);
        animator.SetBool(FacingUpHash, target.position.y > transform.position.y);
    }

    private void FixedUpdate()
    {
        if (IsDead)
        {
            return;
        }

        body.linearVelocity = desiredVelocity;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, loseRange);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }
}