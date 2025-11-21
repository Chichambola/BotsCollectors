using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WoodSpawner : Spawner<Wood>
{
    private Coroutine _coroutine;
    
    private void OnEnable()
    {
        _coroutine = StartCoroutine(Spawn());
    }

    private void OnDisable()
    {
        StopCoroutine(_coroutine);
    }

    protected override IEnumerator Spawn()
    {
        var wait = new WaitForSeconds(Delay);

        while (enabled)
        {
            GetObject();
            
            yield return wait;
        }
    }

    protected override void ActionOnGet(Wood wood)
    {
        SpawnPoint tempSpawnPoint = GetRandomSpawnPoint();

        if (tempSpawnPoint.TryGetComponent(out Collider collider))
        {
            wood.transform.position = GetRandomPosition(collider, tempSpawnPoint);
        
            base.ActionOnGet(wood);
        }
    }

    protected override void ActionOnRelease(Wood wood)
    {
        wood.gameObject.transform.parent = null;
        
        base.ActionOnRelease(wood);
    }
}
