using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

[RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
public class Collector : MonoBehaviour, IPoolable
{
    [SerializeField] private Mover _mover;
    [SerializeField] private AnimationHandler _animationHandler;
    [SerializeField] private CollisionDetector _collisionDetector;
    [SerializeField] private LayerMask _layerToExclude;
    [SerializeField] private Base _mainBase;
    
    private BoxCollider _collider;
    private Rigidbody _rigidbody;
    private Item _targetItem;
    private Item _carryingItem;
    private Vector3 _basePosition;
    
    public bool IsCarryingItem => _carryingItem != null;
    public Base MainBase => _mainBase;

    public void Init(Base mainBase)
    {
        _mainBase = mainBase;
    }

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _rigidbody.excludeLayers = _layerToExclude;
        _basePosition = transform.parent.position;
        _targetItem = null;
        _carryingItem = null;
        _collisionDetector.TargetItemDetected += CarryItemToBase;
    }

    public Item GetItem()
    {
        Item tempItem = _targetItem;

        _targetItem = null;
        
        return tempItem;
    }

    public void SetTargetItem(Item item)
    {
        _targetItem = item;
        
        _collisionDetector.SetTargetItem(item);
    }
    
    public void Reset()
    {        
        float speed = 0;

        _animationHandler.PlayRunAnimation(speed);

        _mover.StopMoving();

        _carryingItem = null;

        _targetItem = null;
    }
    
    public void StartMoving(Vector3 target)
    {
        float speed = _mover.Speed;

        _mover.StartMoving(target);

        _animationHandler.PlayRunAnimation(speed);
    }

    private void CarryItemToBase(Item item)
    {
        StartMoving(_basePosition);

        _carryingItem = item;

        item.transform.parent = transform;
    }
}
