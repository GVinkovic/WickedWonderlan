using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCombatWeapon : CombatWeapon {

    private void OnParticleCollision(GameObject other)
    {
        OnTriggerEnter(other.GetComponent<Collider>());
    }
    
}
