using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesFinder : MonoBehaviour
{
    [SerializeField] private float _delay;
    [SerializeField] private float _lookRadius;

    public event Action<Item> ItemFound;
    
    public float Delay => _delay;

    public void ScanForItems()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _lookRadius);

        if (colliders.Length > 0)
        {
            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent(out Item item))
                {
                    ItemFound?.Invoke(item);
                }
            }
        }
    }
}
