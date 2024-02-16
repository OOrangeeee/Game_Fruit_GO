using System;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class beeChaseState : Base_State
{
    private Vector3 target;
    private Vector3 moveDir;
    private Attack attack;
    private bool isAttack;
    private float attackRateCounter;
    public override void OnEnter(enemy enemy)
    {
        nowEnemy = enemy;
        nowEnemy.nowWaitTime = nowEnemy.waitTime;
        nowEnemy.nowSpeed = nowEnemy.chaseSpeed;
        attack = nowEnemy.GetComponent<Attack>();
        nowEnemy.anim.SetBool("run", true);
        attack.ifattack = false;
    }
    public override void LogicUpdate()
    {
        nowEnemy.FoundTimeCounter();
        if (nowEnemy.nowLostTime <= 0)
            nowEnemy.SwitchState(NPCState.Patrol);
        target = new Vector3(nowEnemy.attacker.position.x, nowEnemy.attacker.position.y + 1.5f, nowEnemy.attacker.position.z);
        if (Math.Abs(target.x - nowEnemy.transform.position.x) <= attack.attackRange && Math.Abs(target.y - nowEnemy.transform.position.y) <= 0.1f)
        {
            isAttack = true;
            if (!nowEnemy.isHurt)
                nowEnemy.rb.velocity = Vector2.zero;

            attackRateCounter -= Time.deltaTime;
            if (attackRateCounter <= 0)
            {
                nowEnemy.anim.SetBool("attack", true);
                attackRateCounter = attack.attackRate;
            }
        }
        else
        {
            isAttack = false;
        }
        moveDir = (target - nowEnemy.transform.position).normalized;
        if (moveDir.x > 0)
        {
            nowEnemy.transform.localScale = new Vector3(-1, nowEnemy.transform.localScale.y, nowEnemy.transform.localScale.z);
        }
        if (moveDir.x < 0)
        {
            nowEnemy.transform.localScale = new Vector3(1, nowEnemy.transform.localScale.y, nowEnemy.transform.localScale.z);
        }
    }
    public override void PhysicsUpdate()
    {
        if (!nowEnemy.isDead && !nowEnemy.isHurt && !isAttack)
        {
            nowEnemy.rb.velocity = moveDir * nowEnemy.nowSpeed * Time.deltaTime;
            nowEnemy.anim.SetBool("run", true);
        }
    }
    public override void OnExit()
    {
        nowEnemy.anim.SetBool("run", false);
    }
}
