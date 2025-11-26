using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class Base : MonoBehaviour, IPoolable
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
    public bool HasFlagCollector  => _collectorHandler.HasFlagCollector;

    public void Init(ResourcesKeeper resourcesKeeper, ResourcesSpawnersHandler resourcesSpawnersHandler)
    {
        _resourcesKeeper = resourcesKeeper;
        _spawnersHandler = resourcesSpawnersHandler;
    }

    private void OnEnable()
    {
        _storage.EnoughToCreateCollector += SpawnCollector;
        _storage.EnoughToBuildBase += MoveUnitToFlag;
        
        if (_scanningRoutine != null)
            StopCoroutine(_scanningRoutine); 
        
        if (_collectingRoutine != null)
            StopCoroutine(_collectingRoutine);
    }

    private void OnDisable()
    {
        StopCoroutine(_scanningRoutine);
        StopCoroutine(_collectingRoutine);
        
        _storage.EnoughToCreateCollector -= SpawnCollector;
        _storage.EnoughToBuildBase -= MoveUnitToFlag;
    }

    private void Start()
    {
        _scanningRoutine = StartCoroutine(ScanForItems());
        _collectingRoutine = StartCoroutine(CollectItems());
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

    public void ChangePriority(bool value)
    {
        _storage.SetPriority(value);
    }

    public void SetTargetFlag(Flag flag)
    {
        _targetFlag = flag;
    }
    
    public Flag GetTargetFlag()
    {
        return _targetFlag;
    }

    public void ChangeDirection()
    {
        Collector flagCollector = _collectorHandler.GetFlagCollector();
        
        _collectorHandler.MoveUnitToTarget(flagCollector, _targetFlag.transform.position);
    }
    
    private void MoveUnitToFlag()
    {
        int minUnitsAmount = 1;
        
        if (_collectorHandler.HasFreeCollectors && _collectorHandler.Count > minUnitsAmount && _collectorHandler.HasFlagCollector == false)
        {
            Collector collector = _collectorHandler.GetFreeCollector();
            
            _collectorHandler.SetFlagCollector(collector);
            
            _collectorHandler.SetTargetObject(collector, _targetFlag);
            
            _collectorHandler.MoveUnitToTarget(collector, _targetFlag.transform.position);
        }
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

            yield return wait;
        }
    }

    private void StartCollecting()
    {
        if (_resourcesKeeper.HasFreeItems && _collectorHandler.HasFreeCollectors)
        {
            Item item = _resourcesKeeper.GetFreeItem();

            Collector collector = _collectorHandler.GetFreeCollector();

            _collectorHandler.SetTargetObject(collector, item);

            _collectorHandler.MoveUnitToTarget(collector, item.transform.position);
        }
    }
}
