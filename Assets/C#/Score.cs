using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    public static int highScore;
    private void Update()
    {
        highScore = PlayerPrefs.GetInt("HighScore");
    }
}
