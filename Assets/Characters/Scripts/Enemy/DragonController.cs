using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonController : EnemyController {

    public GameObject FlameEffect;

    private EnemyStats stats;
    public override void Start()
    {
        enemyType = EnemyType.Dragon;
        base.Start();
        stats = GetComponent<EnemyStats>();
        animator.SetFloat(AttackTypeFloat, 3);
    }

    void FlameAttackStart()
    {
        FlameEffect.SetActive(true);

    }
    void FlameAttackEnd()
    {
        FlameEffect.SetActive(false);
    }

    protected override int GetAttackIndex()
    {
        if (stats.CurrentHealth > 50)
        {
            return 3;
        }
        return 1;
    }

    public override void Hit()
    {
        if(animator.GetFloat(AttackTypeFloat)<3)
            base.Hit();
    }
    public override void NotfyHit()
    {
        base.NotfyHit();
        animator.SetFloat(AttackTypeFloat, GetAttackIndex());
    }
}
