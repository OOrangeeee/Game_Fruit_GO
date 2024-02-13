using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float maxLife;
    public float nowLife;
    [Header("�����޵�")]
    public float invulerableTime;
    private float invulerableCounter;
    public bool invulerable;
    private void Start()
    {
        nowLife = maxLife;
    }

    private void Update()
    {
        if (invulerable)
        {
            invulerableCounter -= Time.deltaTime;
            if(invulerableCounter <= 0 )
            {
                invulerable = false;
            }
        }
    }
    public void TakeDamage(Attack attacker)
    {
        if (invulerable) return;
        if(nowLife>attacker.damage)
        {
            nowLife -= attacker.damage;
            GetInvulerable();
        }
        else
        {
            nowLife = 0;
            //��������
        }
    }

    /// <summary>
    /// �����޵�
    /// </summary>
    private void GetInvulerable()
    {
        if (!invulerable)
        {
            invulerable = true;
            invulerableCounter = invulerableTime;
        }
    }
}
