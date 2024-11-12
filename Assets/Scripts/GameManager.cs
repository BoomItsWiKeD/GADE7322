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
    public TMP_Text selectedTowerText;
    public Slider HQHPSlider;
    public GameObject pauseScreen;
    public GameObject deathScreen;

    public static bool selectedTower1;
    public static bool selectedTower2;
    public static bool selectedTower3;

    public TMP_Text healthLvlText;
    public TMP_Text attackLvlText;
    public TMP_Text attackSpeedLvlText;

    private int healthLvl;
    private int attackLvl;
    private int attackSpeedLvl;

    // Start is called before the first frame update
    void Start()
    {
        Turret.health1 = 100;
        Turret.health2 = 50;
        Turret.health3 = 150;
        
        Turret.shootDelay1 = 1f;
        Turret.shootDelay2 = 0.5f;
        Turret.shootDelay3 = 3f;
        
        EnemyManager.missile1DMG = 40;
        EnemyManager.missile2DMG = 30;
        EnemyManager.missile3DMG = 100;

        healthLvl = 0;
        attackLvl = 0;
        attackSpeedLvl = 0;
        healthLvlText.text = healthLvl.ToString();
        attackLvlText.text = attackLvl.ToString();
        attackSpeedLvlText.text = attackSpeedLvl.ToString();
        
        selectedTowerText.text = "No Turret Selected";
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
        selectedTowerText.text = "Normal Turret Selected";
    }
    public void OnPlaceTower2Click()
    {
        selectedTower1 = false;
        selectedTower2 = true;
        selectedTower3 = false;
        selectedTowerText.text = "Quick Turret Selected";
    }
    public void OnPlaceTower3Click()
    {
        selectedTower1 = false;
        selectedTower2 = false;
        selectedTower3 = true;
        selectedTowerText.text = "Heavy Turret Selected";
    }

    public void OnHealthUpgradeClick()
    {
        if (GameManager.playerGold >= 1000)
        {
            GameManager.playerGold = GameManager.playerGold - 1000;
            
            Turret.health1 += 20;
            Turret.health2 += 20;
            Turret.health3 += 20;

            healthLvl += 1;
            healthLvlText.text = healthLvl.ToString();
        }
        
    }

    public void OnAttackUpgradeClick()
    {
        if (GameManager.playerGold >= 1500)
        {
            GameManager.playerGold = GameManager.playerGold - 1500;
            
            EnemyManager.missile1DMG += 30;
            EnemyManager.missile2DMG += 20;
            EnemyManager.missile3DMG += 50;
            
            attackLvl += 1;
            attackLvlText.text = attackLvl.ToString();
        }
    }

    public void OnAttackSpeedUpgradeClick()
    {
        if (GameManager.playerGold >= 2000)
        {
            GameManager.playerGold = GameManager.playerGold - 2000;
            
            Turret.shootDelay1 -= 0.25f;
            Turret.shootDelay2 -= 0.15f;
            Turret.shootDelay3 -= 0.4f;
            
            attackSpeedLvl += 1;
            attackSpeedLvlText.text = attackSpeedLvl.ToString();
            
            //0.1 is fastest shoot delay:
            if (Turret.shootDelay1 <= 0.1 || Turret.shootDelay2 <= 0.1 || Turret.shootDelay3 <= 0.1)
            {
                Turret.shootDelay1 -= 0.1f;
                Turret.shootDelay2 -= 0.1f;
                Turret.shootDelay3 -= 0.1f;
                
                attackSpeedLvl = 0;
                attackSpeedLvlText.text = attackSpeedLvl.ToString();
            }
        }
    }
}
