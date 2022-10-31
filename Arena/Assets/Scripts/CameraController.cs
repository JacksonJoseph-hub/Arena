using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform playerTransform;

    private Vector3 offset;

    [Range(.01f, 1.0f)]
    public float smoothing = 0.5f;
    public float rotSpeed = 5.0f;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        offset = transform.position - playerTransform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(Input.GetMouseButton(1))
        {
            Quaternion camAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * rotSpeed, Vector3.up);
            offset = camAngle * offset;
        }
        
        Vector3 newPos = playerTransform.position + offset;

        
        transform.position = Vector3.Slerp(transform.position, newPos, smoothing);
        transform.LookAt(playerTransform);
    }
}
