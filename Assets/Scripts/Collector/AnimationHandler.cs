using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    public const string Speed = nameof(Speed);

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayRunAnimation(float speed)
    {
        _animator.SetFloat(Speed, speed);
    }
}
