using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header("�˺�����Χ��Ƶ�ʣ����ȣ��Ƿ񹥻�")]
    public int damage;
    public float attackRange;
    public float attackRate;
    public float hurtForce;
    public bool ifattack;
    private void Awake()
    {
        ifattack = true;
    }

    /// <summary>
    /// �����ʹ��
    /// </summary>
    /// <param name="collision">�������Ķ���</param>
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (ifattack)
            collision.GetComponent<Character>()?.TakeDamage(this);
    }
}
