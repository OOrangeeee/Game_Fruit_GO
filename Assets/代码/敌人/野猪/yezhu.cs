using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class yezhu : enemy
{
    protected override void Awake()
    {
        base.Awake();
        patrolState=new yezhuPatrolState();
        chaseState=new yezhuChaseState();
    }
}
