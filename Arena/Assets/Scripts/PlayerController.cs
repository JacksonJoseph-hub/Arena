using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;


    [Range(1, 5)]
    private float playerMovementSpeed = 1.1f;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }



    // Update is called once per frame
    void Update()
    {

    }
    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
            Debug.Log("Left Mouse Click");
        if (Input.GetMouseButtonDown(1))
            Debug.Log("Right Mouse Click");
        if (Input.GetKey(KeyCode.D))
            playerRb.AddForce(new Vector3(playerMovementSpeed, 0, 0));
        if (Input.GetKey(KeyCode.A))
            GetComponent<Rigidbody>().AddForce(new Vector3(-playerMovementSpeed, 0, 0));
        if (Input.GetKey(KeyCode.W))
            GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, playerMovementSpeed));
        if (Input.GetKey(KeyCode.S))
            GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, -playerMovementSpeed));
    }
    public void updatePlayerSpeed(float speed)
    {
        playerMovementSpeed = speed;
    }

}
