using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Spawner<T> : MonoBehaviour where T : MonoBehaviour, IPoolable
{
    [SerializeField] protected int PoolCapacity;
    [SerializeField] protected int MaxPoolCapacity = 5;
    [SerializeField] protected float Delay;
    [SerializeField] private T _objectPrefab;

    protected Vector3 SpawnPosition;
    
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

    public void StartSpawning()
    {
        _pool.Get();
    }

    public void StartSpawning(Vector3 position)
    {
        SpawnPosition = position;
        
        _pool.Get();
    }

    public void Release(T @object) 
    {
        if (@object.gameObject.activeSelf)
        {
            _pool.Release(@object);
        }
    }

    protected void GetObject()
    {
        _pool.Get();
    }

    protected virtual void ActionOnGet(T @object)
    {
        @object.gameObject.SetActive(true);
    }

    protected virtual void ActionOnRelease(T @object)
    {
        @object.gameObject.SetActive(false);
    }

    private T CreateObject()
    {
        return Instantiate(_objectPrefab);
    }
}
