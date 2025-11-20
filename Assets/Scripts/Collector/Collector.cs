using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Collector : MonoBehaviour
{
    [SerializeField] private Mover _mover;
    
    private BoxCollider2D _collider;
    private Item _carryingItem;
    private Vector3 _basePosition;
    
    public bool IsBusy { get; private set; }
    public bool IsCarryingItem => _carryingItem != null;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
    }

    private void OnEnable()
    {
        _basePosition = transform.parent.position;
        IsBusy = false;
        _carryingItem = null;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out Item item))
        {
            _mover.StartMoving(_basePosition);
            
            _carryingItem = item;
            
            item.transform.parent = transform;
        }

        if (collider.TryGetComponent(out Base _))
        {
            _mover.StopMoving();
        }
    }

    public Item GetItem()
    {
        Item tempItem = _carryingItem;

        _carryingItem = null;
        
        return tempItem;
    }
    
    public void MoveToTarget(Vector3 target)
    {
        IsBusy = true;

        _mover.StartMoving(target);
    }
}
