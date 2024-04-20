using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerInteract : MonoBehaviour {

    [SerializeField] private Transform mainCamera;
    [SerializeField] private Transform startRayPoint;
    [SerializeField] private GameObject interactPopupCanvas;
    [SerializeField] private Animator playerAnimator;

    private bool hasCollider;

    private Vector3 pickedObjPos;
    
    void Update() {
        hasCollider = Physics.Raycast(startRayPoint.position, mainCamera.forward, out RaycastHit hitInfo, 5f);
        if (hasCollider) {
            GameObject transformGameObject = hitInfo.transform.gameObject;
            bool isGameObjectPickable = transformGameObject.CompareTag("Pickable");
            interactPopupCanvas.SetActive(isGameObjectPickable);
            if (isGameObjectPickable) {
                Vector3 objPos = hitInfo.transform.position;
                interactPopupCanvas.transform.position = new Vector3(objPos.x, objPos.y + 0.5f, objPos.z);
            }

            if (Input.GetKeyDown(KeyCode.E) && isGameObjectPickable) {
                Debug.Log(transformGameObject.name + " was picked up");


                playerAnimator.SetBool("PickUp", true);
                pickedObjPos = transformGameObject.transform.position;
                Destroy(transformGameObject);
            } else {
                playerAnimator.SetBool("PickUp", false);
            }
        } else {
            interactPopupCanvas.SetActive(false);
        }
        
        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("PickUp")) {
            Vector3 directionToStone = pickedObjPos - transform.position;
            directionToStone.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(directionToStone);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.05f);
        }
    }

    public void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Vector3 dir = pickedObjPos - transform.position;
        Gizmos.DrawSphere(dir, 0.2f);
    }
}