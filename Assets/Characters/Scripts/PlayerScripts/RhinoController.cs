using UnityEngine.AI;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(RhinoStats))]
public class RhinoController : MonoBehaviour {


    public float attackDistance = 3;


    protected int speedFloat = Animator.StringToHash("Speed");
    protected int deadTrigger = Animator.StringToHash("Dead");
    protected int respawnTrigger = Animator.StringToHash("Respawn");
    protected int attackTrigger = Animator.StringToHash("Attack");
    protected int AttackState = Animator.StringToHash("Attack");

    private Animator animator;
    private GameObject target;
    private RhinoStats rhinoStats;
    private NavMeshAgent navMeshAgent;

    private bool dead = false;
    private bool attack = false;

    private float defaultStoppingDistance = 0;
	void Start () {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        rhinoStats = GetComponent<RhinoStats>();

        defaultStoppingDistance = navMeshAgent.stoppingDistance;

        target = PlayerManager.Player;

        StartCoroutine(TrackTarget());

        PlayerManager.RegisterRhino(this);

    }
	
    IEnumerator TrackTarget()
    {
        while (true)
        {
            var distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
            float speed = 0;
            if (distanceToTarget > navMeshAgent.stoppingDistance)
            {
             
                navMeshAgent.SetDestination(target.transform.position);

                speed = distanceToTarget / navMeshAgent.stoppingDistance * 2;
                if (speed > 30) speed = 30;

                navMeshAgent.speed = speed;
                animator.SetFloat(speedFloat, speed/2);
                
            }
            else
            {
                animator.SetFloat(speedFloat, 0);
                if(attack) Attack();
                
            }
            yield return new WaitForSeconds(.5f);
        }
    }

    public void SetTarget(GameObject targetToFollow, bool attackTarget)
    {
        if (!attackTarget)
        {
            Revive();
            navMeshAgent.stoppingDistance = defaultStoppingDistance;
        }
        else
        {
            navMeshAgent.stoppingDistance = targetToFollow.transform.lossyScale.x * targetToFollow.GetComponent<NavMeshAgent>().radius * 3;
        }
        this.target = targetToFollow;
        this.attack = attackTarget;
//        print("Following target " + targetToFollow.name);
    }

    public void Revive()
    {
        if (dead)
        {
            animator.SetTrigger(respawnTrigger);
            dead = false;
            rhinoStats.CurrentHealth = rhinoStats.MaxHealth;
        }
    }

    public bool IsAttacking()
    {
        return animator.GetCurrentAnimatorStateInfo(0).shortNameHash == AttackState;
    }
    public void Die()
    {
        if (!dead)
        {
            dead = true;
            animator.SetTrigger(deadTrigger);
        }
    }
    public void Attack()
    {

        if(!IsAttacking())
        {
            animator.SetTrigger(attackTrigger);
        }
    }
    public bool Attacks
    {
        get { return attack; }
    }
	
}
