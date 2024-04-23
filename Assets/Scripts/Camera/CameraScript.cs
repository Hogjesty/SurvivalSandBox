using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class CameraScript : MonoBehaviour {
    [SerializeField] private float verticalSensitivity;
    [SerializeField] private float horizontalSensitivity;
    [SerializeField] private float lerpRatio;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private LayerMask obstacle;

    private Camera camera;
    
    private float cameraDistance;
    private float actualCameraDistance;

    private float axisY;
    private float axisX;

    private void Start() {
        camera = GetComponent<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        actualCameraDistance = cameraDistance;
    }
    
    private void LateUpdate() {
        axisX += Input.GetAxis("Mouse X") * Time.deltaTime * verticalSensitivity;
        axisY -= Input.GetAxis("Mouse Y") * Time.deltaTime * horizontalSensitivity;
        float mouseWheel = Input.GetAxis("Mouse ScrollWheel") * -15;
        
        axisY = Mathf.Clamp(axisY, -85f, 85f);
        cameraDistance = Mathf.Clamp(cameraDistance + mouseWheel, 1, 30);
       
        Quaternion newRotation = Quaternion.Euler(axisY, axisX, 0f);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, lerpRatio);
        
        Vector3 position = playerTransform.position - transform.forward * actualCameraDistance;
        
        bool isHittingWall = Physics.Raycast(
            playerTransform.position,
            position - playerTransform.position,
            actualCameraDistance,
            obstacle);
        
        for (int i = 0; i < 8; i++) {
            Vector3 direction = Quaternion.Euler(0, 0, i * 45) * Vector3.up;
            Ray ray = new Ray(transform.position, transform.rotation * direction);
            if (Physics.Raycast(ray, 0.3f, obstacle) || isHittingWall) {
                actualCameraDistance = cameraDistance * 0.7f;
                position = playerTransform.position - transform.forward * actualCameraDistance;
                break;
            }
            actualCameraDistance = cameraDistance;
        }
        
        transform.position = position;
    }
    
    // private void OnDrawGizmos() {
    //     Gizmos.color = Color.red;
    //     Vector3 dir = transform.position - playerTransform.position;
    //     Gizmos.DrawRay(playerTransform.position, dir);
    // }
}
