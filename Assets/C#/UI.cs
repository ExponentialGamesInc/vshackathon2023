using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UIElements;

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

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Timer());
        playerPlayer = FindObjectOfType<Player>();
        player = playerPlayer.gameObject;
        healthBarWidth = healthBar.rect.width;
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

}