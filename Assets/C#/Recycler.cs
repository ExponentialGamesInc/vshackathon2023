using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recycler : MonoBehaviour
{
    public Animator animator;
    void Start()
    {
        animator.SetBool("inTrigger", false);
    }
    
    public void ToggleAnimation(bool toggle)
    {
        animator.SetBool("inTrigger", toggle);
    }
}
