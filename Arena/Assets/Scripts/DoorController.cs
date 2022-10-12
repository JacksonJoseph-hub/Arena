using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Vector3 closedPosition;
    public Vector3 openPosition;
    private Vector3 currentTarget;
    private IEnumerator coroutine;
    private static float movementSpeed = 1.4f;
    private bool doorClosed = true;
    private bool enemyWaiting = false;
    private bool onCooldown = false;
    private float doorOpenerRecoveryPeriod = 15.0f;

    // Start is called before the first frame update
    void Start()
    {
        closedPosition = transform.position;
        openPosition = transform.position + new Vector3(0, 8, 0);
        currentTarget = openPosition;

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Distance: " + Vector3.Distance(currentTarget, transform.position) + currentTarget );
        //Debug.Log("Enemy Waiting?: " + enemyWaiting + " Door Closed?: " + doorClosed + "Current Target?: " + currentTarget);
        if (currentTarget == openPosition && doorClosed && enemyWaiting && !onCooldown) // If moving towards open door
        {
            transform.position = Vector3.MoveTowards(transform.position, currentTarget, movementSpeed * Time.deltaTime);
            //Debug.Log("Moving towards open");
            if (Vector3.Distance(currentTarget,transform.position) <= .2f)
            {
                doorClosed = false;
                enemyWaiting = false;
                currentTarget = closedPosition;
                coroutine = DoorCooldown();
                StartCoroutine(coroutine);
            }    
        }
        if (currentTarget == closedPosition && !onCooldown) // if moving towards closed door
        {
            transform.position = Vector3.MoveTowards(transform.position, currentTarget, movementSpeed * Time.deltaTime);
            //Debug.Log("Moving towards close");
            if (Vector3.Distance(currentTarget, transform.position) <= .2f)
            {
                doorClosed = true;
                currentTarget = openPosition;
                coroutine = DoorCooldown();
                StartCoroutine(coroutine);
            }
        }
        if(transform.position == openPosition)
        {
            currentTarget = closedPosition;
        }

        
    }
    /*
    private bool IsDoorOpen()
    {
        if(gameObject.transform.position == openPosition.transform.position)
        {
            enemyWaiting = false; // Reset the bool
            isMoving = false;
            return true;
        }
        return false;
    }
    private bool IsDoorClosed()
    {
        if (gameObject.transform.position == closedPosition.transform.position)
        {
            isMoving = false; // Reset the bool
            return true;
        }
        return false;
    }
    private void OpenDoor()
    {
        isMoving = true;
        transform.position = Vector3.MoveTowards(transform.position, openPosition.position, movementSpeed * Time.time);
    }
    private void CloseDoor()
    {
        isMoving = true;
        transform.position = Vector3.MoveTowards(transform.position, closedPosition.position, movementSpeed * Time.time);
    }
    */
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.root.CompareTag("Enemy"))
        {
            enemyWaiting = true;
            //Debug.Log("Enemy Behind Door - OPEN!!");
        }
    }

    private IEnumerator DoorCooldown()
    {
        //Debug.Log("Door cooldown starts at " + Time.time);
        onCooldown = true;
        yield return new WaitForSeconds(doorOpenerRecoveryPeriod);
        //Debug.Log("Door cooldown ends at " + Time.time);
        onCooldown = false;
    }
}
