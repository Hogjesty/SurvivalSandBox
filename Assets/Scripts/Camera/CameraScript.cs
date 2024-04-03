using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {
    [SerializeField] private float verticalSensitivity;
    [SerializeField] private float horizontalSensitivity;
    [SerializeField] private float lerpRatio;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private LayerMask obstacle;
    
    private float cameraDistance;

    private float axisY;
    private float axisX;

    private void Start() {
        axisX = 0;
        axisY = 0;

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate() {
        axisX += Input.GetAxis("Mouse X") * Time.deltaTime * verticalSensitivity;
        axisY -= Input.GetAxis("Mouse Y") * Time.deltaTime * horizontalSensitivity;
        float mouseWheel = Input.GetAxis("Mouse ScrollWheel") * -15;
        
        axisY = Mathf.Clamp(axisY, -85f, 85f);
        cameraDistance = Mathf.Clamp(cameraDistance + mouseWheel, 1, 30);
    
        Quaternion newRotation = Quaternion.Euler(axisY, axisX, 0f);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, lerpRatio);
        
        transform.position = playerTransform.position - transform.forward * cameraDistance;
        ObstacleReact();
    }

    private void ObstacleReact() {
        bool didHitWall = Physics.Raycast(
            playerTransform.position,
            transform.position - playerTransform.position,
            out RaycastHit hit,
            cameraDistance,
            obstacle);
        
        if (didHitWall) {
            transform.position = hit.point;
        }
    }

    // private void OnDrawGizmos() {
    //     Gizmos.color = Color.red;
    //     Vector3 dir = transform.position - playerTransform.position;
    //     Gizmos.DrawRay(playerTransform.position, dir);
    // }
}
