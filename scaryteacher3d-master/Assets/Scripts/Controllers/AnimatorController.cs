using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    public void AnimateStudent(GameConstants.InGameConstants.StudentAnimationNames studentAnimationName)
    {
        _animator.SetTrigger(studentAnimationName.ToString());
    }
    public void PlayAnimation(string animationName)
    {
        _animator.Play(animationName);
    }

}
