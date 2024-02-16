using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bee : enemy
{
    [Header("ÒÆ¶¯·¶Î§")]
    public float partrolRadius;
    public Vector3 target;

    protected override void Awake()
    {
        base.Awake();
        patrolState = new beePatrolState();
        chaseState = new beeChaseState();
    }
    public override bool FoundPlayer()
    {
        var obj = Physics2D.OverlapCircle(transform.position, checkDistance, attackLayer);
        if (obj)
            attacker = obj.transform;
        return obj;
    }

    public override void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, checkDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, partrolRadius);
    }

    public override Vector3 GetNewPoint()
    {
        var targetX = Random.Range(-partrolRadius, partrolRadius);
        var targetY = Random.Range(-partrolRadius, partrolRadius);
        target= startPoint + new Vector3(targetX, targetY);
        return target;
    }
}
