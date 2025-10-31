using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Movement : MonoBehaviour
{
    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 16f;
    private bool isFacingRight = true;


    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private PlayerInput playerInput;
    private InputAction lightAttack;
    private InputAction heavyAttack;
    private InputAction move;
    private InputAction jump;

    [SerializeField] Vector2 _moveDirection;

    private void Awake()
    {
        var actions = playerInput.actions;

        lightAttack = actions.FindAction("LightAttack");
        heavyAttack = actions.FindAction("HeavyAttack");
        move = actions.FindAction("Move");
        jump = actions.FindAction("Jump");
    }
    private void OnEnable()
    {
        lightAttack.performed += OnLightAttack;
        heavyAttack.performed += OnHeavyAttack;
        move.performed += OnMove;
        jump.performed += OnJump;
    }
    private void OnDisable()
    {
        lightAttack.performed -= OnLightAttack;
        heavyAttack.performed -= OnHeavyAttack;
        move.performed -= OnMove;
        jump.performed -= OnJump;
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

    }

    void FixedUpdate()
    {
        //rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
    
    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale; 
            localScale.x *= 1f;
            transform.localScale = localScale;
        }

    }
}
