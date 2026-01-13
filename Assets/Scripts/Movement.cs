using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using FirstGearGames.SmoothCameraShaker;
using System.Collections;

public class Movement : MonoBehaviour, IDamageable
{
    private Vector2 input;
    private float moveSpeed = 8f;
    [SerializeField] float jumpingPower = 4f;
    private bool isFacingRight = true;
    private bool isJumping = false;
    private bool isPunching = false;
    private bool isGameOverScreen = false;
    public float dashPower = 20f;
    public float dashTime = 0.1f;
    public float dashCooldown = 0.1f;
    private bool isDashing;
    private bool canDash = true;
    public GameObject gameover;
    public ShakeData punchshake;
    Animator animator;
    SpriteRenderer spriteRenderer;
    private AudioSource source;
    public AudioClip impactSound;


    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] Transform _hitPosition;
    [SerializeField] int _damage = 1;
    [SerializeField] LayerMask _detectMask;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] int health = 9;
    private PlayerController PlayerController;


    [SerializeField] Vector2 _moveDirection;
    private object context;

    //health code
    public int Health { get; set; }
    public int InitialHealth { get; set; }



    //punch code stuff(yay)
    public void Punch()
    {
        RaycastHit2D hit = Physics2D.BoxCast(_hitPosition.position, Vector2.one, 0f, Vector2.right, 1f, _detectMask);

        Debug.Log("Player Punched");

        if (hit.collider != null)
        {
            hit.collider.GetComponent<IDamageable>().TakeDamage(_damage);
            animator.SetBool("isPunching", true);
            source.PlayOneShot(impactSound, 1.0f);
        }
    }

    void Update()
    {
        if (isGameOverScreen == false)
        {
            gameover.SetActive(false);
        }
        else
        {
            gameover.SetActive(true);
        }
    }

    private void TriggerDash()
    {
        if (isDashing)
        {
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash()); 
        }
    }
    
    //jump code stuff
    void Jump()
    {
        if (isDashing)
        {
            return;
        }
        
        animator.SetBool("isPunching", false);
        if (IsGrounded())
        {
            print("Grounded: Jump");
            rb.AddForce(Vector2.up * jumpingPower, ForceMode2D.Impulse);
            animator.SetBool("IsJumping", true);
        }
        else
        {
            print("Not Grounded: Cry");
        }
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isGameOverScreen = false;
        source = GetComponent<AudioSource>();
    }


    //doohickey
    private void Awake()
    {
        PlayerController ??= new PlayerController();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //input code stuff
    private void OnEnable()
    {
        PlayerController.Player.Enable();
        PlayerController.Player.LightAttack.performed += OnLightAttack;
        PlayerController.Player.HeavyAttack.performed += OnHeavyAttack;
        PlayerController.Player.Move.performed += OnMove;
        PlayerController.Player.Move.canceled += OnMove;
        PlayerController.Player.Move.performed += OnDash;
        PlayerController.Player.Move.canceled += OnDash;

        PlayerController.Player.Jump.performed += OnJump;
    }
    private void OnDisable()
    {
        PlayerController.Player.LightAttack.performed -= OnLightAttack;
        PlayerController.Player.HeavyAttack.performed -= OnHeavyAttack;
        PlayerController.Player.Move.performed -= OnMove;
        PlayerController.Player.Move.canceled -= OnMove;
        PlayerController.Player.Move.performed -= OnDash;
        PlayerController.Player.Move.canceled -= OnDash;

        PlayerController.Player.Jump.performed -= OnJump;

        PlayerController.Player.Disable();
    }

    void OnLightAttack(InputAction.CallbackContext context)
    {
        print("Light Attack");
        Punch();
        animator.SetBool("isPunching", true);
        CameraShakerHandler.Shake(punchshake);
    }

    void OnHeavyAttack(InputAction.CallbackContext context)
    {
        print("Heavy Attack");
        Punch();

    }
    void OnMove(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
        print($"Input Direction: {input}");

    }
    void OnJump(InputAction.CallbackContext context)
    {
        Jump();
        print("Jumped!");
    }
    void OnDash (InputAction.CallbackContext context)
    {
        Dash();
        print("Dashed");
    }

    //fixed update stuff
    void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
        
        _moveDirection = input * moveSpeed;
        rb.linearVelocity = new Vector2(_moveDirection.x, rb.linearVelocity.y);

        animator.SetFloat("Speed", Mathf.Abs(input.x != 0 ? input.x : input.y));
        Flip();
    }


    //touching groundy
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    //collisonstuffcode
    private void OnCollisionEnter2D(Collision2D collision)
    {
        isJumping = false;
        animator.SetBool("IsJumping", false);
    }

    //IDamageable
    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void HealDamage(int amount)
    {
        Health += amount;
    }

    public void ResetHealth()
    {
        Health = InitialHealth;
    }

    public void SetHealth(int amount)
    {
        Health = amount;
    }

    //Hitbox for the hit position(how far the player can punch)
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(_hitPosition.position, Vector2.one);
    }
    
    // Flips the character and the hit position
    private void Flip()
    {
        if (isFacingRight && input.x < 0f || !isFacingRight && input.x > 0f)
        {
            isFacingRight = !isFacingRight;
            spriteRenderer.flipX = !isFacingRight;
            _hitPosition.localPosition = _hitPosition.localPosition * new Vector2(-1, 1);
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;
        rb.linearVelocity = new Vector2(transform.localScale.x * dashPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true; 

    }
}
