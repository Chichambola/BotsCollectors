using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Spawner<T> : MonoBehaviour where T : Item
{
    [SerializeField] protected int PoolCapacity;
    [SerializeField] protected int MaxPoolCapacity = 5;
    [SerializeField] protected float Delay;
    [SerializeField] protected List<SpawnPoint> SpawnPoints;
    [SerializeField] private T _objectPrefab;

    private ObjectPool<T> _pool;

    private void OnValidate()
    {
        if (PoolCapacity > MaxPoolCapacity)
            PoolCapacity = MaxPoolCapacity - 1;
    }

    protected void Awake()
    {
        _pool = new ObjectPool<T>(
            createFunc: CreateObject,
            actionOnGet: ActionOnGet,
            actionOnRelease: ActionOnRelease,
            actionOnDestroy: @object => Destroy(@object.gameObject),
            collectionCheck: true,
            defaultCapacity: PoolCapacity,
            maxSize: MaxPoolCapacity);
    }

    private T CreateObject()
    {
        return Instantiate(_objectPrefab);
    }

    protected void GetObject()
    {
        _pool.Get();
    }

    protected virtual void ActionOnGet(T @object)
    {
        @object.gameObject.SetActive(true);
    }

    public void Release(T @object) 
    {
        if (@object.gameObject.activeSelf)
        {
            _pool.Release(@object);
        }
    }

    protected virtual void ActionOnRelease(T @object)
    {
        @object.gameObject.SetActive(false);
    }

    protected SpawnPoint GetRandomSpawnPoint()
    {
        int firstIndex = 0;
        
        int randomIndex = Random.Range(firstIndex, SpawnPoints.Count);
        
        return SpawnPoints[randomIndex];
    }
    
    protected Vector3 GetRandomPosition(Collider collider, SpawnPoint spawnPoint)
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
    
    protected abstract IEnumerator Spawn();
}
