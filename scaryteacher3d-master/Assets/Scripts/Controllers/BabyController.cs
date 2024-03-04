using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyController : MonoBehaviour
{

    private Animator animator;


    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayAnimation(GameConstants.InGameConstants.BabyAnimation babyAnimation)
    {
        animator.SetTrigger(babyAnimation.ToString());
    }
}
