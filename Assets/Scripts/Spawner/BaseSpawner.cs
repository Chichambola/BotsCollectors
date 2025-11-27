using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSpawner : Spawner<Base>
{
    [Header("Base components")]
    [SerializeField] private ResourcesKeeper _resourcesKeeper;
    [SerializeField] private ResourcesSpawnersHandler _spawnersHandler;

    private Collector _collector;

    public void SetCollector(Collector collector)
    {
        _collector = collector;
    }
    
    protected override void ActionOnGet(Base @base)
    {
        @base.Init(_resourcesKeeper, _spawnersHandler);

        @base.transform.position = SpawnPosition;

        @base.SetCollector(_collector);
        
        base.ActionOnGet(@base);
    }
}
