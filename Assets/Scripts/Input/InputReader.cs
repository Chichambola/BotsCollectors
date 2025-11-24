using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputReader : MonoBehaviour
{
    private const int CLick = 0;

    public event Action<Vector3> Clicked;

    private void Update()
    {
        if (Input.GetMouseButtonDown(CLick))
        {
            Clicked?.Invoke(Input.mousePosition);
        }
    }
}
