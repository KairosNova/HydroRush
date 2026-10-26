using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controls Aqua: 8-direction movement with a Rigidbody2D, mouse aiming and held-button shooting.
/// Input comes from the project-wide actions of the new Input System
/// (Assets/InputSystem_Actions.inputactions): Player/Move (WASD or arrows) and Player/Attack (left mouse button).
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;

    [Header("Shooting")]
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private float shotsPerSecond = 6f;
    [Tooltip("Distance from Aqua's center where the shots appear.")]
    [SerializeField] private float muzzleDistance = 0.6f;
    [Tooltip("Optional sound played on every shot (played through the AudioManager).")]
    [SerializeField] private AudioClip shootSound;

    private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");
    private static readonly int HitHash = Animator.StringToHash("Hit");

    private Rigidbody2D body;
    private Animator animator;
    private Camera mainCamera;
    private InputAction moveAction;
    private InputAction attackAction;

    private Vector2 moveInput;
    private Vector2 aimDirection = Vector2.up;
    private float nextShotTime;

    /// <summary>Unit vector from Aqua towards the mouse pointer.</summary>
    public Vector2 AimDirection
    {
        get { return aimDirection; }
    }

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        // Look the actions up by "Map/Action" name so they can be rebound later without touching this code
        moveAction = InputSystem.actions.FindAction("Player/Move");
        attackAction = InputSystem.actions.FindAction("Player/Attack");

        if (moveAction == null || attackAction == null)
        {
            Debug.LogError("PlayerController: Player/Move or Player/Attack was not found in the project-wide input actions.");
            enabled = false;
        }
    }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // Read input every frame; apply it to the physics body in FixedUpdate
        moveInput = Vector2.ClampMagnitude(moveAction.ReadValue<Vector2>(), 1f);

        UpdateAimDirection();

        if (attackAction.IsPressed() && Time.time >= nextShotTime)
        {
            Shoot();
        }

        if (animator != null)
        {
            animator.SetBool(IsMovingHash, moveInput.sqrMagnitude > 0.01f);
        }
    }

    private void FixedUpdate()
    {
        body.linearVelocity = moveInput * moveSpeed;
    }

    /// <summary>Called by enemy shots. For now it only plays the hit animation; health comes in Stage 4.</summary>
    public void TakeHit()
    {
        if (animator != null)
        {
            animator.SetTrigger(HitHash);
        }
    }

    /// <summary>Converts the mouse position to world space and stores the direction from Aqua to it.</summary>
    private void UpdateAimDirection()
    {
        if (Mouse.current == null || mainCamera == null)
        {
            return;
        }

        Vector2 screenPosition = Mouse.current.position.ReadValue();
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(
            new Vector3(screenPosition.x, screenPosition.y, -mainCamera.transform.position.z));

        Vector2 toMouse = (Vector2)worldPosition - (Vector2)transform.position;
        if (toMouse.sqrMagnitude > 0.0001f)
        {
            aimDirection = toMouse.normalized;
        }
    }

    private void Shoot()
    {
        if (projectilePrefab == null)
        {
            return;
        }

        nextShotTime = Time.time + 1f / shotsPerSecond;

        Vector2 spawnPosition = (Vector2)transform.position + aimDirection * muzzleDistance;
        Projectile shot = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
        shot.Launch(aimDirection);

        if (shootSound != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySfx(shootSound);
        }
    }
}
