using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class BuildHandler : MonoBehaviour
{
    [SerializeField] private HitDetector _hitDetector;
    [SerializeField] private FlagHandler _flagHandler;
    [SerializeField] private BaseSpawner _baseSpawner;
    [SerializeField] private Vector3 _positionOffset;

    private Base _base;

    private void OnEnable()
    {
        _hitDetector.BaseSelected += SelectBase;
        _hitDetector.GroundSelected += PlaceFlag;
        _flagHandler.FlagReached += BuildBase;
    }

    private void OnDisable()
    {
        _hitDetector.BaseSelected -= SelectBase;
        _hitDetector.GroundSelected -= PlaceFlag;
        _flagHandler.FlagReached -= BuildBase;
    }

    private void SelectBase(Base @base)
    {
        _base = @base;

        _base.ChangeSelection(true);
        
        _flagHandler.ShowText();
    }

    private void PlaceFlag(Vector3 position)
    {
        int minCollectorsAmount = 1;
        
        Debug.Log(_base.CollectorsCount);
        
        if (_base.IsSelected)
        {
            if (_base.CollectorsCount > minCollectorsAmount)
            {
                if (_base.IsFlagPlaced)
                {
                    Flag tempFlag = _base.GetTargetFlag();

                    tempFlag.transform.position = _flagHandler.GetNewPosition(tempFlag, position);
                
                    _base.SetTargetFlag(tempFlag);
                
                    if (_base.HasFlagCollector)
                    {
                        _base.ChangeDirection();
                    }
                }
                else
                {
                    Flag tempFlag = _flagHandler.CreateFlag(position);

                    _base.ChangePriority(false);

                    _base.SetTargetFlag(tempFlag);
                }
            }
        }

        _flagHandler.CloseText();
        
        _base.ChangeSelection(false);
    }

    private void BuildBase(Vector3 position)
    {
        position += _positionOffset;
        
        Collector flagCollector = _base.GetFlagCollector();
        
        _base.RemoveFlagCollector(flagCollector);
        
        _baseSpawner.SetCollector(flagCollector);
        
        _base.ChangePriority(true);
        
        _base = null;
        
        _baseSpawner.StartSpawning(position);
    }
}
