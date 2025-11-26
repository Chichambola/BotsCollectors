using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagCollisionDetector : MonoBehaviour
{
    public event Action<Flag> CollectorDetected;
    
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.TryGetComponent(out Collector collector) && gameObject.TryGetComponent(out Flag flag) && collector.IsFlagTarget)
        {
            CollectorDetected?.Invoke(flag);
        }
    }
}
