using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

[RequireComponent(typeof(BoxCollider))]
public class Collector : MonoBehaviour
{
    [SerializeField] private Mover _mover;
    [SerializeField] private AnimationHandler _animationHandler;
    [SerializeField] private CollisionDetector _collisionDetector;
    
    private BoxCollider _collider;
    private Item _targetItem;
    private Item _carryingItem;
    private Vector3 _basePosition;
    
    public bool IsCarryingItem => _carryingItem != null;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
    }

    private void OnEnable()
    {
        _collisionDetector.TargetItemDetected += CarryItemToBase;
        _basePosition = transform.parent.position;
        _targetItem = null;
        _carryingItem = null;
    }

    private void CarryItemToBase(Item item)
    {
        StartMoving(_basePosition);

        _carryingItem = item;

        item.transform.parent = transform;
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
}
