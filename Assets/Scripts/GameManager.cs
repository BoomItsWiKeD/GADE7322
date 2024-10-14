using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static int playerGold;
    public TMP_Text goldText;
    public TMP_Text waveCounterText;
    public TMP_Text statsText;
    public Slider HQHPSlider;
    public GameObject pauseScreen;
    public GameObject deathScreen;

    public static bool selectedTower1;
    public static bool selectedTower2;
    public static bool selectedTower3;
    
    // Start is called before the first frame update
    void Start()
    {
        playerGold = 500;
    }

    // Update is called once per frame
    void Update()
    {
        goldText.text = "" + playerGold + "G";
        HQHPSlider.value = HQManager.hqHP;
        waveCounterText.text = "Wave: " + SpawnEnemy.wavesSpawned;
        if (HQManager.hqHP <= 0)
        {
            Time.timeScale = 0;
            statsText.text = "You survived " + SpawnEnemy.wavesSpawned + " waves";
            deathScreen.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            pauseScreen.SetActive(true);
        }
    }

    public void onResumeClick()
    {
        Time.timeScale = 1;
        pauseScreen.SetActive(false);
    }

    public void onRetryClick()
    {
        SceneManager.LoadScene("Caden_Test");
    }

    public void onMainMenuClick()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    //When player presses button to buy and place turret:
    //The variables here are used in "PlaceTurret.cs"
    public void OnPlaceTower1Click()
    {
        selectedTower1 = true;
        selectedTower2 = false;
        selectedTower3 = false;
    }
    public void OnPlaceTower2Click()
    {
        selectedTower1 = false;
        selectedTower2 = true;
        selectedTower3 = false;
    }
    public void OnPlaceTower3Click()
    {
        selectedTower1 = false;
        selectedTower2 = false;
        selectedTower3 = true;
    }
}
