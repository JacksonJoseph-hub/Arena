using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerControl : MonoBehaviour
{


    public GameObject[] TowerList;
    private int numberOfTowers;
    private int numberOfEffects = 2; //2 effects, only used in an array

    [Header("Tower Stats")]
    public float effectCooldownTimer = 15.0f;
    public float minEffectDuriationTimer = 10.0f;
    public float maxEffectDuriationTimer = 15.0f;
    public float minEffectRadius = 8.0f;
    public float maxEffectRadius = 16.0f;


    private float activateNextTowerTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        TowerList = GameObject.FindGameObjectsWithTag("Tower");
        numberOfTowers = TowerList.Length;
    }

    // Update is called once per frame
    void Update()
    {
        MasterTowerController();
    }

    // Check if any tower in the game is active
    private bool IsAnyTowerActive()
    {
        foreach (GameObject tower in TowerList)
        {
            if (tower.GetComponent<TowerFollower>().IsActiveTower())
            {
                return true;
            }
        }
        return false;
    }

    private bool onCooldown()
    {
        // activateNextTowerTimer is set in MasterTowerController and is the sum of Time.time + effectCooldownTimer
        // once time is NOT greater than the timer the function returns that it is NOT on cooldown
        if(Time.time < activateNextTowerTimer) 
        {
            return true;
        }
        return false;
    }

    private void MasterTowerController()
    {
        // Check if any towers are active and off cooldown
        if(!IsAnyTowerActive()  && !onCooldown())
        {
            // Randomly select a tower to active
            int towerSelection = Random.Range(0, numberOfTowers);

            // Randomly select an effect to use 0 = enrage, 1 = healing, 2 = ?, 3? etc
            int effectSelection = Random.Range(0, numberOfEffects);

            //Select Duriation of effect
            float duriationSelection = Mathf.Floor(Random.Range(minEffectDuriationTimer, maxEffectDuriationTimer));

            //Select radius of effect
            float radiusSelection = Mathf.Floor(Random.Range(minEffectRadius, maxEffectRadius));

            Debug.Log("Tower Control activates tower: " + TowerList[towerSelection].name + " at: " + Time.time);
            TowerList[towerSelection].GetComponent<TowerFollower>().FlipActiveStatus();
            TowerList[towerSelection].GetComponent<TowerFollower>().SetActiveTowerParameters(effectSelection, duriationSelection, radiusSelection);
            
            // Reset cooldown - each tower keeps track of their individual cooldown in the follower script, the master keeps
            // track of a longer cooldown to space out active towers further
            activateNextTowerTimer = Time.time + effectCooldownTimer;

        }
    }

}
