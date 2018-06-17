using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyStats))]
[RequireComponent(typeof(EnemyController))]
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : Interactable {

    public float attackSpeed = 0.2f;

    private EnemyController controller;
    private NavMeshAgent navAgent;

    private GameObject onDeathParticle;

    private bool interacting = false;
    private float attackCooldown = 0f;

    private EnemyStats enemyStats;
    private ProgressBar healthBar;
    private Vector3 initialPosition;
    private bool isPlayer = true;

    // za questove type enemy-a
    // dodat dodatne po po potrebi
    public enum Type
    {
        Elemental = 0,
        Goblin = 1,
        GoblinBoss = 2,
        Monster = 3,
        Troll = 4,
        Spider = 5,
        Dragon = 6
    }
    public Type enemyType;

    public Vector3 DyingPosition { get; set; }
    public override void Start()
    {
        base.Start();
        controller = GetComponent<EnemyController>();
        navAgent = GetComponent<NavMeshAgent>();
        healthBar = GameManager.GetEnemyHealthBarCopy(gameObject);
        enemyStats = GetComponent<EnemyStats>();
        initialPosition = transform.position;
     //   navAgent.isStopped = true;
    }
    private void Update()
    {
        if (interacting && navAgent.enabled)
        {
            if (controller.CanAttack())
            {
                attackCooldown -= Time.deltaTime*10;

                if (GetPlayerDistance() < navAgent.stoppingDistance)
                {
                    if (attackCooldown <= 0)
                    {
                        Attack();
                        attackCooldown = 1f / attackSpeed;
						if (Random.Range (0, 3) == 0) {
							gameObject.GetComponent<AudioController> ().Play ("EnemyAttack");
						}

						if (Random.Range (0, 15) == 0) {
							gameObject.GetComponent<AudioController> ().Play ("Grawl");
						}
					
                    }
                    else FaceTarget();
                }
                else
                {
                    navAgent.SetDestination(player.transform.position);
                }
            }
        }
        controller.SetSpeed(navAgent.velocity.magnitude);

    }
    public override void Interact()
    {
        base.Interact();
        PlayerManager.UnderEnemyAttack(this);

        interacting = true;

			gameObject.GetComponent<AudioController> ().Play ("Grawl");
		
       // navAgent.isStopped = false;
        //  Attack();

    }
    public override void StopInteract()
    {
        base.StopInteract();
        PlayerManager.EnemyStoppedAttack(this);
        interacting = false;
        //  if(navAgent.enabled) navAgent.isStopped = true;
        //controller.SetSpeed(0);

        if(navAgent.enabled)
            //vraca se na inicijalnu poziciju ako nije umra
            navAgent.SetDestination(initialPosition);

    }
    public void Attack()
    {
        FaceTarget();
        controller.Attack();
    }
    void FaceTarget()
    {
        //  Vector3 direction = (player.transform.position - transform.position).normalized;
        //   Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.y));
        //        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime );
        // transform.rotation.SetLookRotation(player.transform.position);
        //navAgent.SetDestination(player.transform.position);
        //   transform.LookAt(player.transform);
        // transform.rotation.SetLookRotation(player.transform.position);

        Vector3 targetDir = player.transform.position - transform.position;
        // The step size is equal to speed times frame time.
        float step = 3   * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
      //  Debug.DrawRay(transform.position, newDir, Color.red);
        // Move our position a step closer to the target.
        transform.rotation = Quaternion.LookRotation(newDir);
    }
    public void Died()
    {
        controller.Die();
        healthBar.SetProgress(0);

        DyingPosition = transform.position;

        StopInteract();
        navAgent.enabled = false;
        var colider = GetComponentInChildren<BoxCollider>();
        colider.size = new Vector3(colider.size.x, colider.size.x/2, colider.size.z);

        Invoke("RemoveEnemy", 3);
        
        Invoke("DeathParticle", 2);
		gameObject.GetComponent<AudioController> ().Play ("Death");
    }
    void DeathParticle()
    {
        onDeathParticle = Instantiate(GameManager.instance.OnEnemyDeathParticle, null);
        onDeathParticle.transform.position = DyingPosition;

        onDeathParticle.SetActive(true);
        Invoke("RemoveOnDeathParticle",3);

    }
    void RemoveOnDeathParticle()
    {

        Destroy(onDeathParticle);
        Destroy(gameObject);

    }
    void RemoveEnemy()
    {
        gameObject.SetActive(false);
        // samo ako se je bori s likom obavjesti GameManeger-a
        if(isPlayer)GameManager.EnemyDied(this);
    }
    public void Knocback()
    {
        controller.KnockBack();
    }
    public void TakeHit()
    {
        if (!interacting) Interact();

        // samo ako ga player napada onda animacije 
        if (isPlayer)
        {
            // ako trči prema igraču onda neće play-at animacije udara da ga ne usporava
            if (GetPlayerDistance() < navAgent.stoppingDistance)
            {
                controller.Hit();
            }
            else
            {
                controller.NotfyHit();
            }

        }
        healthBar.SetProgress(enemyStats.CurrentHealth);

    }

    public EnemyController EnemyController
    {
        get { return controller; }
    }
    public bool IsAttacking()
    {
        return controller.IsAttacking();
    }


    public void SetTarget(GameObject target, bool isPlayer)
    {
        player = target;
        this.isPlayer = isPlayer;
    }

}
