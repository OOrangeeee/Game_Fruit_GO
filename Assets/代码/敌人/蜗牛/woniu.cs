using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class woniu : enemy
{
    protected override void Awake()
    {
        base.Awake();
        patrolState = new woniuPatrolState();
        skillState = new woniuSkillState();
    }
}
