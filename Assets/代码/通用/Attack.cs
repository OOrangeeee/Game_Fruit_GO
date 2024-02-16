using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header("伤害，范围，频率，力度，是否攻击")]
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
    /// 碰到就打击
    /// </summary>
    /// <param name="collision">被碰到的对象</param>
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (ifattack)
            collision.GetComponent<Character>()?.TakeDamage(this);
    }
}
