using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetectetor : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private LayerMask _layerMask;

    private float _maxDistance = 100000;

    public event Action<Base> CollisionDetected;

    private void OnEnable()
    {
        _inputReader.Clicked += IsRayCollided;
    }

    private void OnDisable()
    {
        _inputReader.Clicked -= IsRayCollided;
    }

    private void IsRayCollided(Vector3 mousePosition)
    {
        Ray ray = _camera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, _maxDistance) && hit.collider.TryGetComponent(out Base @base))
        {
            CollisionDetected?.Invoke(@base);
        }
    }
}
