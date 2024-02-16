using System;
using UnityEngine;

public class beePatrolState : Base_State
{
    private Vector3 target;
    private Vector3 moveDir;
    public override void OnEnter(enemy enemy)
    {
        nowEnemy = enemy;
        nowEnemy.nowWaitTime = nowEnemy.waitTime;
        nowEnemy.nowSpeed = nowEnemy.normalSpeed;
        target = nowEnemy.GetNewPoint();
        nowEnemy.anim.SetBool("run", false);
    }
    public override void LogicUpdate()
    {
        if (nowEnemy.FoundPlayer())
        {
            nowEnemy.SwitchState(NPCState.Chase);
        }
        if (Math.Abs(nowEnemy.transform.position.x - target.x) <= 0.1f && Math.Abs(nowEnemy.transform.position.y - target.y) <= 0.1f)
        {
            nowEnemy.ifwait = true;
            target = nowEnemy.GetNewPoint();
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
        nowEnemy.WaitTimeCounter();
    }
    public override void PhysicsUpdate()
    {
        if (!nowEnemy.ifwait && !nowEnemy.isDead && !nowEnemy.isHurt)
        {
            nowEnemy.rb.velocity = moveDir * nowEnemy.nowSpeed * Time.deltaTime;
            nowEnemy.anim.SetBool("run", false);
        }
        else
        {
            nowEnemy.rb.velocity = Vector2.zero;
            nowEnemy.anim.SetBool("run", false);
        }
    }
    public override void OnExit()
    {
        nowEnemy.anim.SetBool("run", true);
    }
}
