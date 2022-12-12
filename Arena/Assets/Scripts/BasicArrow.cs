using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicArrow : MonoBehaviour
{

    private GameObject player;
    private Vector3 _target;
    private float _projectileSpeed;
    private float _projectileDamage;

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

    public void SetTarget(Vector3 target, float projectileSpeed, float projectileDamage)
    {
        _target = target;
        _projectileSpeed = projectileSpeed;
        _projectileDamage = projectileDamage;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Hit Check");
            collision.gameObject.GetComponentInParent<PlayerInformation>().TakeMeleeDamage(_projectileDamage);
            Destroy(this.gameObject);
        }
    }
}
