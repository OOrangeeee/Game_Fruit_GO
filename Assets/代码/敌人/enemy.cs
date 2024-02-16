using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(PhysicsCheck))]

public class enemy : MonoBehaviour
{
    [Header("�ؼ�")]
    [HideInInspector] public Animator anim;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public PhysicsCheck physicsCheck;
    [HideInInspector] public CapsuleCollider2D capsuleCollider;

    [Header("�ƶ��ٶ�")]
    public float normalSpeed;
    public float chaseSpeed;
    [HideInInspector] public float nowSpeed;

    [Header("������")]
    [HideInInspector] public Vector3 faceDir;

    [Header("������")]
    [HideInInspector] public Transform attacker;

    [Header("״̬")]
    public bool isHurt;
    public bool isDead;
    public bool ifwait;

    [Header("״̬��״̬")]
    private Base_State nowState;
    protected Base_State patrolState;
    protected Base_State chaseState;
    protected Base_State skillState;

    [Header("�ȴ�ʱ��")]
    public float waitTime;
    [HideInInspector] public float nowWaitTime;

    [Header("���ȴ�ʱ��")]
    public float lostTime;
    [HideInInspector] public float nowLostTime;

    [Header("������")]
    public Vector2 centeroffset;//�������ƫ��
    public Vector2 checkSize;
    public float checkDistance;
    public LayerMask attackLayer;

    [Header("������Ϣ")]
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
    /// �ƶ�����
    /// </summary>
    public virtual void Move()
    {
        rb.velocity = new Vector2(nowSpeed * faceDir.x * Time.deltaTime, rb.velocity.y);
    }
    /// <summary>
    /// �ȴ���ʱ��
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
    /// �������˺���
    /// </summary>
    /// <param name="attackerTransform">������</param>
    /// <param name="hurtForce">����ɵ���</param>
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
    /// �𲽽��У��Ȼ��ɣ��ȼ������ж�
    /// </summary>
    /// <param name="dir">���ɷ���</param>
    /// <param name="hurtForce">����</param>
    /// <returns></returns>
    private IEnumerator OnHurt(Vector2 dir, float hurtForce)
    {
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.7f);
        isHurt = false;
    }
    /// <summary>
    /// �������������ص�unityevent��event��Character�д���
    /// </summary>
    /// <param name="attackerTransform">������λ��</param>
    /// <param name="hurtForce">����</param>
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
    /// ���������٣����ص������¼�
    /// </summary>
    public void DeatroyAfterAnim()
    {
        Destroy(this.gameObject);
    }
    /// <summary>
    /// ����ٶ�
    /// </summary>
    public void StopRb()
    {
        rb.velocity = new Vector2(0, 0);
    }
    /// <summary>
    /// ��ó������ڸı䱻�����ķ���
    /// </summary>
    public void GetFace()
    {
        faceDir = new Vector3(-transform.localScale.x, 0, 0);//��ȡ����
    }
    /// <summary>
    /// �������
    /// </summary>
    /// <returns>�Ƿ��ҵ�</returns>
    public virtual bool FoundPlayer()
    {
        return Physics2D.BoxCast(transform.position + (Vector3)centeroffset, checkSize, 0, faceDir, checkDistance, attackLayer);
    }
    /// <summary>
    /// �л�״̬
    /// </summary>
    /// <param name="state">ѡ��</param>
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
    /// ���ӻ����
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
