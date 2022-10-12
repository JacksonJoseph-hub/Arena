using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BasicMelee_Controller : MonoBehaviour
{
    //You may consider adding a rigid body to the zombie for accurate physics simulation
    private GameObject player;
    private PlayerInformation playerInfo;
    private Transform playerPosition;

    //This will be the enemy speed. Adjust as necessary.
    public float movementSpeed = 1.5f;

    //Time between attacks
    public float attackDelay = 1.7f;
    //Max distance of attack
    public float attackRange = 5.0f;

    private bool isAttacking = false;

    private float minAttackDamage = 3;
    private float maxAttackDamage = 18;



    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player"); //Assign player game object
        playerPosition = player.transform; //Track its position (for pathing/shooting)
        playerInfo = player.GetComponentInParent<PlayerInformation>(); //Access info script to set effects/pass damage
    }
    void Start()
    {

    }

    void Update()
    {
        transform.LookAt(playerPosition);
        if (InAttackRange() && !isAttacking)
        {
            StartCoroutine(Attack());
        }
        else
            MoveTowardsPlayer();
    }
    private bool InAttackRange()
    {
            if (Vector3.Distance(transform.position, playerPosition.position) >= attackRange)
                return false;
            else
                return true;
    }
    private void MoveTowardsPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, playerPosition.position, movementSpeed * Time.deltaTime);
    }

    IEnumerator Attack()
    {

        isAttacking = true;
        movementSpeed = movementSpeed / 5;
        yield return new WaitForSeconds(attackDelay);
        if(InAttackRange())
            {
                playerInfo.TakeMeleeDamage(Mathf.FloorToInt(Random.Range(minAttackDamage,maxAttackDamage)));
                //playerInfo.SetNegativeStatusEffect(6.0f, 2, 2);
            }
        movementSpeed = movementSpeed * 5; 
        isAttacking = false;
    }

}
