using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagHandler : MonoBehaviour
{
    [SerializeField] private Flag _flagPrefab;

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    private void PlaceFlag(Base @base, Vector3 position)
    {
        Instantiate(_flagPrefab, position, Quaternion.identity);
    }
}
