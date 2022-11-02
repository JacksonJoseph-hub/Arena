using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDControl : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text healthText;
    public TMP_Text waveText;

    public int playerScore = 0;
    // Start is called before the first frame update
    void Start()
    {
        UpdateScoreText();
        healthText.text = "";
    }

    public void ModifyPlayerScore(int mod)
    {
        playerScore += mod;
        UpdateScoreText();
    }
    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + playerScore;
    }

    public void UpdateHealthText(float currentH, float maxH)
    {
        //Debug.Log("Update text @" + Time.time + currentH + " / " + maxH);
        string t = Mathf.FloorToInt(currentH) + "/" + Mathf.FloorToInt(maxH);
        healthText.text = t;
    }

    public void UpdateWaveText(int waveNum)
    {
        waveText.text = "Wave: " + waveNum;
    }
}
