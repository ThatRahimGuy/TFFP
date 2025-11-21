using UnityEngine;
using UnityEngine.InputSystem;

public class TestMove : MonoBehaviour
{
    private PlayerInput playerInput;
    private Vector2 _moveDirection;

    //Inputs
    InputAction moveAction;
    InputAction jumpAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        var actionMap = playerInput.currentActionMap;

        moveAction = actionMap.FindAction("Move");
        jumpAction = actionMap.FindAction("Jump");
    }

    private void OnEnable()
    {
        moveAction.performed += OnMove;
        jumpAction.performed += OnJump;
    }

    private void OnDisable()
    {
        moveAction.performed -= OnMove;
        jumpAction.performed -= OnJump;
    }

        void OnMove(InputAction.CallbackContext context)
    {
        print("Move YAHHHAH");
    }

    void OnJump(InputAction.CallbackContext context)
    {
        print("JUmp YAHHHH");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
