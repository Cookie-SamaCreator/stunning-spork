using System.Collections;
using UnityEngine;
using RPG.Combat;

public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float dashSpeed = 5f;
    [SerializeField] private float dashDuration = 0.2f; // Duration of the dash effect
    [SerializeField] private float dashCooldown = 0.25f; // Cooldown time for dashing
    [SerializeField] private TrailRenderer trailRenderer; // Optional: Trail renderer for dash effect
    [SerializeField] private Transform weaponCollider;

    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Knockback knockback;
    private DamageController damageController;
    private float startingMoveSpeed;

    private bool facingLeft = false;
    public bool FacingLeft {get { return facingLeft; }}

    private bool isDashing = false;
    protected override void Awake()
    {
        base.Awake();
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        knockback = GetComponent<Knockback>();
        damageController = GetComponent<DamageController>();
    }

    private void Start()
    {
        playerControls.Combat.Dash.performed += _ => Dash();
        startingMoveSpeed = moveSpeed; // Store the initial move speed

        //ActiveInventory.Instance.EquipStartingWeapon();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        if (movement != Vector2.zero)
        {
            MovePlayer();
        }
        AdjustPlayerFacingDirection();
    }

    public Transform GetWeaponCollider()
    {
        return weaponCollider;
    }
    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();

        animator.SetFloat("moveX", movement.x);
        animator.SetFloat("moveY", movement.y);
    }

    private void MovePlayer()
    {
        if(knockback.GettingKnockedBack || damageController.IsDead){ return; }
        Vector2 moveDirection = movement * (moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(rb.position + moveDirection);
    }

    private void AdjustPlayerFacingDirection()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if (mousePos.x < playerScreenPoint.x)
        {
            //Flip player sprite to face left
            spriteRenderer.flipX = true;
            facingLeft = true;
        }
        else
        {
            //Flip player sprite to face right
            spriteRenderer.flipX = false;
            facingLeft = false;
        }
    }

    private void Dash()
    {
        if (!isDashing && Stamina.Instance.UseStamina())
        {
            isDashing = true;
            moveSpeed *= dashSpeed;
            trailRenderer.emitting = true; // Start emitting trail effect
            StartCoroutine(EndDashRoutine());
        }
    }

    private IEnumerator EndDashRoutine()
    {
        yield return new WaitForSeconds(dashDuration);
        moveSpeed = startingMoveSpeed; // Reset move speed after dash duration
        trailRenderer.emitting = false; // Stop emitting trail effect
        yield return new WaitForSeconds(dashCooldown); // Wait for cooldown before allowing another dash
        isDashing = false;
    }
}
