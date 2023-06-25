using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq.Expressions;

public class Base : MonoBehaviour
{
    public int maxHealth;
    public int health;
    public TextMeshPro healthText;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = string.Format("{0}/{1}", health, maxHealth);
    }
}
