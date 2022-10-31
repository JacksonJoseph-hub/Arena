using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicArrow : MonoBehaviour
{

    private GameObject player;
    private Vector3 _target;
    private float _projectileSpeed;

    // Start is called before the first frame update
    void Start()
    {
        transform.LookAt(_target);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target, _projectileSpeed * Time.deltaTime);
        
        if(transform.position == _target)
        {
            Destroy(this.gameObject);
        }
    }

    public void SetTarget(Vector3 target, float projectileSpeed)
    {
        _target = target;
        _projectileSpeed = projectileSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponentInParent<PlayerInformation>().TakeMeleeDamage(15);
            Destroy(this.gameObject);
        }
    }
}
