using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlagHandler : MonoBehaviour
{
    [SerializeField] private Flag _flagPrefab;
    [SerializeField] private TextMeshProUGUI _text;
    
    private void OnEnable()
    {
        _text.gameObject.SetActive(false);
    }

    public void ShowText()
    {
        _text.gameObject.SetActive(true);
    }
    
    public void PlaceFlag(Vector3 position)
    {
        _text.gameObject.SetActive(false);
        
        Instantiate(_flagPrefab, position, Quaternion.identity);
    }
}
