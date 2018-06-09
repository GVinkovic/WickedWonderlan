using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonController : EnemyController {

    public GameObject FlameEffect;
    public Vector3 flameGroundRotation;
    public int hitAttackDistance = 12;
    public int biteAttackDistance = 10;

    private EnemyStats stats;
    private Enemy enemy;

    public override void Start()
    {
        enemyType = EnemyType.Dragon;
        base.Start();
        stats = GetComponent<EnemyStats>();
        enemy = GetComponent<Enemy>();
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

        FlameEffect.transform.localEulerAngles = flameGroundRotation;
        print("PD: " + enemy.GetPlayerDistance());

        var distance = enemy.GetPlayerDistance();

        if (distance > hitAttackDistance) return 2;
        if (distance > biteAttackDistance) return 1;

        return Random.Range(0, 2);
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
