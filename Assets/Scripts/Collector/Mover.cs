using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private float _speed;
    
    private Coroutine _coroutine;
    
    public float Speed  => _speed;
    
    public void Move(Vector3 targetPosition)
    {
        transform.LookAt(targetPosition);
        
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);
    }
}
