﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySounds : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Destroy(this.gameObject, 1.5f);
    }
}
