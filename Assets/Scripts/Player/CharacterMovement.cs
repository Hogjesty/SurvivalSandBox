using System.Collections;
using System.Linq;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {
    [SerializeField] private float speed; //5
    [SerializeField] private float rotationSpeed; // 0.2
    [SerializeField] private float gravitySpeed; // 35
    [SerializeField] private float jumpForce; // 11
    [SerializeField] private float sprintCoefficient; // 2

    [SerializeField] private Transform mainCamera;
    [SerializeField] private Transform groundPoint;
    [SerializeField] private Transform roofPoint;
    [SerializeField] private Animator playerAnimator;

    private Rigidbody playerRigidbody;
    private CapsuleCollider playerCollider;

    private bool onGround;
    private float gravity = -10;
    private float moveForward;
    private float remainedSpeed;

    private bool isReadyToJump = true;
    private bool isReadyToLand;

    private int movingState; // 0 - Idle, 1 - Jogging, 2 - Sprint, 3 - Crouching
    private bool isCrouching;
    private bool isSprinting;

    private void Awake() {
        playerRigidbody = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();
        
        remainedSpeed = speed;
    }

    private void Update() {
        Move();
    }

    private void Move() {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.LeftControl) && !isSprinting) {
            bool canStay = !Physics.Raycast(roofPoint.position, Vector3.up, 0.7f);
            if (isCrouching && canStay) {
                isCrouching = false;
                playerCollider.center = new Vector3(playerCollider.center.x, 0.9f, playerCollider.center.z);
                playerCollider.height = 1.8f;
            } else if (!isCrouching) {
                isCrouching = true;
                playerCollider.center = new Vector3(playerCollider.center.x, 0.65f, playerCollider.center.z);
                playerCollider.height = 1.3f;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isCrouching) {
            isSprinting = !isSprinting;
        }

        movingState = 0;
        
        moveForward = (new Vector3(horizontal, 0, vertical).normalized * speed).magnitude;
        bool isPlayerMoving = moveForward > 0;
        if (isPlayerMoving) {
            float rotationY = mainCamera.rotation.eulerAngles.y + Mathf.Atan2(horizontal, vertical) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, rotationY, 0), isReadyToJump ? rotationSpeed : rotationSpeed / 5);
            movingState = isSprinting ? 2 : 1;
            moveForward = isSprinting ? moveForward * sprintCoefficient : moveForward;
        } else {
            isSprinting = false;
        }

        if (isPlayerMoving && isCrouching) {
            movingState = 3;
            moveForward /= 2;
        }
        
        playerAnimator.SetBool("IsCrouching", isCrouching);
        playerAnimator.SetInteger("MovingState", movingState);

        HandleJump();
        HandleGravity();
        playerRigidbody.velocity = transform.TransformDirection(0, gravity, moveForward);
    }

    private void HandleJump() {
        if (Input.GetKeyDown(KeyCode.Space) && isReadyToJump && !isCrouching) {
            isReadyToJump = false;
            playerAnimator.SetTrigger("Jump");
            if (movingState is 1 or 2) {
                gravity = jumpForce;
            } else {
                remainedSpeed = speed;
                speed /= 3;
                StartCoroutine(DelayJump());                
            }
        }
    }

    private void HandleGravity() {
        bool isPlayerOnGround = Physics.OverlapSphere(groundPoint.position, 0.15f)
            .Where(x => !x.gameObject.CompareTag("Player"))
            .ToList().Count > 0;
        
        playerAnimator.SetBool("IsFalling", !isPlayerOnGround);
        
        if (!isPlayerOnGround) {
            isReadyToLand = true;
            isReadyToJump = false;
        }

        if (isReadyToLand && isPlayerOnGround) {
            isReadyToLand = false;
            StartCoroutine(DelayReadyToJump());
        }
        
        gravity -= Time.deltaTime * gravitySpeed;
        gravity = Mathf.Clamp(gravity, isPlayerOnGround ? -3 : -30, 100);
    }

    private IEnumerator DelayJump() {
        yield return new WaitForSeconds(0.3f);
        gravity = jumpForce;
    }

    private IEnumerator DelayReadyToJump() {
        yield return new WaitForSeconds(0.25f);
        isReadyToJump = true;
        speed = remainedSpeed;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(roofPoint.position, new Vector3(roofPoint.position.x, roofPoint.position.y + 0.7f, roofPoint.position.z));
        Gizmos.DrawSphere(groundPoint.position, 0.15f);
    }
}