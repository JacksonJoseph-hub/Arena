using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BasicMelee_Controller : MonoBehaviour
{
    //You may consider adding a rigid body to the zombie for accurate physics simulation
    private GameObject player;
    private PlayerInformation playerInfo;
    private EnemyInformation gruntInfo;
    private Vector3 playerPosition;
    private UnityEngine.AI.NavMeshAgent navAgent;

    //Time between attacks
    private float attackDelay;
    //Max distance of attack
    private float attackRange;

    private bool isAttacking = false;

    private float minAttackDamage;
    private float maxAttackDamage;




    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); //Assign player game object
        gruntInfo = gameObject.GetComponent<EnemyInformation>();

        attackDelay = gruntInfo.attackSpeed;
        attackRange = gruntInfo.attackRange;

        navAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        navAgent.speed = gruntInfo.movementSpeed;

        playerPosition = player.transform.position; //Track its position (for pathing/shooting)
        playerInfo = player.GetComponentInParent<PlayerInformation>(); //Access info script to set effects/pass damage
        
    }

    void Update()
    {
        playerPosition = player.transform.position;
        //navAgent.speed = movementSpeed;
        transform.LookAt(playerPosition);
        navAgent.SetDestination(playerPosition);


        if (InAttackRange() && !isAttacking)
        {
            minAttackDamage = gruntInfo.strength;
            maxAttackDamage = gruntInfo.strength * 10;
            StartCoroutine(Attack());
        }
    }
    private bool InAttackRange()
    {
        return Vector3.Distance(transform.position, playerPosition) <= attackRange;
    }

    IEnumerator Attack()
    {

        isAttacking = true;
        navAgent.isStopped = true;
        yield return new WaitForSeconds(attackDelay);
        if(InAttackRange())
            {
                playerInfo.TakeMeleeDamage(Mathf.FloorToInt(Random.Range(minAttackDamage,maxAttackDamage)));
                //playerInfo.SetNegativeStatusEffect(6.0f, 2, 2);
            }
        navAgent.isStopped = false;
        isAttacking = false;
    }

}
