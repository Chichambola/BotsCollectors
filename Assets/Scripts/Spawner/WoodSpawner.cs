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
        StartSpawning();
    }

    private void OnDisable()
    {
        StopCoroutine(_coroutine);
    }

    public override void StartSpawning()
    {
        if (_coroutine != null) 
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(Spawning());
    }

    private IEnumerator Spawning()
    {
        var wait = new WaitForSeconds(Delay);

        while (enabled)
        {
            GetObject();
            
            yield return wait;
        }
    }

    protected override void ActionOnRelease(Wood wood)
    {
        wood.gameObject.transform.parent = null;
        
        base.ActionOnRelease(wood);
    }
}
