using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour, IDamageable
{
    private float horizontalInput;
    private float verticalInput;
    private float moveSpeed = 8f;
    [SerializeField] float jumpingPower = 4f;
    private bool isFacingRight = true;
    private bool isJumping = false;
    Animator animator;


    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] Transform _hitPosition;
    [SerializeField] int _damage = 1;
    [SerializeField] LayerMask _detectMask;

    private PlayerController PlayerController;

    [SerializeField] Vector2 _moveDirection;

    public int Health { get; set; }
    public int InitialHealth { get; set; }

    public void Punch()
    {
        RaycastHit2D hit = Physics2D.BoxCast(_hitPosition.position, Vector2.one, 0f, Vector2.right, 1f, _detectMask);

        Debug.Log("Player Punched");
        
        if(hit.collider!= null)
        {
            hit.collider.GetComponent<IDamageable>().TakeDamage(_damage);
        }
    }

    void Jump()
    {
        if(IsGrounded())
        {
            print("Was Grounded: Jump");
            rb.AddForce(Vector2.up * jumpingPower, ForceMode2D.Impulse);
        }
        else
        {
            print("Not Grounded: Cry");
        }
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Awake()
    {
        PlayerController ??= new PlayerController();
        animator = GetComponent<Animator>();
    }

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
    }

    void OnHeavyAttack(InputAction.CallbackContext context)
    {
        print("Heavy Attack");
        Punch();

    }
    void OnMove(InputAction.CallbackContext context)
    {
        _moveDirection = context.ReadValue<Vector2>();
        print($"Move Direction: {_moveDirection}");

    }
    void OnJump(InputAction.CallbackContext context)
    {
        Jump();
        print("Jumped!");
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(_moveDirection.x * moveSpeed, rb.linearVelocity.y);

        animator.SetFloat("Speed", Mathf.Abs(horizontalInput != 0 ? horizontalInput : verticalInput));
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isJumping = false;
    }

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

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(_hitPosition.position, Vector2.one);
    }
    private void Flip()
    {
        if (isFacingRight && horizontalInput < 0f || isFacingRight && horizontalInput > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= 1f;
            transform.localScale = localScale;
        }

    }
}
