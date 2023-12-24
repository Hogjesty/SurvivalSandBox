using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour {
    
    [SerializeField] private Transform point;
    
    private void LateUpdate() {
        transform.LookAt(point);
    }
}
