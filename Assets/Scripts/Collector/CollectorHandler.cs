using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CollectorHandler : MonoBehaviour
{
    [SerializeField] private Collector[] _collectors;
    [SerializeField] private Collector _prefab;
    [SerializeField] private Base _mainBase;
    
    public event Action<Collector> FlagReached;
    
    private List<Collector> _freeUnits;
    private List<Collector> _busyUnits;
    private Collector _flagCollector;
    
    public int Count => _freeUnits.Count + _busyUnits.Count;
    public bool HasFlagCollector => _flagCollector != null;
    
    private void Awake()
    {
        _freeUnits = new List<Collector>();
        _busyUnits = new List<Collector>();
    }

    private void OnEnable()
    {
        GetCollectors();
    }

    public bool HasFreeCollectors => _freeUnits.Count > 0;

    public void CreateUnit()
    {
        Collector collector = Instantiate(_prefab, transform.position, Quaternion.identity, transform.parent);

        collector.Init(_mainBase);

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

    public void SetFlagCollector(Collector flagCollector)
    {
        _flagCollector = flagCollector;
    }

    public Collector GetFlagCollector()
    {
        return _flagCollector;
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
        if (_collectors.Length != 0)
        {
            foreach (Collector collector in _collectors)
            {
                _freeUnits.Add(collector);
            }
        }
    }
}