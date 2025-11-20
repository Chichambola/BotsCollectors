using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Collector : MonoBehaviour
{
    [SerializeField] private Mover _mover;
    
    private BoxCollider _collider;
    private Item _targetItem;
    private Item _carryingItem;
    private Vector3 _basePosition;
    
    public bool IsBusy { get; private set; }
    public bool IsCarryingItem => _carryingItem != null;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
    }

    private void OnEnable()
    {
        _basePosition = transform.parent.position;
        IsBusy = false;
        _targetItem = null;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out Item item) && item == _targetItem)
        {
            _mover.StartMoving(_basePosition);
            
            _targetItem = item;
            
            item.transform.parent = transform;
        }
    }

    public void SetTargetItem(Item item)
    {
        _targetItem = item;
    }

    public Item GetItem()
    {
        Item tempItem = _targetItem;

        _targetItem = null;
        
        return tempItem;
    }
    
    public void StopMoving()
    {
        _mover.StopMoving();
    }

    public void MoveToTarget(Vector3 target)
    {
        IsBusy = true;

        _mover.StartMoving(target);
    }
}
