using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    [Header("�ֶ����Ʋ���")]
    public bool isPlayer;
    public bool manual;
    [Header("�ؼ�")]
    private CapsuleCollider2D cc2;
    private Player_Controller playerController;
    private Rigidbody2D rb;
    [Header("�����ײ")]
    public float checkRaduis;//��ⷶΧ
    public Vector2 bottomOffset;//�ŵ�λ�Ʋ�ֵ
    public Vector2 leftOffset;
    public Vector2 rightOffset;
    public bool isGround;
    public bool touchLeftWall;
    public bool touchRightWall;
    public LayerMask groundLayer;//���ͼ��
    public bool isWall;

    private void Awake()
    {
        cc2=GetComponent<CapsuleCollider2D>();
        if (!manual)
        {
            rightOffset = new Vector2((cc2.bounds.size.x + cc2.offset.x) / 2f, cc2.bounds.size.y / 2);
            leftOffset = new Vector2(-rightOffset.x, rightOffset.y);
        }
        if (isPlayer)
        {
            playerController = GetComponent<Player_Controller>();
            rb = GetComponent<Rigidbody2D>();
        }
    }

    private void Update()
    {
        check();
    }
    /// <summary>
    /// ����������Ƿ���������
    /// </summary>
    public void check()
    {
        if (isWall)
            isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(bottomOffset.x * transform.localScale.x, bottomOffset.y), checkRaduis, groundLayer);
        else
            isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(bottomOffset.x * transform.localScale.x, 0), checkRaduis, groundLayer);
        touchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(leftOffset.x, leftOffset.y), checkRaduis, groundLayer);
        touchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(rightOffset.x, rightOffset.y), checkRaduis, groundLayer);

        if (isPlayer)
        {
            isWall = (touchLeftWall && playerController.inputDirection.x < 0f || touchRightWall && playerController.inputDirection.x > 0f) && rb.velocity.y < 0f;

        }
    }

    /// <summary>
    /// ���ӻ���ⷶΧ
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(bottomOffset.x * transform.localScale.x, bottomOffset.y), checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(leftOffset.x, leftOffset.y), checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(rightOffset.x, rightOffset.y), checkRaduis);
    }
}
