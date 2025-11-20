using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Vector3 CurrentPosition { get; private set; }

    private void OnEnable()
    {
        CurrentPosition = transform.position;
    }
}
