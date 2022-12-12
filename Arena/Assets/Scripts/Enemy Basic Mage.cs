using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasicMage : MonoBehaviour
{ 
    public GameObject arrow;
    private GameObject player;
    public Transform firePosition;
    private Vector3 playerPosition;
    private EnemyInformation gruntInfo;
    private UnityEngine.AI.NavMeshAgent navAgent;

    //Minimum distance before firing
    public float minShootRange = 5.0f;
    public float maxShootRange = 25.0f;

    //Time between attacks
    public float attackDelay;
    //Max distance of attack
    public float attackRange;
    public float projectileSpeed;
    private float nextShot;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gruntInfo = gameObject.GetComponent<EnemyInformation>();

        attackDelay = gruntInfo.attackSpeed;
        attackRange = gruntInfo.attackRange;
        projectileSpeed = gruntInfo.projectileSpeed;

        nextShot = Time.time + attackDelay;
        navAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();

        navAgent.speed = gruntInfo.movementSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        Skirmish();
    }

    // Skirmish logic ->
    // If too far away -> move towards player
    // If too close to player - > run away
    // If neither true && attack is off cooldown -> shoot player
    private void Skirmish()
    {
        transform.LookAt(playerPosition); //Rotate object to look at player
        playerPosition = player.transform.position; //Update player position


        if (!InRange()) // Check if the player is too far away - true = move towards player
        {
            navAgent.isStopped = false;
            navAgent.SetDestination(playerPosition);
        }
        else if (RunAway()) // Check if the player is too far away - true = move towards player
        {
            Vector3 direction = transform.position - playerPosition; //Sets direction to opposite way of player
            Vector3 moveTo = transform.position + direction * 2;
            navAgent.isStopped = false;
            navAgent.SetDestination(moveTo);
        }
        else if (AttackCooldownCheck() && !RunAway()) // Check if the player can shoot (not moving, attack not on cooldown)
        {
            navAgent.isStopped = true;
            ShootArrow();
        }
    }

    private bool InRange()
    {
        return DistanceCheck() <= maxShootRange;
    }

    private bool AttackCooldownCheck()
    {
        return Time.time > nextShot;
    }

    private bool RunAway()
    {
        return DistanceCheck() <= minShootRange;
    }

    private float DistanceCheck()
    {
        return Vector3.Distance(gameObject.transform.position, playerPosition);
    } // Returns distance between object and player
    private void ShootArrow()
    {
        GameObject tempArrow = Instantiate(arrow, firePosition.position, Quaternion.LookRotation(playerPosition));
        //tempArrow.GetComponent<BasicArrow>().SetTarget(playerPosition, projectileSpeed, projectileDamage);

        nextShot = Time.time + attackDelay;
    }
}

