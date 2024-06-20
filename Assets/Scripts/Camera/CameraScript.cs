using UnityEngine;

public class CameraScript : MonoBehaviour {
    [SerializeField] private float verticalSensitivity;
    [SerializeField] private float horizontalSensitivity;
    [SerializeField] private float lerpRatio;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private LayerMask obstacle;

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
        transform.position = AdjustCameraPosition(playerTransform.position, position);
    }
    
    private Vector3 AdjustCameraPosition(Vector3 playerPosition, Vector3 targetPosition) {
        Ray ray = new Ray(playerPosition, targetPosition - playerPosition);
        bool sphereCast = Physics.SphereCast(ray, 0.3f, out RaycastHit sphereHit, cameraDistance, obstacle);
        return sphereCast ? sphereHit.point + sphereHit.normal * 0.31f : targetPosition;
    }
}
