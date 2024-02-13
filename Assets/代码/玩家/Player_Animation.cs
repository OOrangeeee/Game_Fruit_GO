using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Player_Animation : MonoBehaviour
{
    private Animator anim;
    private PhysicsCheck physicsCheck;
    private Rigidbody2D rb;
    private Player_Controller playerController;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        playerController = GetComponent<Player_Controller>();
    }
    private void Update()
    {
        SetAnimation();
    }

    public void SetAnimation()
    {
        anim.SetFloat("speedX", math.abs(rb.velocity.x));
        anim.SetFloat("speedY", rb.velocity.y);
        anim.SetBool("isGround", physicsCheck.isGround);
        anim.SetBool("isCrouch", playerController.isCrouch);
    }

    public void PlayHurt()
    {
        anim.SetTrigger("hurt");
    }
}
