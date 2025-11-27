using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CollectorHandler : MonoBehaviour
{
    [SerializeField] private List<Collector> _collectors;
    [SerializeField] private Collector _prefab;
    [SerializeField] private Base _mainBase;
    
    public event Action<Collector> FlagReached;
    
    private List<Collector> _freeUnits;
    private List<Collector> _busyUnits;
    private Collector _flagCollector;
    
    public int Count => _collectors.Count;
    public bool HasFlagCollector => _flagCollector != null;
    
    private void Awake()
    {
        _freeUnits = new List<Collector>();
        _busyUnits = new List<Collector>();
    }

    private void Start()
    {
        GetCollectors();
    }

    public bool HasFreeCollectors => _freeUnits.Count > 0;

    public void CreateUnit()
    {
        Collector collector = Instantiate(_prefab, transform.position, Quaternion.identity, transform.parent);

        collector.Init(_mainBase);

        _collectors.Add(collector);
        
        _freeUnits.Add(collector);
    }

    public Collector GetFreeCollector()
    {
        int firstIndex = 0;

        int randomIndex = Random.Range(firstIndex, _freeUnits.Count);
        
        Collector freeCollector = _freeUnits[randomIndex];
        
        _busyUnits.Add(freeCollector);
        _freeUnits.Remove(freeCollector);
        
        return freeCollector;
    }

    public void SetUnitFree(Collector collector)
    {
        collector.Reset();
        
        _busyUnits.Remove(collector);
        _freeUnits.Add(collector);
    }

    public void RemoveFlagCollector(Collector collector)
    {
        _collectors.Remove(collector);
        _busyUnits.Remove(collector);
    }
    
    public void SetCollector(Collector collector)
    {
        _collectors.Add(collector);
        _freeUnits.Add(collector);
        
        collector.transform.parent = transform;
        
        collector.Init(_mainBase);
        
        collector.Reset();
    }
    
    public void SetFlagCollector(Collector flagCollector)
    {
        _flagCollector = flagCollector;
    }

    public Collector GetFlagCollector()
    {
        _flagCollector.transform.parent = null;
        
        Collector flagCollector = _flagCollector;
        
        _flagCollector = null;
        
        return flagCollector;
    }
    
    public void SetTargetObject(Collector collector,ITarget target)
    {
        collector.SetTargetObject(target);
    }

    public void MoveUnitToTarget(Collector collector, Vector3 target)
    {
        collector.StartMoving(target);
    }
    
    private void GetCollectors()
    {
        if (_collectors.Count != 0)
        {
            foreach (Collector collector in _collectors)
            {
                _freeUnits.Add(collector);
            }
        }
    }
}