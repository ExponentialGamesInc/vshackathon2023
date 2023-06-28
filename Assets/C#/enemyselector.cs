using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyselector : MonoBehaviour
{
    public Animator animator;
    public RuntimeAnimatorController POPcontroller;
    public RuntimeAnimatorController CBAGcontroller;
    public RuntimeAnimatorController PBAGcontroller;

    RuntimeAnimatorController[] Controllers;

    private int randomInt;
    void Start()
    {
        Controllers = new RuntimeAnimatorController[] {POPcontroller, CBAGcontroller, PBAGcontroller};


        randomInt = UnityEngine.Random.Range(0, 3);

        animator.runtimeAnimatorController = Controllers[randomInt];
        Debug.Log(randomInt);
    }

}
