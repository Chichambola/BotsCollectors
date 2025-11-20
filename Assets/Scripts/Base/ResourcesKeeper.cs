using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using Random = UnityEngine.Random;

public class ResourcesKeeper : MonoBehaviour
{
    [SerializeField] private ResourcesFinder _resourcesFinder;

    private List<Item> _foundFree = new();
    private List<Item> _foundBusy = new();

    public bool HasFreeItems => _foundFree.Count > 0;


    private void OnEnable()
    {
        _resourcesFinder.ItemFound += AddItemToList;
    }

    public Item GetFreeItem()
    {
        int firstIndex = 0;

        int randomIndex = Random.Range(firstIndex, _foundFree.Count);

        Item freeItem = _foundFree[randomIndex];

        _foundBusy.Add(freeItem);
        _foundFree.Remove(freeItem);

        return freeItem;
    }

    private void AddItemToList(Item item)
    {
        if (_foundFree.Contains(item) == false && _foundBusy.Contains(item) == false)
        {
            _foundFree.Add(item);
        }
    }
}
