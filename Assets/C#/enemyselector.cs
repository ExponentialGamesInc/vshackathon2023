using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyselector : MonoBehaviour
{
    public Animator animator;
    private int randomInt;
    void Start()
    {
        randomInt = UnityEngine.Random.Range(0, 3);
        animator.SetInteger("EnemySelector", randomInt);
        Debug.Log(randomInt);
    }

}
