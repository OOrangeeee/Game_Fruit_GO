using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class yezhuChaseState : Base_State
{

    public override void OnEnter(enemy enemy)
    {
        nowEnemy = enemy;
        nowEnemy.nowSpeed = nowEnemy.chaseSpeed;
        nowEnemy.anim.SetBool("run", true);
        nowEnemy.anim.SetBool("walk", false);
    }
    public override void LogicUpdate()
    {
        nowEnemy.FoundTimeCounter();
        if (nowEnemy.nowLostTime <= 0)
            nowEnemy.SwitchState(NPCState.Patrol);
        nowEnemy.GetFace();
        if (!nowEnemy.physicsCheck.isGround || (nowEnemy.physicsCheck.touchLeftWall && nowEnemy.faceDir.x < 0) || (nowEnemy.physicsCheck.touchRightWall && nowEnemy.faceDir.x > 0))
        {
            nowEnemy.transform.localScale = new Vector3(nowEnemy.faceDir.x, 1, 1);
        }
        else
        {
            nowEnemy.anim.SetBool("run", true);
            nowEnemy.anim.SetBool("walk", false);
        }
    }
    public override void PhysicsUpdate()
    {
        if (!nowEnemy.isHurt && !nowEnemy.isDead && !nowEnemy.ifwait)
            nowEnemy.Move();

    }
    public override void OnExit()
    {
        nowEnemy.anim.SetBool("run", false);
    }
}
