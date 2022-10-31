using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerFollower : MonoBehaviour
{
    [SerializeField]
    private GameObject effectSphere;
    private Vector3 scaleEffectSphere;
    private GameObject player;
    private PlayerInformation playerInfo;


    [SerializeField]
    private Material _m_Enrage;
    [SerializeField]
    private Material _m_Healing;

    private IEnumerator coroutine; // To use while tower is active timers
    public float effectRadius;
    public float effectDuriation;
    private float effectDuriationTimer;
    private bool activeEffect = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerInfo = player.GetComponentInParent<PlayerInformation>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public bool IsActiveTower()
    {
        return activeEffect;
    }
    private bool CheckForPlayer()
    {

        /*
        return(Vector3.Distance(effectSphere.transform.position, player.transform.position) <= effectRadius)
        */
        //*
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, effectRadius);
        foreach (Collider c in hitColliders)
        {
            if (c.CompareTag("Player"))
            {
                
                return true;
            }
        }  
        return false;
       // */
    }

    public void FlipActiveStatus()
    {
        activeEffect = !activeEffect;
    }

    public void SetActiveTowerParameters(int effectType, float duration, float radius)
    {
        //Debug.Log("Step 1: Effect type: " + effectType + " Duration: " + duration + " Radius: " + radius);
        effectRadius = radius;
        effectDuriation = duration;

        effectSphere.transform.localScale += new Vector3(effectRadius/8, effectRadius/8, effectRadius/8);
        switch (effectType)
        {
            case 0:
                effectSphere.GetComponent<MeshRenderer>().material = _m_Enrage;
                coroutine = EnrageEffect();
                StartCoroutine(coroutine);
                break;
            case 1:
                effectSphere.GetComponent<MeshRenderer>().material = _m_Healing;
                coroutine = HealingEffect();
                StartCoroutine(coroutine);
                break;
            default:
                Debug.LogError("Tower SetActiveTowerParameters failed");
                break;
        }
    }

    
    private IEnumerator EnrageEffect()
    {
        while (effectDuriation >= 0) //Check for poison status in case it is removed by other means (bandages/cures)
        {

            // How this should work:
            // Radius check at begin/end of a 1 sec period, if true, player keeps enrage effect

            playerInfo.Enrage(CheckForPlayer());

            effectDuriation -= 1.0f;
            yield return new WaitForSeconds(1); // Damage is taken every second until status expires

            playerInfo.Enrage(CheckForPlayer());

        }

        // Once duriation ends, turn enrage off
        playerInfo.Enrage(false);

        // Once the effect ends, flip the tower off
        FlipActiveStatus();
        effectSphere.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
    }

    private IEnumerator HealingEffect()
    {
        while (effectDuriation >= 0) //Check for poison status in case it is removed by other means (bandages/cures)
        {
            // How this should work:
            // When Healing effect is called, the duriation has already been set in SetActiveTowerParameters(int effectType, float duration, float radius)
            // While that duriation is active, and the player is within the radius, heal the player for 10% of their max health

            effectDuriation -= 1.0f;
            yield return new WaitForSeconds(1); 
            if(CheckForPlayer()) //Check if player is within the effect radius
            {
                Debug.Log("Character should be healing...");
                float heal = playerInfo.GetMaxHealth() * 0.1f; 
                playerInfo.TakeHealing(heal);
            }
        }
        // Once the effect ends, flip the tower off       
        FlipActiveStatus();
        effectSphere.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
    }
}
