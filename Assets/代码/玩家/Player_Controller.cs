using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Player_Controller : MonoBehaviour
{
    [Header("控件")]
    private PlayerInputControl inputControl;
    private Rigidbody2D rb;//刚体
    private PhysicsCheck physicsCheck;
    private CapsuleCollider2D capsuleCollider;
    private Player_Animation playerAnimation;
    private CapsuleCollider2D cc2;
    private Character character;
    [Header("移动参数")]
    private Vector2 inputDirection;//输入方向
    public float speed;
    public float runSpeed;
    private float walkSpeed => speed / 2.5f;
    private float attackSpeed => speed / 1.5f;
    private Vector2 originalColiderSize;
    private Vector2 originalColiderPosition;
    [Header("跳跃参数")]
    public float jumpForce;
    [Header("状态")]
    [HideInInspector] public bool isHurt;
    [HideInInspector] public bool isDead;
    [HideInInspector] public bool isAttack;
    [Header("材质")]
    public PhysicsMaterial2D noMoCa;
    public PhysicsMaterial2D normal;

    private void Awake()
    {
        #region 获取控件
        inputControl = new PlayerInputControl();
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        playerAnimation = GetComponent<Player_Animation>();
        cc2 = GetComponent<CapsuleCollider2D>();
        character = GetComponent<Character>();
        #endregion

        #region 下蹲初始化
        originalColiderPosition = capsuleCollider.offset;
        originalColiderSize = capsuleCollider.size;
        #endregion

        #region 跳跃函数挂载
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

        #region 攻击函数挂载
        inputControl.Gameplay.Attack.started += PlayerAttack;
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
        #region 输入方向
        inputDirection = inputControl.Gameplay.Move.ReadValue<Vector2>();
        #endregion
    }
    private void FixedUpdate()
    {
        if (!isHurt)
        {
            Move();
        }
        character.CrouchTimeCounter();
        ChanegPhysicsMaterial();
    }
    /// <summary>
    /// 移动函数
    /// </summary>
    public void Move()
    {
        if (!character.isCrouch)
            rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y);
        if (inputDirection.x <= -0.1f)
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }
        else if (inputDirection.x >= 0.1f)
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }

        #region 下蹲
        if (character.canCrouch)
            character.isCrouch = inputDirection.y < -0.1f && physicsCheck.isGround;
        else character.isCrouch = false;
        if (character.isCrouch)
        {
            capsuleCollider.size = new Vector2(0.8181465f, 1.68f);
            capsuleCollider.offset = new Vector2(-0.06471342f, 0.82f);
            gameObject.layer = 9;
        }
        else
        {
            capsuleCollider.size = originalColiderSize;
            capsuleCollider.offset = originalColiderPosition;
            gameObject.layer = 7;
        }
        #endregion
    }
    /// <summary>
    /// 跳跃函数，挂载到输入按钮
    /// </summary>
    /// <param name="obj">无</param>
    public void Jump(InputAction.CallbackContext obj)
    {
        if (physicsCheck.isGround)
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }
    /// <summary>
    /// 攻击函数，挂载到攻击按钮
    /// </summary>
    /// <param name="context">无</param>
    private void PlayerAttack(InputAction.CallbackContext context)
    {
        playerAnimation.PlayerAttack();
        isAttack = true;
        if (speed == runSpeed)
            speed = attackSpeed;
    }
    /// <summary>
    /// 改版材质，起跳无摩擦
    /// </summary>
    private void ChanegPhysicsMaterial()
    {
        if (physicsCheck.isGround)
        {
            cc2.sharedMaterial = normal;
        }
        else
        {
            cc2.sharedMaterial = noMoCa;
        }
    }
    /// <summary>
    /// 受伤函数，挂载到受伤event
    /// </summary>
    /// <param name="attackerTransform">攻击者位置</param>
    /// <param name="hurtForce">力度</param>
    public void GetHurt(Transform attackerTransform, float hurtForce)
    {
        isHurt = true;
        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2(transform.position.x - attackerTransform.position.x, (transform.position.y - attackerTransform.position.y) + 1).normalized;
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);

    }
    /// <summary>
    /// 死亡时间，挂载到死亡event
    /// </summary>
    public void PlayerDead()
    {
        isDead = true;
        inputControl.Gameplay.Disable();
    }
}
