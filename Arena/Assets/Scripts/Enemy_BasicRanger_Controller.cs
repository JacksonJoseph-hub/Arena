using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy_BasicRanger_Controller : MonoBehaviour
{
    public GameObject arrow;
    private GameObject player;
    public Transform firePosition;
    private Vector3 playerPosition;
    private UnityEngine.AI.NavMeshAgent navAgent;




    //This will be the enemy speed. Adjust as necessary.
    public float movementSpeed = 1.5f;
    public bool isMoving;

    //Minimum distance before firing
    public float minShootRange = 5.0f;
    public float maxShootRange = 25.0f;

    //Time between attacks
    public float attackDelay = 1.7f;
    //Max distance of attack
    public float attackRange = 15.0f;
    public float projectileSpeed = 3.0f;
    private float nextShot;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        nextShot = Time.time + attackDelay;
        navAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        isMoving = false;

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


        if(!InRange()) // Check if the player is too far away - true = move towards player
        {
            Debug.Log("Moving towards player");
            MoveTowardsPlayer(); 
        }     
        else if(DistanceCheck() <= minShootRange) // Check if the player is too far away - true = move towards player
        {
            RunAway();
        }
        else if (AttackCooldownCheck()) // Check if the player can shoot (not moving, attack not on cooldown)
        {
            Debug.Log("Can shoot = true");
            //RunAway();
            ShootArrow();     
        }
    }
    
    private bool InRange()
    {
        
        if(DistanceCheck() <= maxShootRange)
        {
            Debug.Log("In Range");
            return true;
        }
        Debug.Log("Out of Range");
        return false;
    }

    private void MoveTowardsPlayer()
    {
        isMoving = true;
        transform.position = Vector3.MoveTowards(transform.position, playerPosition, movementSpeed * Time.deltaTime);

    }
    private bool AttackCooldownCheck()
    {
        return Time.time > nextShot;
    }

    private bool CanShoot()
    {
        if (DistanceCheck() >= minShootRange && !isMoving && AttackCooldownCheck())
        {
            return true;
        }
        return false;
    }
    private void RunAway()
    {
        while(DistanceCheck() <= minShootRange)
        {
            isMoving = true;
        }
        isMoving = false;
    }

    private float DistanceCheck()
    {
        return Vector3.Distance(gameObject.transform.position, playerPosition);
    } // Returns distance between object and player
    private void ShootArrow()
    {
        GameObject tempArrow = Instantiate(arrow, firePosition.position, Quaternion.LookRotation(playerPosition));
        tempArrow.GetComponent<BasicArrow>().SetTarget(playerPosition, projectileSpeed);

        nextShot = Time.time + attackDelay;
    }
}
