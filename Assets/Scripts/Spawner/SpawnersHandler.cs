using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnersHandler : MonoBehaviour
{
    [SerializeField] private WoodSpawner _woodSpawner;
    
    public void Release(IPoolable item)
    {
        if (item is Wood wood)
        {
            _woodSpawner.Release(wood);
        }
    }
}
