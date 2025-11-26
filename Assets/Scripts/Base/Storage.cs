using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Storage : MonoBehaviour
{
    [SerializeField] private WoodInfo _woodInfo;

    public event Action EnoughToCreateCollector;
    public event Action EnoughToBuildBase;

    private List<Wood> _listWoods;
    
    private int _resourcesToCreateCollector = 3;
    private int _resourcesToBuildBase = 5;
    
    private bool _isBuildingBase => !IsBuildingUnits;
    private bool IsBuildingUnits;
    
    private Coroutine _buildCoroutine;

    private void Awake()
    {
        _listWoods = new List<Wood>();
    }

    private void OnEnable()
    {
        IsBuildingUnits = true;

        _buildCoroutine = StartCoroutine(BuildingRoutine());
    }

    private void OnDisable()
    {
        StopCoroutine(_buildCoroutine);
    }

    public void SetPriority(bool value)
    {
        IsBuildingUnits = value;
    }
    
    public void IdentifyItem(IPoolable item)
    {
        if (item is Wood wood)
        {
            _listWoods.Add(wood);
        }
        
        _woodInfo.UpdateValue(_listWoods.Count);
    }

    private IEnumerator BuildingRoutine()
    {
        while (enabled)
        {
            if (_listWoods.Count >= _resourcesToCreateCollector && IsBuildingUnits)
            {
                EnoughToCreateCollector?.Invoke();

                _listWoods.RemoveRange(0, _resourcesToCreateCollector);
            }
            else if(_listWoods.Count >= _resourcesToBuildBase && _isBuildingBase)
            {
                EnoughToBuildBase?.Invoke();
            
                _listWoods.RemoveRange(0, _resourcesToBuildBase);
            }
            
            _woodInfo.UpdateValue(_listWoods.Count);
            
            yield return null;
        }
    }
}
