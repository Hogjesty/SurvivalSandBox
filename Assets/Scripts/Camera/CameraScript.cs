using System;
using Player.StateMachines;
using Player.StateMachines.Movement.States.SubStates;
using UnityEngine;

public class CameraScript : MonoBehaviour {
    [SerializeField] private float verticalSensitivity;
    [SerializeField] private float horizontalSensitivity;
    [SerializeField] private float lerpRatio;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform sphereCastUp;
    [SerializeField] private Transform sphereCastDown;
    [SerializeField] private LayerMask obstacle;
    [SerializeField] private float sphereRadius;
    [SerializeField] private StateMachineManager stateMachineManager;

    [HideInInspector] public bool isRotationFrozen;
    
    private float cameraDistance;

    private float axisY;
    private float axisX;
    
    private void LateUpdate() {
        float mouseWheel = 0;
        if (!isRotationFrozen) {
            axisX += Input.GetAxis("Mouse X") * Time.deltaTime * verticalSensitivity;
            axisY -= Input.GetAxis("Mouse Y") * Time.deltaTime * horizontalSensitivity;
            mouseWheel = Input.GetAxis("Mouse ScrollWheel") * -15;
        }
        
        axisY = Mathf.Clamp(axisY, -85f, 85f);
        cameraDistance = Mathf.Clamp(cameraDistance + mouseWheel, 1, 30);
       
        Quaternion newRotation = Quaternion.Euler(axisY, axisX, 0f);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, lerpRatio);
        
        Vector3 position = playerTransform.position - transform.forward * cameraDistance;
        transform.position = AdjustCameraPosition(position);
    }
    
    private Vector3 AdjustCameraPosition(Vector3 targetPosition) {
        Vector3 sphereCastPosition = sphereCastUp.position;
        if (stateMachineManager.GetMovementCurrentStateName().Equals(nameof(Crouching)) ||
            stateMachineManager.GetMovementCurrentStateName().Equals(nameof(Sneaking))) {
            sphereCastPosition = sphereCastDown.position;
        }
        Ray ray = new Ray(sphereCastPosition, targetPosition - sphereCastPosition);
        bool sphereCast = Physics.SphereCast(ray, sphereRadius, out RaycastHit sphereHit, cameraDistance, obstacle);
        return sphereCast ? sphereHit.point + sphereHit.normal * (sphereRadius + 0.01f) : targetPosition;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(sphereCastUp.position, sphereRadius);
    }
}
