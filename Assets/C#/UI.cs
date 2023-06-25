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
    public GameObject pauseMenuUI;

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
        playerBulletText.text = String.Format("{0}/{1}", playerPlayer.gun.ammo, playerPlayer.gun.maxAmmo);
        healthBar.offsetMax = new Vector2(-(1 - (float)playerPlayer.health / playerPlayer.maxHealth) * healthBarWidth, 0);
        PauseMenu();
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
        Time.timeScale = 1.0f;
        gamePaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
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
}