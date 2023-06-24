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
    [SerializeField]
    public Slider healthSlider;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Timer());
        playerPlayer = FindObjectOfType<Player>();
        player = playerPlayer.gameObject;
        healthSlider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        playerBulletText.text = String.Format("{0}/{1}", playerPlayer.gun.ammo, playerPlayer.gun.maxAmmo);
        healthSlider.value = playerPlayer.health / playerPlayer.maxHealth;
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
}
