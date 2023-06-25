using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UIElements;
using System.Net.NetworkInformation;

public class UI : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI playerBulletText;
    private GameObject player;
    private Player playerPlayer;
    //private Slider healthSlider;
    public RectTransform healthBar;
    private float healthBarWidth;
    public static bool gamePaused = false;
    public TextMeshProUGUI scrapText;
    public GameObject pauseMenuUI;
    public GameObject baseMenuUI;
    public TextMeshProUGUI baseUIScrapText;
    public TextMeshProUGUI pointsText;
    [Header("Upgrades")]

    public List<int> prices = new List<int>
    {
        30,
        75,
        250,
        600,
        1550,
    };

    public TextMeshProUGUI healthUpgradeText;
    public TextMeshProUGUI damageUpgradeText;
    public TextMeshProUGUI attackDelayUpgradeText;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Timer());
        playerPlayer = FindObjectOfType<Player>();
        player = playerPlayer.gameObject;
        healthBarWidth = healthBar.rect.width;
        attackDelayUpgradeText.text = string.Format("{0} -> {1}\nprice: {2}", playerPlayer.gun.fireDelay, playerPlayer.attackDelayUpgrades[playerPlayer.attackDelayUpgradeLevel + 1], prices[playerPlayer.attackDelayUpgradeLevel + 1]);
        healthUpgradeText.text = string.Format("{0} -> {1}\nprice: {2}", playerPlayer.maxHealth, playerPlayer.healthUpgrades[playerPlayer.healthUpgradeLevel + 1], prices[playerPlayer.healthUpgradeLevel + 1]);
        damageUpgradeText.text = string.Format("{0} -> {1}\nprice: {2}", playerPlayer.gun.damage, playerPlayer.damageUpgrades[playerPlayer.damageUpgradeLevel + 1], prices[playerPlayer.damageUpgradeLevel + 1]);
    }

    // Update is called once per frame
    void Update()
    {

        if (gamePaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
        playerBulletText.text = string.Format("{0}/{1}", playerPlayer.gun.ammo, playerPlayer.gun.maxAmmo);
        healthBar.offsetMax = new Vector2(-(1 - (float)playerPlayer.health / playerPlayer.maxHealth) * healthBarWidth, 0);
        scrapText.text = string.Format("Scrap: {0}", playerPlayer.scrap);
        pointsText.text = string.Format("Points: {0}", playerPlayer.points);
        baseUIScrapText.text = string.Format("Scrap: {0}", playerPlayer.scrap);
        PauseMenu();

        if (Input.GetKeyDown(KeyCode.E) && !pauseMenuUI.active)
        {
            if (Vector3.Distance(player.transform.position, new Vector3(0, 0, 0)) < 4.2f && !gamePaused)
            {
                baseMenuUI.SetActive(true);
                gamePaused = !gamePaused;
            }
            else if (gamePaused)
            {
                baseMenuUI.SetActive(false);
                gamePaused = !gamePaused;
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape) && baseMenuUI.active)
        {
            baseMenuUI.SetActive(false);
            gamePaused = !gamePaused;
        }
    }

    IEnumerator Timer()
    {
        int count = 0;
        while (true)
        {
            yield return new WaitForSeconds(1);
            count++;
            TimeSpan time = TimeSpan.FromSeconds(count);
            timerText.text = time.ToString(@"mm\:ss");
        }
    }

    public void PauseMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !baseMenuUI.active)
        {
            if (gamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }    

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        gamePaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        baseMenuUI.SetActive(false);
        gamePaused = true;
    }

    public void LoadMenu()
    {
        Debug.Log("Loading Menu...");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
    }

    public void ConvertScrap()
    {
        playerPlayer.points += playerPlayer.scrap;
        playerPlayer.scrap = 0;
    }

    public void UpgradeHealth()
    {
        if (playerPlayer.healthUpgradeLevel == 4)
            return;

        var price = prices[playerPlayer.healthUpgradeLevel + 1];
        if (playerPlayer.points < price)
            return;

        playerPlayer.points -= price;
        playerPlayer.healthUpgradeLevel += 1;
        playerPlayer.maxHealth = playerPlayer.healthUpgrades[playerPlayer.healthUpgradeLevel];
        playerPlayer.health = playerPlayer.maxHealth;

        if (playerPlayer.healthUpgradeLevel < 4)
            healthUpgradeText.text = string.Format("{0} -> {1}\nprice: {2}", playerPlayer.maxHealth, playerPlayer.healthUpgrades[playerPlayer.healthUpgradeLevel + 1], prices[playerPlayer.healthUpgradeLevel + 1]);
        else
            healthUpgradeText.text = "Max";
    }

    public void UpgradeDamage()
    {
        if (playerPlayer.damageUpgradeLevel == 4) 
            return;   

        var price = prices[playerPlayer.damageUpgradeLevel + 1];
        if (playerPlayer.points < price)
            return;
       
        playerPlayer.points -= price;
        playerPlayer.damageUpgradeLevel += 1;
        playerPlayer.gun.damage = playerPlayer.damageUpgrades[playerPlayer.damageUpgradeLevel];

        if (playerPlayer.damageUpgradeLevel < 4) damageUpgradeText.text = string.Format("{0} -> {1}\nprice: {2}", playerPlayer.gun.damage, playerPlayer.damageUpgrades[playerPlayer.damageUpgradeLevel + 1], prices[playerPlayer.damageUpgradeLevel + 1]);
        else damageUpgradeText.text = "Max";
    }

    public void UpgradeAttackDelay()
    {
        if (playerPlayer.attackDelayUpgradeLevel == 4) 
            return;

        var price = prices[playerPlayer.attackDelayUpgradeLevel + 1];
        if (playerPlayer.points < price)
            return;

        playerPlayer.points -= price;
        playerPlayer.attackDelayUpgradeLevel += 1;
        playerPlayer.gun.fireDelay = playerPlayer.attackDelayUpgrades[playerPlayer.attackDelayUpgradeLevel];

        if (playerPlayer.attackDelayUpgradeLevel < 4) attackDelayUpgradeText.text = string.Format("{0} -> {1}\nprice: {2}", playerPlayer.gun.fireDelay, playerPlayer.attackDelayUpgrades[playerPlayer.attackDelayUpgradeLevel + 1], prices[playerPlayer.attackDelayUpgradeLevel + 1]);
        else attackDelayUpgradeText.text = "Max";
    }
}