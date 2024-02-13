using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Player_Controller : MonoBehaviour
{
    public PlayerInputControl inputControl;
    public Vector2 inputDirection;
    private Rigidbody2D rb;//刚体
    private SpriteRenderer sr;
    private PhysicsCheck physicsCheck;
    private CapsuleCollider2D capsuleCollider;

    [Header("基本参数")]
    public float speed;
    public float jumpForce;
    public bool isCrouch;

    private float runSpeed;
    private float walkSpeed => speed / 2.5f;

    private Vector2 originalColiderSize;
    private Vector2 originalColiderPosition;

    private void Awake()
    {

        inputControl = new PlayerInputControl();

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        physicsCheck = GetComponent<PhysicsCheck>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();

        originalColiderPosition = capsuleCollider.offset;
        originalColiderSize = capsuleCollider.size;

        #region 检测跳跃
        inputControl.Gameplay.Jump.started += Jump;
        #endregion

        #region 强制走路
        runSpeed = speed;
        inputControl.Gameplay.WalkButton.performed += ctx =>
        {
            if (physicsCheck.isGround)
            {
                speed = walkSpeed;
            }
        };
        inputControl.Gameplay.WalkButton.canceled += ctx =>
        {
            if (physicsCheck.isGround)
            {
                speed = runSpeed;
            }
        };
        #endregion
    }

    private void OnEnable()
    {
        inputControl.Enable();
    }
    private void OnDisable()
    {
        inputControl.Disable();
    }

    private void Update()
    {
        inputDirection = inputControl.Gameplay.Move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        if (!isCrouch)
            rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y);
        if (inputDirection.x <= -0.1f) sr.flipX = true;
        else if (inputDirection.x >= 0.1f) sr.flipX = false;

        //下蹲
        isCrouch = inputDirection.y < -0.1f && physicsCheck.isGround;
        if (isCrouch)
        {
            capsuleCollider.size = new Vector2(0.8181465f, 1.68f);
            capsuleCollider.offset = new Vector2(-0.06471342f, 0.82f);
        }
        else
        {
            capsuleCollider.size = originalColiderSize;
            capsuleCollider.offset = originalColiderPosition;
        }
    }

    public void Jump(InputAction.CallbackContext obj)
    {
        //Debug.Log("JUMP");
        if (physicsCheck.isGround)
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }
}
