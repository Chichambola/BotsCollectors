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
            Item tempItem = collector.GetItem();
            
            IdentifyItem(tempItem);

            collector.StopMoving();
        }
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

    private void IdentifyItem(Item item)
    {
        if (item.TryGetComponent(out Wood wood))
        {
            _listWood.Add(wood);
        }
    }
    
    private void GetItem()
    {
        if (_resourcesKeeper.HasFreeItems && _collectorHandler.HasFreeCollectors)
        {
            Item item = _resourcesKeeper.GetFreeItem();

            Collector collector = _collectorHandler.GetFreeCollector();
            
            collector.SetTargetItem(item);

            collector.MoveToTarget(item.transform.position);
        }
    }
}
