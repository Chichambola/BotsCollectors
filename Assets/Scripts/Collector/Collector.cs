using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Collector : MonoBehaviour
{
    [SerializeField] private Mover _mover;
    [SerializeField] private AnimationHandler _animationHandler;
    
    private BoxCollider _collider;
    private Item _targetItem;
    private Item _carryingItem;
    private Vector3 _basePosition;
    private Coroutine _coroutine;
    
    public bool IsCarryingItem => _carryingItem != null;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
    }

    private void OnEnable()
    {
        _basePosition = transform.parent.position;
        _targetItem = null;
        _carryingItem = null;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out Item item) && item == _targetItem)
        {
            StartMoving(_basePosition);
            
            _carryingItem = item;
            
            item.transform.parent = transform;
        }
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
    }
    
    public void Reset()
    {
        StopCoroutine(_coroutine);
        
        float speed = 0;
        
        _animationHandler.PlayRunAnimation(speed);

        _carryingItem = null;

        _targetItem = null;
    }
    
    public void StartMoving(Vector3 target)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
        
        _coroutine = StartCoroutine(Move(target));
    }

    private IEnumerator Move(Vector3 target)
    {
        float speed = _mover.Speed;
        
        Vector3 currentTargetPosition = new Vector3(target.x, transform.position.y, target.z);
        
        while (enabled)
        {
            _animationHandler.PlayRunAnimation(speed);
            
            _mover.Move(currentTargetPosition);

            yield return null;
        }
    }
}
