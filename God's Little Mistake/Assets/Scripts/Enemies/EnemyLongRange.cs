using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLongRange : GameBehaviour
{

    [Header ("Enemy Navigation")]
    bool projectileShot;
    public GameObject firingPoint;
    public GameObject player;
    public NavMeshAgent agent;


    public float sightRange = 7;
    public float attackRange;

    bool canAttack;
    bool canSee;



    ////patrolling
    public Vector3 walkPoint;
    public float walkPointRange;

    //public bool playerInSightRange, playerInAttackRange, enemyInHitRange;
    //public bool isPatrolling;

    public LayerMask whatIsGround, whatIsPlayer;


    BaseEnemy enemyStats;
    BaseEnemy BaseEnemy;

    Vector3 target;

    //wowo
    // Start is called before the first frame update
    void Start()
    {
        //enemyRange = _ED.enemies[0].range;
        //enemySightRange = _ED.enemies[0].range + 0.5f;
        //player = GameObject.Find("Player");
        enemyStats = GetComponent<BaseEnemy>();
        BaseEnemy = GetComponent<BaseEnemy>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");

        attackRange = enemyStats.stats.range;
        target = SearchWalkPoint();

    }

    // Update is called once per frame
    void Update()
    {

        ////check for the sight and attack range
        if (BaseEnemy.enemyState != BaseEnemy.EnemyState.Die)
        {
            canSee = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            canAttack = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

            //if cant see player, patrol
            if(!canSee) BaseEnemy.enemyState = BaseEnemy.EnemyState.Patrolling;
            else if (canSee) BaseEnemy.enemyState = BaseEnemy.EnemyState.Chase;

            //playerInAttackRange = Physics.CheckSphere(transform.position, enemyStats.stats.range, whatIsPlayer);
            //playerInSightRange = Physics.CheckSphere(transform.position, enemyStats.stats.range + 1, whatIsPlayer);
            //if (playerInAttackRange) BaseEnemy.enemyState = BaseEnemy.EnemyState.Attacking;
            //if (!playerInAttackRange) BaseEnemy.enemyState = BaseEnemy.EnemyState.Patrolling;

        }

        //Visual indicator for health
        //HealthVisualIndicator(enemyStats.stats.health, enemyStats.stats.maxHP);


        firingPoint.transform.LookAt(player.transform.position);


        switch (BaseEnemy.enemyState)
        {
            case BaseEnemy.EnemyState.Patrolling:



                if (!agent.pathPending)
                {
                    if (agent.remainingDistance <= agent.stoppingDistance)
                    {
                        if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                        {
                            // Destination reached

                            //new target
                            target = SearchWalkPoint();
                        }
                    }
                }


                agent.SetDestination(target);

                break;
            case BaseEnemy.EnemyState.Chase:

                if (Vector3.Distance(player.transform.position, gameObject.transform.position) > 5)
                {

                    agent.isStopped = false;

                    agent.SetDestination(player.transform.position);

                }
                else if (Vector3.Distance(player.transform.position, gameObject.transform.position) < 4)
                {

                    agent.isStopped = false;
                    Vector3 toPlayer = player.transform.position - transform.position;
                    Vector3 targetPosition = toPlayer.normalized * -5f;

                    agent.SetDestination(targetPosition);


                }
                else
                {

                    agent.isStopped = true;

                }

                // ATTACK

                if(canAttack)
                {
                    FireProjectile(enemyStats.stats.projectilePF, enemyStats.stats.projectileSpeed, enemyStats.stats.fireRate, enemyStats.stats.range);
                }


                break; 
        }




        //enemy to patrol

        //if player is in sight range of player, chase

        //if player is in attack range, shoot, CAN BE CHASING AND SHOOT AT THE SAME TIME



        //switch(BaseEnemy.enemyState)
        //{
        //    case BaseEnemy.EnemyState.Patrolling:
        //        //if(isPatrolling != true)
        //        //{
        //        //    isPatrolling = true;
        //        //    StartCoroutine(PatrolingIE());
        //        //    walkPointRange = 5;
        //        //}
        //        break;
        //    case BaseEnemy.EnemyState.Chase:





        //        break;
        //    case BaseEnemy.EnemyState.Attacking:
        //        //isPatrolling = false;
        //        //walkPointRange = 2;
        //        //FireProjectile(enemyStats.stats.projectilePF, enemyStats.stats.projectileSpeed, enemyStats.stats.fireRate, enemyStats.stats.range);
        //        break;
        //    case BaseEnemy.EnemyState.Die:
        //        //StopCoroutine(PatrolingIE());
        //        ////death animation etc
        //        //print("Dead");
        //        //BaseEnemy.Die();
        //        break;
        //}

        }

    void Patroling()
    {

        var target = SearchWalkPoint();


        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    // Destination reached

                    //new target
                    target = SearchWalkPoint();
                }
            }
        }

        agent.SetDestination(target);
        //    var destination = SearchWalkPoint();

        //    agent.SetDestination(destination);

        //    if(Vector3.Distance(transform.gameObject))

        //    yield return new WaitForSeconds(Random.Range(2, 6));
        //    StartCoroutine(PatrolingIE());
        //}
    }

    private Vector3 SearchWalkPoint()
    {

        return transform.position + Random.insideUnitSphere * walkPointRange;

    }


    void FireProjectile(GameObject _prefab, float _projectileSpeed, float _firerate, float _range)
    {

        if (!projectileShot)
        {

            //Spawn bullet and apply force in the direction of the mouse
            //Quaternion.LookRotation(flatAimTarget,Vector3.forward);
            GameObject bullet = Instantiate(_prefab, firingPoint.transform.position, firingPoint.transform.rotation);
            bullet.GetComponent<EnemyProjectile>().dmg = enemyStats.stats.dmg;
            bullet.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * _projectileSpeed);

            Mathf.Clamp(bullet.transform.position.y, 0, 0);

            //This will destroy bullet once it exits the range, aka after a certain amount of time
            Destroy(bullet, _range);

            //Controls the firerate, player can shoot another bullet after a certain amount of time
            projectileShot = true;
            ExecuteAfterSeconds(_firerate, () => projectileShot = false);
        }
    }


    //visualise sight range
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(player.transform.position, 5);
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawWireSphere(transform.position, enemyStats.stats.range+1);



    }   
}
