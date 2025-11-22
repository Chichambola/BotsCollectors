using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class ObjectInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private string InitialValue = "0";

    private void Start()
    {
        _text.text = InitialValue;
    }

    public void UpdateValue(int value)
    {
        _text.text = value.ToString();
    }
}
