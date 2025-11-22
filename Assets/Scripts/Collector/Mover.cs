using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Mover : MonoBehaviour
{
    [SerializeField] private float _speed;
    
    private Coroutine _coroutine;
    
    public float Speed  => _speed;
    
    public void StartMoving(Vector3 target)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(Move(target));
    }

    public IEnumerator Move(Vector3 targetPosition)
    {
        Vector3 currentTargetPosition = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);

        while(enabled)
        {
            transform.LookAt(currentTargetPosition);

            transform.position = Vector3.MoveTowards(transform.position, currentTargetPosition, _speed * Time.deltaTime);

            yield return null;
        }
    }

    public void StopMoving()
    {
        StopCoroutine(_coroutine);
    }
}
