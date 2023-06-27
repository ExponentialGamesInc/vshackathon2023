using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class enemyselector : MonoBehaviour
{
    public Animator animator;
    public AnimatorController POPcontroller;
    public AnimatorController CBAGcontroller;
    public AnimatorController PBAGcontroller;

    AnimatorController[] Controllers;

    private int randomInt;
    void Start()
    {
        Controllers = new AnimatorController[] {POPcontroller, CBAGcontroller, PBAGcontroller};


        randomInt = UnityEngine.Random.Range(0, 3);

        animator.runtimeAnimatorController = Controllers[randomInt];
        Debug.Log(randomInt);
    }

}
