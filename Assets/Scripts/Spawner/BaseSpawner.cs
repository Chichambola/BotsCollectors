using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSpawner : Spawner<Base>
{
    [Header("Base components")]
    [SerializeField] private ResourcesKeeper _resourcesKeeper;
    [SerializeField] private ResourcesSpawnersHandler _spawnersHandler;

    protected override void ActionOnGet(Base @base)
    {
        @base.Init(_resourcesKeeper, _spawnersHandler);

        @base.transform.position = SpawnPosition;
        
        base.ActionOnGet(@base);
    }
}
