using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;

public class Character : MonoBehaviour
{
    [Header("角色血量")]
    public float maxLife;
    public float nowLife;
    [Header("角色下蹲值（仅限玩家）")]
    public float crouchTime;
    public float nowCrouchTime;
    public bool isCrouch;
    public bool canCrouch;
    public float crouchBack;
    [Header("角色体力（仅限玩家）")]
    public float maxTili;
    public float nowTili;
    public float tiliBack;
    public float huaChanCost;
    [Header("受伤无敌计时和状态")]
    public float invulerableTime;
    private float invulerableCounter;
    public bool invulerable;
    [Header("受伤和死亡事件")]
    public UnityEvent<Transform, float> OnTakeDamage;
    public UnityEvent<Transform, float> OnDeath;
    [Header("血量改变")]
    public UnityEvent<Character> OnHealthChange;
    [Header("下蹲改变")]
    public UnityEvent<Character> OnXiadunChange;
    [Header("滑铲体力改变")]
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
    /// 角色受伤函数
    /// </summary>
    /// <param name="attacker">攻击者</param>
    public void TakeDamage(Attack attacker)
    {
        if (invulerable)
            return;
        if (nowLife > attacker.damage)
        {
            nowLife -= attacker.damage;
            GetInvulerable();
            //触发受伤
            OnTakeDamage?.Invoke(attacker.transform, attacker.hurtForce);
        }
        else
        {
            nowLife = 0;
            //触发死亡
            OnDeath?.Invoke(attacker.transform, attacker.hurtForce);
        }
        OnHealthChange.Invoke(this);
    }

    /// <summary>
    /// 触发无敌
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
    /// 进入无敌计时器
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
    /// 下蹲时间计时器
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
