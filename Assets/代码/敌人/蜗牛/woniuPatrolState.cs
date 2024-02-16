using UnityEngine;
public class woniuPatrolState : Base_State
{
    public override void OnEnter(enemy enemy)
    {
        nowEnemy = enemy;
        nowEnemy.nowWaitTime = nowEnemy.waitTime;
        nowEnemy.nowSpeed = nowEnemy.normalSpeed;
        nowEnemy.anim.SetBool("walk", true);
        nowEnemy.anim.SetBool("run", false);
    }
    public override void LogicUpdate()
    {
        if (nowEnemy.FoundPlayer())
        {
            nowEnemy.SwitchState(NPCState.Skill);
        }
        nowEnemy.GetFace();
        if (!nowEnemy.physicsCheck.isGround || (nowEnemy.physicsCheck.touchLeftWall && nowEnemy.faceDir.x < 0) || (nowEnemy.physicsCheck.touchRightWall && nowEnemy.faceDir.x > 0))
        {
            nowEnemy.ifwait = true;
            nowEnemy.anim.SetBool("walk", false);
            nowEnemy.anim.SetBool("run", false);
        }
        else if (!nowEnemy.ifwait)
        {
            nowEnemy.anim.SetBool("walk", true);
            nowEnemy.anim.SetBool("run", false);
        }
        nowEnemy.WaitTimeCounter();
    }
    public override void PhysicsUpdate()
    {
        if (!nowEnemy.isHurt && !nowEnemy.isDead && !nowEnemy.ifwait && !nowEnemy.anim.GetCurrentAnimatorStateInfo(0).IsName("普通蜗牛准备移动") && !nowEnemy.anim.GetCurrentAnimatorStateInfo(0).IsName("普通蜗牛从壳子里出来"))
            nowEnemy.Move();
    }
    public override void OnExit()
    {
        nowEnemy.anim.SetBool("walk", false);
    }
}
