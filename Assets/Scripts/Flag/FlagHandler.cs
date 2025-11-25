using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlagHandler : MonoBehaviour
{
    [SerializeField] private Flag _flagPrefab;
    [SerializeField] private TextMeshProUGUI _text;

    private Flag _flag;

    private void OnEnable()
    {
        _text.gameObject.SetActive(false);
    }

    public void ShowText()
    {
        _text.gameObject.SetActive(true);
    }
    
    public void CloseText()
    {
        _text.gameObject.SetActive(false);
    }

    public Flag CreateFlag(Vector3 position)
    {
        return Instantiate(_flagPrefab, position, Quaternion.identity);
    }

    public void ChangePosition(Flag flag, Vector3 position)
    {
        flag.gameObject.transform.position = position;
    }
}
