using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private ResourcesFinder _resourcesFinder;
    [SerializeField] private ResourcesKeeper _resourcesKeeper;
    [SerializeField] private CollectorHandler _collectorHandler;
    [SerializeField] private SpawnersHandler _spawnersHandler;

    public event Action<IPoolable, int> ValuesChanged;
    
    private Coroutine _coroutine;
    private List<Wood> _listWood;

    private void Awake()
    {
        _listWood = new List<Wood>();
    }

    private void OnEnable()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
        
        _coroutine = StartCoroutine(ScanForItems());
    }

    private void OnDisable()
    {
        StopCoroutine(_coroutine);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out Collector collector) && collector.IsCarryingItem)
        {
            ProcessTrigger(collector);
        }
    }

    private void ProcessTrigger(Collector collector)
    {
        Item tempItem = collector.GetItem();
            
        IdentifyItem(tempItem);

        _collectorHandler.SetUnitFree(collector);

        _resourcesKeeper.RemoveItem(tempItem);
            
        _spawnersHandler.Release(tempItem);
    }
    
    private IEnumerator ScanForItems()
    {
        var wait = new WaitForSeconds(_resourcesFinder.Delay);

        while (enabled)
        {
            _resourcesFinder.ScanForItems();
            
            GetItem();
            
            yield return wait;
        }
    }

    private void IdentifyItem(IPoolable item)
    {
        if (item is Wood wood)
        {
            _listWood.Add(wood);
            
            ValuesChanged?.Invoke(wood, _listWood.Count);
        }
    }
    
    private void GetItem()
    {
        if (_resourcesKeeper.HasFreeItems && _collectorHandler.HasFreeCollectors)
        {
            Item item = _resourcesKeeper.GetFreeItem();

            Collector collector = _collectorHandler.GetFreeCollector();
            
            _collectorHandler.SetTargetItem(collector, item);

            _collectorHandler.MoveUnitToTarget(collector, item.transform.position);
        }
    }
}
