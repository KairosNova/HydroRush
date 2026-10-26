using System.Collections;
using UnityEngine;

/// <summary>
/// Simple slime attack: every few seconds the slime plays its attack animation and spits a spiral of toxic bubbles.
/// It also turns to face Aqua (Up or Down version of its animations).
/// Add it to any slime that should attack.
/// </summary>
[RequireComponent(typeof(Animator))]
public class SlimeAttacker : MonoBehaviour
{
    [Header("Target")]
    [Tooltip("Leave empty to find Aqua automatically.")]
    [SerializeField] private Transform target;
    [SerializeField] private float attackRange = 10f;

    [Header("Attack pattern")]
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private float timeBetweenBursts = 2.5f;
    [SerializeField] private int bulletsPerBurst = 12;
    [SerializeField] private float timeBetweenBullets = 0.07f;
    [Tooltip("How many degrees the spiral turns after every bullet.")]
    [SerializeField] private float spiralStepDegrees = 25f;
    [Tooltip("Where the bubbles leave the slime, relative to its pivot (at its feet).")]
    [SerializeField] private Vector2 muzzleOffset = new Vector2(0f, 0.35f);
    [Tooltip("Delay between the start of the attack animation and the first bubble.")]
    [SerializeField] private float windUpTime = 0.35f;

    private static readonly int AttackHash = Animator.StringToHash("Attack");
    private static readonly int FacingUpHash = Animator.StringToHash("FacingUp");

    private Animator animator;
    private float spiralAngleDegrees;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (target == null)
        {
            PlayerController player = FindFirstObjectByType<PlayerController>();
            if (player != null)
            {
                target = player.transform;
            }
        }

        StartCoroutine(AttackLoop());
    }

    private void Update()
    {
        // The slime shows its back when Aqua is above it and its front when she is below
        if (target != null)
        {
            animator.SetBool(FacingUpHash, target.position.y > transform.position.y);
        }
    }

    /// <summary>Waits, attacks when Aqua is in range, and repeats forever.</summary>
    private IEnumerator AttackLoop()
    {
        // Random start delay so several slimes do not fire in perfect sync
        yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));

        while (true)
        {
            if (target != null && Vector2.Distance(transform.position, target.position) <= attackRange)
            {
                animator.SetTrigger(AttackHash);
                yield return new WaitForSeconds(windUpTime);

                for (int i = 0; i < bulletsPerBurst; i++)
                {
                    FireBullet();
                    yield return new WaitForSeconds(timeBetweenBullets);
                }
            }

            yield return new WaitForSeconds(timeBetweenBursts);
        }
    }

    private void FireBullet()
    {
        if (projectilePrefab == null)
        {
            return;
        }

        float radians = spiralAngleDegrees * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
        spiralAngleDegrees += spiralStepDegrees;

        Vector2 spawnPosition = (Vector2)transform.position + muzzleOffset;
        Projectile bubble = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
        bubble.Launch(direction);
    }
}
