using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasesHandler : MonoBehaviour
{
    [SerializeField] private HitDetector _hitDetector;
    [SerializeField] private FlagHandler _flagHandler;

    private Base _base;
    private Flag _flag;
    
    private void OnEnable()
    {
        _hitDetector.BaseSelected += ShowMessage;
        _hitDetector.GroundSelected += InitiateBuilding;
    }
    
    private void OnDisable()
    {
        _hitDetector.BaseSelected -= ShowMessage;
        _hitDetector.GroundSelected -= InitiateBuilding;
    }

    private void ShowMessage(Base @base)
    {
        if (@base.IsFlagPlaced == false)
        {
            _base = @base;
            
            _flagHandler.ShowText();
        }
    }

    private void InitiateBuilding(Vector3 position)
    {
        if (_base != null)
        {
            _flagHandler.PlaceFlag(position);
            
            _base.ChangePriority();
        }
    }
}
