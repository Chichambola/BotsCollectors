using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class Base : MonoBehaviour
{
    [SerializeField] private ResourcesFinder _resourcesFinder;
    [SerializeField] private ResourcesKeeper _resourcesKeeper;
    [SerializeField] private CollectorHandler _collectorHandler;
    [SerializeField] private ResourcesSpawnersHandler _spawnersHandler;
    [SerializeField] private Storage _storage;

    private Coroutine _scanningRoutine;
    private Coroutine _collectingRoutine;
    private Flag _targetFlag;
    
    public bool IsFlagPlaced => _targetFlag != null;

    private void OnEnable()
    {
        _storage.EnoughToCreateCollector += SpawnCollector;
        _storage.EnoughToBuildBase += InitiateBuildingProcess;

        if (_scanningRoutine != null)
            StopCoroutine(_scanningRoutine); 
        
        if (_collectingRoutine != null)
            StopCoroutine(_collectingRoutine);

        _scanningRoutine = StartCoroutine(ScanForItems());
        _collectingRoutine = StartCoroutine(CollectItems());
    }

    private void OnDisable()
    {
        StopCoroutine(_scanningRoutine);
        StopCoroutine(_collectingRoutine);
        
        _storage.EnoughToCreateCollector -= SpawnCollector;
        _storage.EnoughToBuildBase -= InitiateBuildingProcess;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out Collector collector) && collector.IsCarryingItem)
        {
            if (gameObject.TryGetComponent(out Base mainBase) && collector.MainBase == mainBase)
            {
                ProcessTrigger(collector);
            }
        }
    }

    public void ChangePriority()
    {
        _storage.SetPriority(false);
    }

    private void InitiateBuildingProcess()
    {
        
    }
    
    private void SpawnCollector()
    {
        _collectorHandler.CreateUnit();
    }

    private void ProcessTrigger(Collector collector)
    {
        Item tempItem = collector.GetItem();

        _storage.IdentifyItem(tempItem);

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

            yield return wait;
        }
    }

    private IEnumerator CollectItems()
    {
        var wait = new WaitForSeconds(_resourcesFinder.Delay);

        while (enabled)
        {
            StartCollecting();

            yield return null;
        }
    }

    private void StartCollecting()
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
