using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    [Header("�ֶ����Ʋ���")]
    public bool manual;
    [Header("�ؼ�")]
    private CapsuleCollider2D cc2;
    [Header("�����ײ")]
    public float checkRaduis;//��ⷶΧ
    public Vector2 bottomOffset;//�ŵ�λ�Ʋ�ֵ
    public Vector2 leftOffset;
    public Vector2 rightOffset;
    public bool isGround;
    public bool touchLeftWall;
    public bool touchRightWall;
    public LayerMask groundLayer;//���ͼ��

    private void Awake()
    {
        if (!manual)
        {
            rightOffset = new Vector2((cc2.bounds.size.x + cc2.offset.x) / 2f, cc2.bounds.size.y / 2);
            leftOffset = new Vector2(-rightOffset.x, rightOffset.y);
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
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(bottomOffset.x * transform.localScale.x, bottomOffset.y), checkRaduis, groundLayer);

        touchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(leftOffset.x, leftOffset.y), checkRaduis, groundLayer);
        touchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(rightOffset.x, rightOffset.y), checkRaduis, groundLayer);


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
