using UnityEngine;

//[RequireComponent(typeof(Collider))]
public class CombatWeapon : MonoBehaviour {

    protected CharacterStats stats;

    public delegate void GameObjectAction(GameObject source, GameObject enemy);
    public GameObjectAction OnDamage;



    public virtual void Start()
    {
        if(!stats)
            stats = GetComponentInParent<CharacterStats>();

    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;
      //  Debug.Log("Collision with " + other.gameObject.name);
        var characterStats = other.gameObject.GetComponentInParent<CharacterStats>();
        if (characterStats)
        {
            //    bool isAttacking = false;
            //   if(characterStats.GetType() != stats.GetType())
            
            // ako nije tip lika super klasa lika kojeg napada i
            // ako nije tip lika nasljedija statove lika kojeg napada
            if(!stats.GetType().IsAssignableFrom(characterStats.GetType()) && !stats.GetType().IsSubclassOf(characterStats.GetType()))
            {
            //    print(stats.GetType() + " attacks with "+other.gameObject.name+", particle= " + other.gameObject.GetComponentInChildren<ParticleSystem>()!=null);
                if (stats.IsAttacking() || other.gameObject.GetComponentInChildren<ParticleSystem>())
                {
                    characterStats.TakeDamage(stats.GetDamageValue());
                    if (OnDamage != null) OnDamage.Invoke(gameObject, other.gameObject);
                }
            }
            
       
        }
    }
    public CharacterStats Stats
    {
        get
        {
            return stats;
        }

        set
        {
            stats = value;
        }
    }
  
}
