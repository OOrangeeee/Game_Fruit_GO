using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    [Header("手动控制参数")]
    public bool manual;
    [Header("控件")]
    private CapsuleCollider2D cc2;
    [Header("检测碰撞")]
    public float checkRaduis;//检测范围
    public Vector2 bottomOffset;//脚底位移差值
    public Vector2 leftOffset;
    public Vector2 rightOffset;
    public bool isGround;
    public bool touchLeftWall;
    public bool touchRightWall;
    public LayerMask groundLayer;//检测图层

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
    /// 检测左右下是否碰到地面
    /// </summary>
    public void check()
    {
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(bottomOffset.x * transform.localScale.x, bottomOffset.y), checkRaduis, groundLayer);

        touchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(leftOffset.x, leftOffset.y), checkRaduis, groundLayer);
        touchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(rightOffset.x, rightOffset.y), checkRaduis, groundLayer);


    }
    /// <summary>
    /// 可视化检测范围
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(bottomOffset.x * transform.localScale.x, bottomOffset.y), checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(leftOffset.x, leftOffset.y), checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(rightOffset.x, rightOffset.y), checkRaduis);
    }
}
