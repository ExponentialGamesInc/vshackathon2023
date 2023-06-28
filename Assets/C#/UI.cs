using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UIElements;
using System.Net.NetworkInformation;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    public int count = 0;
    public TextMeshProUGUI hitnText;
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
    public TextMeshProUGUI healthText;
    public GameObject deathScreen;
    public TextMeshProUGUI deathText;
    public TextMeshProUGUI scoreText;
    public AudioSource music;
    public AudioSource muffledMusic;

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
        gamePaused = false;
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
            music.mute = true;
        }
        else
        {
            Time.timeScale = 1f;
            music.mute = false;
        }

        playerBulletText.text = string.Format("{0}/{1}", playerPlayer.gun.ammo, playerPlayer.gun.maxAmmo);
        healthBar.offsetMax = new Vector2(-(1 - (float)playerPlayer.health / playerPlayer.maxHealth) * healthBarWidth, 0);
        scrapText.text = string.Format("{0}", playerPlayer.scrap);
        healthText.text = string.Format("{0}/{1}", playerPlayer.health, playerPlayer.maxHealth);

        PauseMenu();

        if (Input.GetKeyDown(KeyCode.E) && !pauseMenuUI.active)
        {
            if (Vector3.Distance(player.transform.position, new Vector3(0, 0, 0)) < 4.2f && !baseMenuUI.active)
            {
                baseMenuUI.SetActive(true);
                FindObjectOfType<Recycler>().ToggleAnimation(true);
            }
            else
            {
                baseMenuUI.SetActive(false);
                FindObjectOfType<Recycler>().ToggleAnimation(false);
            }
        }

        if (Vector3.Distance(player.transform.position, new Vector3(0, 0, 0)) >= 4.2f)
        {
            baseMenuUI.SetActive(false);
            FindObjectOfType<Recycler>().ToggleAnimation(false);
        }

        hitnText.gameObject.SetActive(false);

        if (Vector3.Distance(player.transform.position, new Vector3(0, 0, 0)) < 4.2f && !baseMenuUI.active)
        {
            hitnText.gameObject.SetActive(true);
        }
    }

    IEnumerator Timer()
    {
        count = 0;
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
        if (Input.GetKeyDown(KeyCode.Escape))
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
        FindObjectOfType<Recycler>().ToggleAnimation(false);
        gamePaused = true;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Main Menu");
        gamePaused = false;
        Time.timeScale = 1f;
        Debug.Log("Loading Menu...");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting Game...");
    }

    public void UpgradeHealth()
    {
        if (playerPlayer.healthUpgradeLevel == 4)
            return;

        var price = prices[playerPlayer.healthUpgradeLevel + 1];
        if (playerPlayer.scrap < price)
            return;

        playerPlayer.scrap -= price;
        playerPlayer.healthUpgradeLevel += 1;
        playerPlayer.maxHealth = playerPlayer.healthUpgrades[playerPlayer.healthUpgradeLevel];
        playerPlayer.health = playerPlayer.maxHealth;
        playerPlayer.upgrades += 1;

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
        if (playerPlayer.scrap < price)
            return;
       
        playerPlayer.scrap -= price;
        playerPlayer.damageUpgradeLevel += 1;
        playerPlayer.gun.damage = playerPlayer.damageUpgrades[playerPlayer.damageUpgradeLevel];
        playerPlayer.upgrades += 1;

        if (playerPlayer.damageUpgradeLevel < 4) damageUpgradeText.text = string.Format("{0} -> {1}\nprice: {2}", playerPlayer.gun.damage, playerPlayer.damageUpgrades[playerPlayer.damageUpgradeLevel + 1], prices[playerPlayer.damageUpgradeLevel + 1]);
        else damageUpgradeText.text = "Max";
    }

    public void UpgradeAttackDelay()
    {
        if (playerPlayer.attackDelayUpgradeLevel == 4) 
            return;

        var price = prices[playerPlayer.attackDelayUpgradeLevel + 1];
        if (playerPlayer.scrap < price)
            return;

        playerPlayer.scrap -= price;
        playerPlayer.attackDelayUpgradeLevel += 1;
        playerPlayer.gun.fireDelay = playerPlayer.attackDelayUpgrades[playerPlayer.attackDelayUpgradeLevel];
        playerPlayer.upgrades += 1;

        if (playerPlayer.attackDelayUpgradeLevel < 4) attackDelayUpgradeText.text = string.Format("{0} -> {1}\nprice: {2}", playerPlayer.gun.fireDelay, playerPlayer.attackDelayUpgrades[playerPlayer.attackDelayUpgradeLevel + 1], prices[playerPlayer.attackDelayUpgradeLevel + 1]);
        else attackDelayUpgradeText.text = "Max";
    }
}