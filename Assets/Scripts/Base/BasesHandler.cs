using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class BasesHandler : MonoBehaviour
{
    [SerializeField] private HitDetector _hitDetector;
    [SerializeField] private FlagHandler _flagHandler;

    private Base _base;
    private Flag _flag;

    private void OnEnable()
    {
        _hitDetector.BaseSelected += SelectBase;
        _hitDetector.GroundSelected += InitiateBuilding;
    }

    private void OnDisable()
    {
        _hitDetector.BaseSelected -= SelectBase;
        _hitDetector.GroundSelected -= InitiateBuilding;
    }

    private void SelectBase(Base @base)
    {
        _base = @base;

        _flagHandler.ShowText();
    }

    private void InitiateBuilding(Vector3 position)
    {
        if (_base != null)
        {
            if (_base.IsFlagPlaced)
            {
                Flag baseFlag = _base.GetTargetFlag();

                _flagHandler.ChangePosition(baseFlag, position);
            }
            else
            {
                Flag tempFlag = _flagHandler.CreateFlag(position);

                _base.ChangePriority();

                _base.SetTargetFlag(tempFlag);
            }
        }

        _flagHandler.CloseText();

        _base = null;
    }
}
