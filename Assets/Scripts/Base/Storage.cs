using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    [SerializeField] private WoodInfo _woodInfo;

    public event Action EnoughToCreateCollector;

    private List<Wood> _listWoods;
    private int _resourcesToCreateCollector = 3;

    private void Awake()
    {
        _listWoods = new List<Wood>();
    }

    public void IdentifyItem(IPoolable item)
    {
        if (item is Wood wood)
        {
            _listWoods.Add(wood);

            if(_listWoods.Count >= _resourcesToCreateCollector)
            {
                EnoughToCreateCollector.Invoke();

                _listWoods.RemoveRange(0, _resourcesToCreateCollector);
            }
        }

        _woodInfo.UpdateValue(_listWoods.Count);
    } 
}
