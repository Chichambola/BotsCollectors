using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private float _speed;
    
    private Coroutine _coroutine;

    private void OnDisable()
    {
        StopMoving();
    }

    public void StartMoving(Vector3 targetPosition)
    {
        StopMoving();
        
        _coroutine = StartCoroutine(Move(targetPosition));
    }

    public void StopMoving()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }
    
    private IEnumerator Move(Vector3 targetPosition)
    {
        while (transform.position != targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);
            
            yield return null;
        }
    }
}
