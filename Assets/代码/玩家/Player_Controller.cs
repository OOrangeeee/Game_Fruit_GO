using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Player_Controller : MonoBehaviour
{
    [Header("�ؼ�")]
    private PlayerInputControl inputControl;
    private Rigidbody2D rb;//����
    private PhysicsCheck physicsCheck;
    private CapsuleCollider2D capsuleCollider;
    private Player_Animation playerAnimation;
    private CapsuleCollider2D cc2;
    private Character character;
    [Header("�ƶ�����")]
    [HideInInspector] public Vector2 inputDirection;//���뷽��
    public float speed;
    public float runSpeed;
    private float walkSpeed => speed / 2.5f;
    private float attackSpeed => speed / 1.5f;
    private Vector2 originalColiderSize;
    private Vector2 originalColiderPosition;
    [Header("��Ծ����")]
    public float jumpForce;
    public float dengQiangForce;
    public bool wallJump;
    [Header("��������")]
    public float slideDistance;
    public float slideFroce;
    [Header("״̬")]
    [HideInInspector] public bool isHurt;
    [HideInInspector] public bool isDead;
    [HideInInspector] public bool isAttack;
    [HideInInspector] public bool isSlide;
    [Header("����")]
    public PhysicsMaterial2D noMoCa;
    public PhysicsMaterial2D normal;

    private void Awake()
    {
        #region ��ȡ�ؼ�
        inputControl = new PlayerInputControl();
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        playerAnimation = GetComponent<Player_Animation>();
        cc2 = GetComponent<CapsuleCollider2D>();
        character = GetComponent<Character>();
        #endregion

        #region �¶׳�ʼ��
        originalColiderPosition = capsuleCollider.offset;
        originalColiderSize = capsuleCollider.size;
        #endregion

        #region ��Ծ��������
        inputControl.Gameplay.Jump.started += Jump;
        #endregion

        #region ǿ����·
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

        #region ������������
        inputControl.Gameplay.Attack.started += PlayerAttack;
        #endregion

        #region ��������
        inputControl.Gameplay.Slide.started += Slide;
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
        #region ���뷽��
        inputDirection = inputControl.Gameplay.Move.ReadValue<Vector2>();
        #endregion
    }
    private void FixedUpdate()
    {
        if (!isHurt && !isSlide)
        {
            Move();
        }
        character.CrouchTimeCounter();
        character.TiliBack();
        ChanegPhysicsMaterial();
    }
    /// <summary>
    /// �ƶ�����
    /// </summary>
    public void Move()
    {
        if (!character.isCrouch && !wallJump && !isSlide)
            rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y);
        if (inputDirection.x <= -0.1f)
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }
        else if (inputDirection.x >= 0.1f)
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }

        #region �¶�
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
    /// ��Ծ���������ص����밴ť
    /// </summary>
    /// <param name="obj">��</param>
    public void Jump(InputAction.CallbackContext obj)
    {
        if (physicsCheck.isGround && !isSlide)
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        else if (physicsCheck.isWall && !isSlide)
        {
            rb.AddForce(new Vector2(-inputDirection.x, 3f) * dengQiangForce, ForceMode2D.Impulse);
            wallJump = true;
        }

    }
    /// <summary>
    /// �������������ص�������ť
    /// </summary>
    /// <param name="context">��</param>
    private void PlayerAttack(InputAction.CallbackContext context)
    {
        playerAnimation.PlayerAttack();
        isAttack = true;
        if (speed == runSpeed)
            speed = attackSpeed;
    }
    /// <summary>
    /// �İ���ʣ�������Ħ��
    /// </summary>
    private void ChanegPhysicsMaterial()
    {
        if (physicsCheck.isGround && !isSlide)
        {
            cc2.sharedMaterial = normal;
        }
        else
        {
            cc2.sharedMaterial = noMoCa;
        }
        if (physicsCheck.isWall)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 4f);
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        }
        if (wallJump && rb.velocity.y < 0)
        {
            wallJump = false;
        }
    }
    /// <summary>
    /// ���˺��������ص�����event
    /// </summary>
    /// <param name="attackerTransform">������λ��</param>
    /// <param name="hurtForce">����</param>
    public void GetHurt(Transform attackerTransform, float hurtForce)
    {
        isHurt = true;
        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2(transform.position.x - attackerTransform.position.x, (transform.position.y - attackerTransform.position.y) + 1).normalized;
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);

    }
    /// <summary>
    /// ����ʱ�䣬���ص�����event
    /// </summary>
    public void PlayerDead()
    {
        isDead = true;
        inputControl.Gameplay.Disable();
    }

    private void Slide(InputAction.CallbackContext context)
    {
        if (!isSlide && physicsCheck.isGround && character.nowTili >= character.huaChanCost && !(physicsCheck.touchLeftWall && transform.localScale.x < 0) && !(physicsCheck.touchRightWall && transform.localScale.x > 0))
        {
            isSlide = true;
            character.SlideChangeTili();
            gameObject.layer = 11;
            var targetPos = new Vector3(transform.position.x + slideDistance * transform.localScale.x, transform.position.y);
            rb.AddForce(new Vector2(transform.localScale.x * slideFroce, 0), ForceMode2D.Impulse);
            StartCoroutine(InSlide(targetPos));
        }
    }
    private IEnumerator InSlide(Vector3 target)
    {
        do
        {
            yield return null;
            if (physicsCheck.touchLeftWall || physicsCheck.touchRightWall)
            {
                isSlide = false;
                rb.velocity = new Vector2(0, rb.velocity.y);
                yield break;
            }
            Debug.Log("OK");
        } while (Math.Abs(target.x - transform.position.x) > 0.1f && Math.Abs(target.x - transform.position.x) < slideDistance + 1 && Math.Abs(rb.velocity.x) >= 0.1f);
        isSlide = false;
        gameObject.layer = 7;
    }
}
