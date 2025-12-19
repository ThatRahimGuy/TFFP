using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using FirstGearGames.SmoothCameraShaker;

public class Movement : MonoBehaviour, IDamageable
{
    private Vector2 input;
    private float moveSpeed = 8f;
    [SerializeField] float jumpingPower = 4f;
    private bool isFacingRight = true;
    private bool isJumping = false;
    // private bool isPunching = false;
    public ShakeData punchshake;
    Animator animator;
    SpriteRenderer spriteRenderer;


    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] Transform _hitPosition;
    [SerializeField] int _damage = 1;
    [SerializeField] LayerMask _detectMask;

    private PlayerController PlayerController;

    [SerializeField] Vector2 _moveDirection;

    //health code
    public int Health { get; set; }
    public int InitialHealth { get; set; }

    //punch code stuff(yay)
    public void Punch()
    {
        RaycastHit2D hit = Physics2D.BoxCast(_hitPosition.position, Vector2.one, 0f, Vector2.right, 1f, _detectMask);

        Debug.Log("Player Punched");
        
        if(hit.collider!= null)
        {
            hit.collider.GetComponent<IDamageable>().TakeDamage(_damage);
        }
    }

    //jump code stuff
    void Jump()
    {
        if(IsGrounded())
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
    //they rigid on my body till I 2D
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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

        PlayerController.Player.Jump.performed += OnJump;
    }
    private void OnDisable()
    {
        PlayerController.Player.LightAttack.performed -= OnLightAttack;
        PlayerController.Player.HeavyAttack.performed -= OnHeavyAttack;
        PlayerController.Player.Move.performed -= OnMove;
        PlayerController.Player.Move.canceled -= OnMove;

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

    //fixed update stuff
    void FixedUpdate()
    {
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
        Health -= amount;
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
}
