using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class FlagHandler : MonoBehaviour
{
    [SerializeField] private Flag _flagPrefab;
    [SerializeField] private TextMeshProUGUI _text;

    public event Action<Vector3> FlagReached;
    
    private Flag _flag;
    
    private void OnEnable()
    {
        _text.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        CloseText();
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
        _flag = Instantiate(_flagPrefab, position, Quaternion.identity);

        _flag.CanBeDestroyed += RemoveFlag;
        
        return _flag;
    }

    public Vector3 GetNewPosition(Flag flag, Vector3 position)
    {
        return flag.gameObject.transform.position = position;
    }

    private void RemoveFlag(Flag flag)
    {
        flag.CanBeDestroyed -= RemoveFlag;
        
        FlagReached?.Invoke(flag.transform.position);
        
        Destroy(flag.gameObject);
    }
}
