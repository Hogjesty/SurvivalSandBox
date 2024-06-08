using UnityEngine;

public class CameraScript : MonoBehaviour {
    [SerializeField] private float verticalSensitivity;
    [SerializeField] private float horizontalSensitivity;
    [SerializeField] private float lerpRatio;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private LayerMask obstacle;

    private Camera camera;
    
    private float cameraDistance;

    private float axisY;
    private float axisX;

    private void Start() {
        camera = GetComponent<Camera>();
    }
    
    private void LateUpdate() {
        axisX += Input.GetAxis("Mouse X") * Time.deltaTime * verticalSensitivity;
        axisY -= Input.GetAxis("Mouse Y") * Time.deltaTime * horizontalSensitivity;
        float mouseWheel = Input.GetAxis("Mouse ScrollWheel") * -15;
        
        axisY = Mathf.Clamp(axisY, -85f, 85f);
        cameraDistance = Mathf.Clamp(cameraDistance + mouseWheel, 1, 30);
       
        Quaternion newRotation = Quaternion.Euler(axisY, axisX, 0f);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, lerpRatio);
        
        Vector3 position = playerTransform.position - transform.forward * cameraDistance;
        transform.position = AdjustCameraPosition(playerTransform.position, position);
    }
    
    private Vector3 AdjustCameraPosition(Vector3 origin, Vector3 targetPosition) {
        if (Physics.Raycast(origin, targetPosition - origin, out RaycastHit hit, cameraDistance, obstacle)) {
            return hit.point + hit.normal * 0.3f;
        }

        for (int i = 0; i < 8; i++) {
            Vector3 direction = Quaternion.Euler(0, 0, i * 45) * Vector3.up;
            Vector3 rayDir = transform.rotation * direction;
            Ray ray = new Ray(targetPosition, rayDir);
            Debug.DrawRay(targetPosition, rayDir, Color.red, 0.2f);
            if (Physics.Raycast(ray, out RaycastHit hit2, 0.2f, obstacle)) {
                return hit2.point - rayDir.normalized * 0.19f;
            }
        }

        return targetPosition;
    }
    
    // private void OnDrawGizmos() {
    //     Gizmos.color = Color.red;
    //     Vector3 dir = transform.position - playerTransform.position;
    //     Gizmos.DrawRay(playerTransform.position, dir);
    // }
}
