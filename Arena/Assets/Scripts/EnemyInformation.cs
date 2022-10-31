using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInformation : MonoBehaviour
{
    private HUDControl hudControl;
    private PlayerAttack playerAttackScript;
    [Header("Enemy Stats")]
    public int spirit = 5; // determines Max Health
    public float speed = 1.1f; // determines player movement speed
    public int strength = 1; // determines damage
    public int guile = 1; // determines attack speed / crit / usables
    public int protection = 2; //determines damage reduction
    public int scoreValue;
    public float attackSpeed;
    public float attackRange;
    public float projectileSpeed;
    //How difficult this enemy prefab is, higher is harder
    public int difficultyThreshold; 

    private int totalStatValue;

    // resource/health stats
    private float maxHealth;
    private float currentHealth;


    private float damageTakenModifier = 1.0f;


    
    // Start is called before the first frame update
    void Start()
    {
        hudControl = GameObject.FindGameObjectWithTag("UI").GetComponent<HUDControl>();
        maxHealth = spirit * 10;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //CheckifDead();
    }
    private void TakeDamage(float damage)
    {
        if (damage <= 0)
        {
            return;
        }

        currentHealth -= damage * damageTakenModifier;
        CheckifDead();
        Debug.Log(this.gameObject.name + " takes: " + damage * damageTakenModifier + " damage and has " + currentHealth + " health remaining");

    }

    private void CheckifDead()
    {
        if(currentHealth <= 0)
        {
            Debug.Log("Destroying: " + this.gameObject.name);
            totalStatValue = spirit + strength + spirit + guile + protection;
            scoreValue = totalStatValue * 3;
            hudControl.ModifyPlayerScore(scoreValue);
            Debug.Log("Score +" + scoreValue);
            Destroy(this.gameObject);
        }
    }
    public void TakeMeleeDamage(float damage)
    {
        TakeDamage(damage - guile - protection);
    }

    public int ReturnDifficultyLevel()
    {
        return difficultyThreshold;
    }
}
