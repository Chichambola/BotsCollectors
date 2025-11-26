using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Flag : MonoBehaviour, ITarget
{
    [SerializeField] private FlagCollisionDetector _collisionDetector;

    public event Action<Flag> CanBeDestroyed;
    
    private void OnEnable()
    {
        _collisionDetector.CollectorDetected += DestroyFlag;
    }

    private void OnDisable()
    {
        _collisionDetector.CollectorDetected -= DestroyFlag;
    }

    private void DestroyFlag(Flag flag)
    {
        CanBeDestroyed?.Invoke(flag);
    }
}
