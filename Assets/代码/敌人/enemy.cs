using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(PhysicsCheck))]

public class enemy : MonoBehaviour
{
    [Header("控件")]
    [HideInInspector] public Animator anim;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public PhysicsCheck physicsCheck;
    [HideInInspector] public CapsuleCollider2D capsuleCollider;

    [Header("移动速度")]
    public float normalSpeed;
    public float chaseSpeed;
    [HideInInspector] public float nowSpeed;

    [Header("面向方向")]
    [HideInInspector] public Vector3 faceDir;

    [Header("攻击者")]
    [HideInInspector] public Transform attacker;

    [Header("状态")]
    public bool isHurt;
    public bool isDead;
    public bool ifwait;

    [Header("状态机状态")]
    private Base_State nowState;
    protected Base_State patrolState;
    protected Base_State chaseState;
    protected Base_State skillState;

    [Header("等待时间")]
    public float waitTime;
    [HideInInspector] public float nowWaitTime;

    [Header("检测等待时间")]
    public float lostTime;
    [HideInInspector] public float nowLostTime;

    [Header("检测玩家")]
    public Vector2 centeroffset;//检测中心偏移
    public Vector2 checkSize;
    public float checkDistance;
    public LayerMask attackLayer;

    [Header("基本信息")]
    public Vector3 startPoint;
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        physicsCheck = GetComponent<PhysicsCheck>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        nowSpeed = normalSpeed;
        startPoint = transform.position;
    }
    private void OnEnable()
    {
        nowState = patrolState;
        nowState.OnEnter(this);
    }

    private void Update()
    {
        nowState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        nowState.PhysicsUpdate();
    }

    private void OnDisable()
    {
        nowState.OnExit();
    }
    /// <summary>
    /// 移动函数
    /// </summary>
    public virtual void Move()
    {
        rb.velocity = new Vector2(nowSpeed * faceDir.x * Time.deltaTime, rb.velocity.y);
    }
    /// <summary>
    /// 等待计时器
    /// </summary>
    public void WaitTimeCounter()
    {
        if (ifwait)
        {
            nowWaitTime -= Time.deltaTime;
            if (nowWaitTime <= 0)
            {
                ifwait = false;
                nowWaitTime = waitTime;
                transform.localScale = new Vector3(faceDir.x, 1, 1);
            }
        }
    }
    public void FoundTimeCounter()
    {
        if (!FoundPlayer() && nowLostTime > 0)
        {
            nowLostTime -= Time.deltaTime;
        }
        else if (FoundPlayer())
        {
            nowLostTime = lostTime;
        }
    }
    /// <summary>
    /// 敌人受伤函数
    /// </summary>
    /// <param name="attackerTransform">攻击者</param>
    /// <param name="hurtForce">被打飞的力</param>
    public void EnemyTakeDamage(Transform attackerTransform, float hurtForce)
    {
        attacker = attackerTransform;
        if (attackerTransform.position.x - transform.position.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if (attackerTransform.position.x - transform.position.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        isHurt = true;
        if (nowState != skillState)
            anim.SetTrigger("hurt");

        StopRb();
        Vector2 dir = new Vector2(transform.position.x - attackerTransform.position.x, (transform.position.y - attackerTransform.position.y) + 1).normalized;
        StartCoroutine(OnHurt(dir, hurtForce));

    }
    /// <summary>
    /// 逐步进行，先击飞，等几秒再行动
    /// </summary>
    /// <param name="dir">击飞方向</param>
    /// <param name="hurtForce">力度</param>
    /// <returns></returns>
    private IEnumerator OnHurt(Vector2 dir, float hurtForce)
    {
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.7f);
        isHurt = false;
    }
    /// <summary>
    /// 死亡函数，挂载到unityevent，event在Character中触发
    /// </summary>
    /// <param name="attackerTransform">攻击者位置</param>
    /// <param name="hurtForce">力度</param>
    public void OnDie(Transform attackerTransform, float hurtForce)
    {
        this.gameObject.layer = 2;
        anim.SetBool("dead", true);
        StopRb();
        Vector2 dir = new Vector2(transform.position.x - attackerTransform.position.x, (transform.position.y - attackerTransform.position.y) + 1).normalized;
        rb.AddForce(dir * hurtForce * 5, ForceMode2D.Impulse);
        isDead = true;
    }
    /// <summary>
    /// 死亡后销毁，挂载到动画事件
    /// </summary>
    public void DeatroyAfterAnim()
    {
        Destroy(this.gameObject);
    }
    /// <summary>
    /// 清除速度
    /// </summary>
    public void StopRb()
    {
        rb.velocity = new Vector2(0, 0);
    }
    /// <summary>
    /// 获得朝向，用于改变被打击后的方向
    /// </summary>
    public void GetFace()
    {
        faceDir = new Vector3(-transform.localScale.x, 0, 0);//获取朝向
    }
    /// <summary>
    /// 查找玩家
    /// </summary>
    /// <returns>是否找到</returns>
    public virtual bool FoundPlayer()
    {
        return Physics2D.BoxCast(transform.position + (Vector3)centeroffset, checkSize, 0, faceDir, checkDistance, attackLayer);
    }
    /// <summary>
    /// 切换状态
    /// </summary>
    /// <param name="state">选择</param>
    public void SwitchState(NPCState state)
    {
        var newState = state switch
        {
            NPCState.Patrol => patrolState,
            NPCState.Chase => chaseState,
            NPCState.Skill => skillState,
            _ => null,
        };
        nowState.OnExit();
        nowState = newState;
        nowState.OnEnter(this);
    }
    /// <summary>
    /// 可视化检测
    /// </summary>
    public virtual void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + (Vector3)centeroffset + new Vector3(checkDistance * -transform.localScale.x, 0, 0), 0.2f);
    }

    public virtual Vector3 GetNewPoint()
    {
        return transform.position;
    }

}
