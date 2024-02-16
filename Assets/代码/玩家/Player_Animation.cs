using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Player_Animation : MonoBehaviour
{
    [Header("�ؼ�")]
    private Animator anim;
    private PhysicsCheck physicsCheck;
    private Rigidbody2D rb;
    private Player_Controller playerController;
    private Character character;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        playerController = GetComponent<Player_Controller>();
        character = GetComponent<Character>();
    }
    private void Update()
    {
        SetAnimation();
    }
    /// <summary>
    /// ʱ�̸��¶���
    /// </summary>
    public void SetAnimation()
    {
        anim.SetFloat("speedX", math.abs(rb.velocity.x));
        anim.SetFloat("speedY", rb.velocity.y);
        anim.SetBool("isGround", physicsCheck.isGround);
        anim.SetBool("isCrouch", character.isCrouch);
        anim.SetBool("isDeath", playerController.isDead);
        anim.SetBool("isAttack", playerController.isAttack);
        //anim.SetBool("turn", playerController.transform.localScale.x < 0 ? true : false);
    }

    public void PlayHurt()
    {
        anim.SetTrigger("hurt");
    }

    public void PlayerAttack()
    {
        anim.SetTrigger("attack");
    }
}
