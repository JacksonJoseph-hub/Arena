using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private Transform cameraT;
    int floorMask;
    float camRayLength = 100f;
    Vector3 moverDir;

    [Range(1, 5)]
    private float playerMovementSpeed = 1.1f;
    // Start is called before the first frame update
    void Awake()
    {
        cameraT = Camera.main.transform;
        playerRb = GetComponent<Rigidbody>();
        floorMask = LayerMask.GetMask("Floor");
    }

    void FixedUpdate()
    {

        float VertInput = Input.GetAxis("Vertical");
        float HorInput = Input.GetAxis("Horizontal");

        Vector3 direction = new Vector3(HorInput, 0f, VertInput).normalized;
        
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
            moverDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        }

        if (Input.GetKey(KeyCode.S))
            playerRb.AddForce(-moverDir*playerMovementSpeed);
        ///*
        if (Input.GetKey(KeyCode.D))
            playerRb.AddForce(moverDir*playerMovementSpeed);
        if (Input.GetKey(KeyCode.A))
            playerRb.AddForce(moverDir * playerMovementSpeed);
        if (Input.GetKey(KeyCode.W))
            playerRb.AddForce(moverDir * playerMovementSpeed * 2);

        // Turn the player to face the mouse cursor.
        Turning();

    }


    void Turning()
    {
        // Create a ray from the mouse cursor on screen in the direction of the camera.
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Create a RaycastHit variable to store information about what was hit by the ray.
        RaycastHit floorHit;
        // Perform the raycast and if it hits something on the floor layer...
        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            // Create a vector from the player to the point on the floor the raycast from the mouse hit.
            Vector3 playerToMouse = floorHit.point - transform.position;

            // Ensure the vector is entirely along the floor plane.
            playerToMouse.y = 0f;

            // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

            // Set the player's rotation to this new rotation.
            playerRb.MoveRotation(newRotation);
        }
    }

    public void UpdatePlayerSpeed(float speed)
    {
        playerMovementSpeed = speed;
    }

}
