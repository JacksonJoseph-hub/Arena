using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public List<GameObject> coneHitEnemies;
    public PlayerInformation playerInfo;
    //public ParticleSystem shockwaveParticle;
    private EnemyInformation enemyInfo;
    private IEnumerator coroutine;

    [Header("Sounds")]
    public AudioSource audioControl;
    public AudioClip _a_attacksound;


    public float shockwaveRadius = 5.0f;
    public float shockwaveCooldown = 3.0f;

    private float basicAttackCooldownduration = 0.8f;
    private bool isBasicAttackOnCooldown = false;
    public bool isShockwaveOnCooldown = false;
    private void Awake()
    {
        //shockwaveParticle.Stop();
        audioControl = GetComponent<AudioSource>();
        playerInfo = GameObject.FindGameObjectWithTag("Player").GetComponentInParent<PlayerInformation>(); //Access info script to set effects/pass damage
    }
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isBasicAttackOnCooldown)
        {
            BasicConeAttack();
        }
        if(Input.GetKeyDown(KeyCode.Space) && !isShockwaveOnCooldown)
        {
            Debug.Log("Shockwave!");
            ShockwaveAttack();
        }
        if (Input.GetKeyDown(KeyCode.Space) && isShockwaveOnCooldown)
        {
            Debug.Log("Shockwave still on cooldown!");
        }
    }

    private void ShockwaveAttack()
    {
        float shockwaveAttackDamage = playerInfo.strength + playerInfo.protection * playerInfo.glory;
        if(playerInfo.IsEnraged())
        {
            shockwaveRadius *= 2;
        }
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, shockwaveRadius);
        foreach(Collider col in hitEnemies)
        {
            if(col.gameObject.CompareTag("Enemy"))
            {
                Debug.DrawRay(transform.position, col.transform.position - transform.position, Color.green, 2, false);
                enemyInfo = col.gameObject.GetComponent<EnemyInformation>();
                enemyInfo.TakeMeleeDamage(shockwaveAttackDamage);
            }
        }

        shockwaveRadius = 5;

        coroutine = ShockwaveCooldown(shockwaveCooldown);
        StartCoroutine(coroutine);
    }
    private void BasicConeAttack()
    {
        ConeAttackCheckList();
        int dmg = CalculateDamageDealt(playerInfo.strength * 2 + playerInfo.guile);

        audioControl.clip = _a_attacksound;
        audioControl.Play();

        foreach (GameObject target in coneHitEnemies)
        {
            Debug.DrawRay(transform.position, target.transform.position - transform.position, Color.green, 2, false);
            enemyInfo = target.gameObject.GetComponent<EnemyInformation>();
            enemyInfo.TakeMeleeDamage(dmg);
        }

        coroutine = BasicAttackCooldown(basicAttackCooldownduration);
        StartCoroutine(coroutine);
    }

    private int CalculateDamageDealt(float rawDamage)
    {
        float calculatedDamage;
        float critChance = playerInfo.guile + 1;
        bool didCrit = critChance > Random.Range(0, 100);

        if(didCrit)
        {
            calculatedDamage = rawDamage * 3;
            return Mathf.CeilToInt(calculatedDamage);
        }

        
        calculatedDamage = rawDamage;
        Debug.Log("calc damage: " + calculatedDamage);
        return Mathf.CeilToInt(calculatedDamage);
    }

    private void ConeAttackCheckList()
    {
        List<GameObject> temp = coneHitEnemies;
        if (temp.Count == 0) return;
        for (int i = 0; i < temp.Count; i++)
        {
            if (temp[i] == null || !Physics.Raycast(transform.position, temp[i].transform.position))
            {
                temp.RemoveAt(i);
            }
        }
        coneHitEnemies = temp;
        /*
        if (coneHitEnemies.Count == 0) return;
        for (int i = 0; i < coneHitEnemies.Count; i++)
        {
            if (coneHitEnemies[i] == null || !Physics.Raycast(transform.position, coneHitEnemies[i].transform.position))
            {
                Debug.Log("Count: " + coneHitEnemies.Count);
                coneHitEnemies[i] = coneHitEnemies[coneHitEnemies.Count - 1];
                coneHitEnemies.RemoveAt(coneHitEnemies.Count - 1);
                Debug.Log(coneHitEnemies.Count);
            }
        }
        */
    }
    private IEnumerator BasicAttackCooldown(float timer)
    {
        isBasicAttackOnCooldown = true;
        playerInfo.animControl.SetBool("IsAttacking", true);
        Debug.Log("Basic Cooldown started: " + Time.time);
        yield return new WaitForSeconds(timer);
        Debug.Log("Basic Cooldown ended: " + Time.time);
        playerInfo.animControl.SetBool("IsAttacking", false);
        isBasicAttackOnCooldown = false;
    }
    private IEnumerator ShockwaveCooldown(float timer)
    {
        isShockwaveOnCooldown = true;
        Debug.Log("Cooldown started: " + Time.time);
        yield return new WaitForSeconds(timer);
        Debug.Log("Cooldown ended: " + Time.time);
        isShockwaveOnCooldown = false;
    }
}