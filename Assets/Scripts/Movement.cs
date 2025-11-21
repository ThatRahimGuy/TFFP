using Unity.Properties;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    private float horizontalInput;
    private float moveSpeed = 8f;
    private float jumpingPower = 0f;
    private bool isFacingRight = true;
    private bool isJumping = false;

   
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private PlayerController PlayerController;

    [SerializeField] Vector2 _moveDirection;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Awake()
    {
        PlayerController ??= new PlayerController();
    }
   
    private void OnEnable()
    {
        PlayerController.Player.Enable();

        PlayerController.Player.LightAttack.performed += OnLightAttack;
        PlayerController.Player.HeavyAttack.performed += OnHeavyAttack;
        PlayerController.Player.Move.performed += OnMove;
        PlayerController.Player.Jump.performed += OnJump;
    }
    private void OnDisable()
    {
        PlayerController.Player.LightAttack.performed -= OnLightAttack;
        PlayerController.Player.HeavyAttack.performed -= OnHeavyAttack;
        PlayerController.Player.Move.performed -= OnMove;
        PlayerController.Player.Jump.performed -= OnJump;

        PlayerController.Player.Disable();
    }

    void OnLightAttack(InputAction.CallbackContext context)
    {
        print("Light Attack");
    }

    void OnHeavyAttack(InputAction.CallbackContext context)
    {
        print("Heavy Attack");

    }
    void OnMove(InputAction.CallbackContext context)
    {
        _moveDirection = context.ReadValue<Vector2>();
        print($"Move Direction: {_moveDirection}");

    }
    void OnJump(InputAction.CallbackContext context)
    {
        print("Jumped!");
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        //if (Input.GetButtonDown("Jump")!! || (isJumping))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
            isJumping = true;
        }
 
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isJumping = false;
    }


}
