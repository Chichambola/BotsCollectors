using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    public event Action<Item> TargetItemDetected;

    private Item _targetItem;

    private void OnEnable()
    {
        _targetItem = null;
    }

    public void SetTargetItem(Item item)
    {
        _targetItem = item;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out Item item) && item == _targetItem)
        {
            TargetItemDetected?.Invoke(item);
        }
    }
}
