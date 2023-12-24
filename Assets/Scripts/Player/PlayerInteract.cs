using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteract : MonoBehaviour {

    [SerializeField] private Transform camera;
    [SerializeField] private Transform startRayPoint;
    [SerializeField] private GameObject interactPopupCanvas;
    [SerializeField] private Animator playerAnimator;

    private Vector3 rayHit;
    private bool hasCollider;
    
    void Update() {
        hasCollider = Physics.Raycast(startRayPoint.position, camera.forward, out RaycastHit hitInfo, 5f);
        if (hasCollider) {
            rayHit = hitInfo.point;
            GameObject gameObject = hitInfo.transform.gameObject;
            bool isGameObjectPickable = gameObject.CompareTag("Pickable");
            interactPopupCanvas.SetActive(isGameObjectPickable);
            if (isGameObjectPickable) {
                Vector3 objPos = hitInfo.transform.position;
                interactPopupCanvas.transform.position = new Vector3(objPos.x, objPos.y + 0.5f, objPos.z);
            }

            if (Input.GetKeyDown(KeyCode.E) && isGameObjectPickable) {
                Debug.Log(gameObject.name + " was picked up");
                Destroy(gameObject);
                playerAnimator.SetTrigger("IsPickingUp");
            }
        } else {
            interactPopupCanvas.SetActive(false);
        }
    }

    // public void OnDrawGizmos() {
    //     Gizmos.color = Color.green;
    //     if (hasCollider) Gizmos.DrawSphere(rayHit, 0.2f);
    // }
}