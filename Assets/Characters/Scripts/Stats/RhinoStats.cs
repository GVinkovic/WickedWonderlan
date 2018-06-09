using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhinoStats : PlayerStats {

    private RhinoController controller;

    private void Start()
    {
        controller = GetComponent<RhinoController>();   
    }

    public override void TakeHit()
    {

    }

    public override void Die()
    {
        controller.Die();  
    }

    public override bool IsAttacking()
    {
        return controller.IsAttacking();
    }

    public override int GetDamageValue()
    {
        return damage.Value;
    }

}
