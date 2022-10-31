using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInformation : MonoBehaviour
{
    private PlayerController playerController; //Access for speed modifications
    public GameObject player;
    public HUDControl hdControl;
    [SerializeField] private Image HealthCircle; //Hp display tracker

    private IEnumerator coroutine;

    // Player determined stats
    [Header("Player Stats")]
    public int spirit = 5; // determines Max Health
    public float speed = 1.1f; // determines player movement speed
    public float glory = 1.1f; // determintes crowd effect
    public int strength = 1; // determines damage
    public int guile = 1; // determines attack speed / crit / usables
    public int protection = 2; //determines damage reduction
    public int divineFavor = 0; //determines luck

    // resource/health stats
    private float maxHealth;
    private float currentHealth;


    private float damageTakenModifier = 1.0f;
    private float damageDealtModifier = 1.0f;
    private float healingTakenModifier = 1.0f;

    // Temporary effects
    private float bleedDuration;
    private float bleedTick;
    private int bleedDamage;
    private bool isBleeding;

    private float poisonDuration;
    private float poisonTick;
    private int poisonDamage;
    private bool isPoisoned;

    private float slowDuration;
    private float slowSeverity;
    private bool isSlowed;

    private bool isEnraged;

    // Start is called before the first frame update

    private void Awake()
    {
        playerController = player.GetComponent<PlayerController>();
    }
    void Start()
    {
        maxHealth = spirit * 10;
        currentHealth = maxHealth;
        UpdateHealth();
        playerController.UpdatePlayerSpeed(speed);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            //Debug.Log("Player died");
        }
        
    }

    private void UpdateHealth()
    {
        HealthCircle.fillAmount = currentHealth / maxHealth; // Updates health bar graphic
        hdControl.UpdateHealthText(currentHealth, maxHealth); //Updates health text
    }
    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public void TakeHealing(float healing)
    {
        Debug.Log("Player heals for: " + healing);
        if(healing + currentHealth >= maxHealth)
            currentHealth = maxHealth;
        else
            currentHealth += healing * healingTakenModifier;
        UpdateHealth();
    }

    // Final step of damage processing 
    private void TakeDamage(float damage)
    {
        if(damage <= 0)
        {
            return;
        }
        float divineFavorCheck = Random.Range(0.0f, 100.0f);
        if (divineFavorCheck < divineFavor)
        {
            Debug.Log("Divine Intervention - damage prevented");
            return;
        }
        Debug.Log("Player takes: " + damage * damageTakenModifier + " damage and has " + currentHealth + " health remaining");
        currentHealth -= damage * damageTakenModifier;
        UpdateHealth();
    }

    public void TakeMeleeDamage(float damage)
    {
        TakeDamage(damage - guile - (3*protection));
    }


    public void SetPositiveStatusEffects(int type)
    {
        // Enrage
        if (type == 0)
        {
            isEnraged = true;
        }
    }
    public void Enrage(bool toggle)
    {
        if(toggle == true && !isEnraged)
        {
            isEnraged = toggle;
            damageDealtModifier += 0.5f;
            damageTakenModifier += 0.25f;
            Debug.Log("Player is enraged! dealt modifer: " + damageDealtModifier + ". Taken modifier: " + damageTakenModifier);
            return;
        }
        if (toggle == false && isEnraged)
        {
            damageDealtModifier -= 0.5f;
            damageTakenModifier -= 0.25f;
            Debug.Log("Player is no longer enraged! dealt modifer: " + damageDealtModifier + ". Taken modifier: " + damageTakenModifier);
            isEnraged = toggle;
        }
    }

    public bool IsEnraged()
    {
        return isEnraged;
    }

    // Deals with bleeding/poison/stunned/slowed and all future negative status effects
    public void SetNegativeStatusEffect(float duration, int damage, int type)
    {
        // Bleeding
        if (type == 0)
        {
            //Protection / already bleeding check
            if (damage - protection - divineFavor <= 0 || isBleeding)
            {
                return;
            }

            // Modify damage
            bleedDamage = damage - protection - divineFavor;
            bleedDuration = duration;

            //Calculate damage recieved every second for the duration
            bleedTick = Mathf.CeilToInt(bleedDamage / bleedDuration);
            Debug.Log("Tick: " + bleedTick + " damage " + bleedDamage + " dur " + bleedDuration);
            coroutine = PlayerBleeding(bleedTick, bleedDamage);
            StartCoroutine(coroutine);
        }

        // Poison
        if (type == 1)
        {
            //Protection / already poisoned check
            if (damage - protection - divineFavor <= 0 || isPoisoned)
            {
                return;
            }

            // Modify damage
            poisonDamage = damage - protection - divineFavor;
            poisonDuration = duration;

            //Calculate damage recieved every second for the duration
            poisonTick = Mathf.CeilToInt(poisonDamage / poisonDuration);
            Debug.Log("Poison Tick: " + poisonTick + " damage " + poisonDamage + " dur " + poisonDuration);
            coroutine = PlayerPoisoned(poisonTick, poisonDamage);
            StartCoroutine(coroutine);
        }
        // Slowed
        if (type == 2)
        {
            if (isSlowed)
            {
                return;
            }
            slowDuration = duration;
            slowSeverity = damage;
            coroutine = PlayerSlowed(slowSeverity, slowDuration);
            StartCoroutine(coroutine);
        }
    }



    //Bleeding timer
    private IEnumerator PlayerBleeding(float bleed, float totalDamage)
    {
        glory *= 2;

        while(totalDamage > 0 && !isBleeding ) //Check for bleeding status in case it is removed by other means (bandages/cures)
        { 
            yield return new WaitForSeconds(1);  // Damage is taken every second until status expires
            isBleeding = true;
            TakeDamage(bleed);
            totalDamage -= bleed;
            Debug.Log("Player takes " + bleed + " bleed damage and has " + currentHealth + " health. Bleed remaining: " + totalDamage + ". Time: " + Time.time);
        }
        glory /= 2;

        isBleeding = false;
    }


    //Poison timer
    private IEnumerator PlayerPoisoned(float poison, float totalDamage)
    {
        speed -= 0.2f;

        while (totalDamage > 0) //Check for poison status in case it is removed by other means (bandages/cures)
        {
            yield return new WaitForSeconds(1); // Damage is taken every second until status expires
            isPoisoned = true;
            TakeDamage(poison);
            totalDamage -= poison;
            Debug.Log("Player takes " + poison + " poison damage and has " + currentHealth + " health. poison remaining: " + totalDamage + ". Time: " + Time.time);
        }

        speed += 0.2f;

        isPoisoned = false;
    }

    //Slowed timer
    private IEnumerator PlayerSlowed(float severity, float duration)
    {
        isSlowed = true;
        float percent = severity / 100;
        Debug.Log("Player is slowed by " + percent + " %");
        speed /= severity;  // slow player
        playerController.UpdatePlayerSpeed(speed);
        yield return new WaitForSeconds(duration);
        speed *= severity;  // speed them back up
        playerController.UpdatePlayerSpeed(speed);
        isSlowed = false;
    }
}
