using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;

public class Character : MonoBehaviour
{
    [Header("��ɫѪ��")]
    public float maxLife;
    public float nowLife;
    [Header("��ɫ�¶�ֵ��������ң�")]
    public float crouchTime;
    public float nowCrouchTime;
    public bool isCrouch;
    public bool canCrouch;
    public float crouchBack;
    [Header("��ɫ������������ң�")]
    public float maxTili;
    public float nowTili;
    public float tiliBack;
    public float huaChanCost;
    [Header("�����޵м�ʱ��״̬")]
    public float invulerableTime;
    private float invulerableCounter;
    public bool invulerable;
    [Header("���˺������¼�")]
    public UnityEvent<Transform, float> OnTakeDamage;
    public UnityEvent<Transform, float> OnDeath;
    [Header("Ѫ���ı�")]
    public UnityEvent<Character> OnHealthChange;
    [Header("�¶׸ı�")]
    public UnityEvent<Character> OnXiadunChange;
    [Header("���������ı�")]
    public UnityEvent<Character> OnHuaChanChange;
    private void Start()
    {
        nowLife = maxLife;
        nowCrouchTime = crouchTime;
        nowTili = maxTili;
        OnHealthChange.Invoke(this);
        OnXiadunChange.Invoke(this);
        OnHuaChanChange.Invoke(this);
    }
    private void Update()
    {
        InvulerableTimeCounter();
    }
    /// <summary>
    /// ��ɫ���˺���
    /// </summary>
    /// <param name="attacker">������</param>
    public void TakeDamage(Attack attacker)
    {
        if (invulerable)
            return;
        if (nowLife > attacker.damage)
        {
            nowLife -= attacker.damage;
            GetInvulerable();
            //��������
            OnTakeDamage?.Invoke(attacker.transform, attacker.hurtForce);
        }
        else
        {
            nowLife = 0;
            //��������
            OnDeath?.Invoke(attacker.transform, attacker.hurtForce);
        }
        OnHealthChange.Invoke(this);
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
    /// <summary>
    /// �����޵м�ʱ��
    /// </summary>
    private void InvulerableTimeCounter()
    {
        if (invulerable)
        {
            invulerableCounter -= Time.deltaTime;
            if (invulerableCounter <= 0)
            {
                invulerable = false;
            }
        }
    }
    /// <summary>
    /// �¶�ʱ���ʱ��
    /// </summary>
    public void CrouchTimeCounter()
    {
        if (isCrouch)
        {
            nowCrouchTime -= Time.deltaTime;
            OnXiadunChange?.Invoke(this);
        }
        else
        {
            if (nowCrouchTime < crouchTime)
            {
                nowCrouchTime += Time.deltaTime * crouchBack;
                OnXiadunChange?.Invoke(this);
            }
        }
        if (nowCrouchTime <= 0.1f)
        {
            canCrouch = false;
        }
        else if (nowCrouchTime >= 1f)
        {
            canCrouch = true;
        }
    }

    public void SlideChangeTili()
    {
        if (nowTili >= huaChanCost)
            nowTili -= huaChanCost;
    }

    public void TiliBack()
    {
        if (nowTili <= maxTili)
        {
            nowTili += Time.deltaTime * tiliBack;
        }
        OnHuaChanChange?.Invoke(this);
    }
}
