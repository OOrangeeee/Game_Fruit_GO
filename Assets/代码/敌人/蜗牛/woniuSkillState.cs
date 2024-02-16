using UnityEngine;

public class woniuSkillState : Base_State
{
    private Vector2 originalColiderSize;
    private Vector2 originalColiderPosition;
    private float nowLife;
    public override void OnEnter(enemy enemy)
    {
        nowEnemy = enemy;
        nowEnemy.nowSpeed = 0;
        nowEnemy.anim.SetBool("walk", false);
        nowEnemy.anim.SetBool("run", false);
        nowEnemy.anim.SetBool("hide", true);
        nowEnemy.anim.SetTrigger("skill");
        nowLife = nowEnemy.GetComponent<Character>().nowLife;
        originalColiderSize = nowEnemy.capsuleCollider.size;
        originalColiderPosition = nowEnemy.capsuleCollider.offset;
        nowEnemy.capsuleCollider.size = new Vector2(1.34f, 1.14f);
        nowEnemy.capsuleCollider.offset = new Vector2(0.03f, 0.62f);
    }
    public override void LogicUpdate()
    {
        nowEnemy.FoundTimeCounter();
        if (nowEnemy.nowLostTime <= 0)
            nowEnemy.SwitchState(NPCState.Patrol);
        nowEnemy.GetFace();
        nowEnemy.GetComponent<Character>().nowLife = nowLife;
    }
    public override void PhysicsUpdate()
    {

    }
    public override void OnExit()
    {
        nowEnemy.anim.SetBool("walk", true);
        nowEnemy.anim.SetBool("run", false);
        nowEnemy.anim.SetBool("hide", false);
        nowEnemy.capsuleCollider.size = originalColiderSize;
        nowEnemy.capsuleCollider.offset = originalColiderPosition;
    }
}
