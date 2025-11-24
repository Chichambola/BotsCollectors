using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesSpawnersHandler : MonoBehaviour
{
    [SerializeField] private WoodSpawner _woodSpawner;
    
    public void Release(IPoolable poolableObject)
    {
        if (poolableObject is Wood wood)
        {
            _woodSpawner.Release(wood);
        }
    }
}
