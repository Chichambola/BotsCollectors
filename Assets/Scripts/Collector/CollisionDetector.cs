using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    public event Action<Item> ItemDetected;
    public event Action FlagDetected;

    private ITarget _targetObject;

    private void OnEnable()
    {
        _targetObject = null;
    }

    public void SetTargetObject(ITarget target)
    {
        _targetObject = target;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out Item item) && item == _targetObject)
        {
            ItemDetected?.Invoke(item);
        }

        if (collider.TryGetComponent(out Flag flag) && flag == _targetObject)
        {
            FlagDetected?.Invoke();
        }
    }
}
