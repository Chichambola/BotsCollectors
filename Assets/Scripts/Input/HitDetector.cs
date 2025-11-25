using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetector : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private LayerMask _layerMask;

    private float _maxDistance = 100000;

    public event Action<Base> BaseSelected;
    public event Action<Vector3> GroundSelected;

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

        bool isHit = Physics.Raycast(ray, out RaycastHit hit, _maxDistance, _layerMask);
        
        if (isHit && hit.collider.TryGetComponent(out Base @base))
        {
            BaseSelected?.Invoke(@base);
        }

        if (isHit && hit.collider.TryGetComponent(out Ground _))
        {
            GroundSelected?.Invoke(hit.point);
        }
    }
}
