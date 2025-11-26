using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WoodSpawner : Spawner<Wood>
{
    [SerializeField] private List<SpawnPoint> _spawnPoints;
    
    private Coroutine _coroutine;
    
    private void OnEnable()
    {
        StartSpawning();
    }

    private void OnDisable()
    {
        StopCoroutine(_coroutine);
    }

    public void StartSpawning()
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

    protected override void ActionOnGet(Wood @object)
    {
        SpawnPoint tempSpawnPoint = GetRandomSpawnPoint();

        if (tempSpawnPoint.TryGetComponent(out Collider collider))
        {
            @object.transform.position = GetRandomPosition(collider, tempSpawnPoint);
        }
        
        base.ActionOnGet(@object);
    }

    protected override void ActionOnRelease(Wood wood)
    {
        wood.gameObject.transform.parent = null;
        
        base.ActionOnRelease(wood);
    }
    
    private SpawnPoint GetRandomSpawnPoint()
    {
        int firstIndex = 0;
        
        int randomIndex = Random.Range(firstIndex, _spawnPoints.Count);
        
        return _spawnPoints[randomIndex];
    }
    
    private Vector3 GetRandomPosition(Collider collider, SpawnPoint spawnPoint)
    {
        float spawnAreaMinX = collider.bounds.min.x;
        float spawnAreaMaxX = collider.bounds.max.x;

        float spawnAreaMinZ = collider.bounds.min.z;
        float spawnAreaMaxZ = collider.bounds.max.z;

        float objectPositionX = Random.Range(spawnAreaMinX, spawnAreaMaxX);
        float objectPositionY = spawnPoint.transform.position.y;
        float objectPositionZ = Random.Range(spawnAreaMinZ, spawnAreaMaxZ);
        
        Vector3 position = new Vector3(objectPositionX, objectPositionY, objectPositionZ);
        
        return position;
    }
}
