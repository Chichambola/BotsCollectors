using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    [SerializeField] private WoodInfo _woodInfo;

    private List<Wood> _listWoods;

    private void Awake()
    {
        _listWoods = new List<Wood>();
    }

    public void IdentifyItem(IPoolable item)
    {
        if (item is Wood wood)
        {
            _listWoods.Add(wood);

            _woodInfo.UpdateValue(_listWoods.Count);
        }
    } 
}
