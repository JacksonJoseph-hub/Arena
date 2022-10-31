using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackConeCheck : MonoBehaviour
{
    // Start is called before the first frame update
    PlayerAttack pAttack;

    void Start()
    {
        pAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (Physics.Raycast(transform.position, other.transform.position))
            {
                if(!pAttack.coneHitEnemies.Contains(other.gameObject))
                {
                    pAttack.coneHitEnemies.Add(other.gameObject);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            pAttack.coneHitEnemies.Remove(other.gameObject);
        }
    }
}
