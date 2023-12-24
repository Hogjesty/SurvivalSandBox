using System.Collections;
using System.Linq;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {
    [SerializeField] private float speed; //12
    [SerializeField] private float rotationSpeed; // 0.2
    [SerializeField] private float gravitySpeed; // 35
    [SerializeField] private float jumpHigh; // 11
    [SerializeField] private float sprintCoefficient; // 2

    [SerializeField] private Transform camera;
    [SerializeField] private Transform groundPoint;
    [SerializeField] private Animator playerAnimator;

    private Rigidbody rigidbody;
    private CapsuleCollider collider;

    private bool onGround;
    private float gravity = -10;
    private float moveForward;

    private bool isReadyToJump = true;
    private bool isReadyToLand;

    private int movingState; // 0 - Idle, 1 - Jogging, 2 - Running, 3 - Crouching
    private bool isCrouching;
    private bool isSprinting;

    private void Awake() {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();
    }

    private void Update() {
        Move();
    }

    private void Move() {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.LeftControl) && !isSprinting) {
            isCrouching = !isCrouching;
            if (isCrouching) {
                collider.center = new Vector3(collider.center.x, 0.65f, collider.center.z);
                collider.height = 1.3f;
            } else {
                collider.center = new Vector3(collider.center.x, 0.9f, collider.center.z);
                collider.height = 1.8f; 
            }
            // if crouch
            //center.y = 0.65; Height = 1.3
            // else
            //center.y = 0.9; Height = 1.8
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isCrouching) {
            isSprinting = !isSprinting;
        }

        movingState = 0;
        
        moveForward = (new Vector3(horizontal, 0, vertical).normalized * speed).magnitude;
        bool isPlayerMoving = moveForward > 0;
        if (isPlayerMoving) {
            float rotationY = camera.rotation.eulerAngles.y + Mathf.Atan2(horizontal, vertical) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, rotationY, 0), rotationSpeed);
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
        rigidbody.velocity = transform.TransformDirection(0, gravity, moveForward);
    }

    private void HandleJump() {
        if (isCrouching) {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isReadyToJump) {
            isReadyToJump = false;
            playerAnimator.SetTrigger("Jump");
            StartCoroutine(DelayJump());
        }
    }

    private void HandleGravity() {
        bool isPlayerOnGround = Physics.OverlapSphere(groundPoint.position, 0.2f)
            .Where(x => !x.gameObject.CompareTag("Player"))
            .ToList().Count > 0;
        
        playerAnimator.SetBool("IsFalling", !isPlayerOnGround);
        
        if (!isPlayerOnGround) {
            isReadyToLand = true;
        }

        if (isReadyToLand && isPlayerOnGround) {
            isReadyToLand = false;
            StartCoroutine(DelayReadyToJump());
        }

        if (moveForward != 0 && !isReadyToJump) {
            moveForward /= 3;
        }
        
        gravity -= Time.deltaTime * gravitySpeed;
        gravity = Mathf.Clamp(gravity, isPlayerOnGround ? -3 : -30, 100);
    }

    private IEnumerator DelayJump() {
        yield return new WaitForSeconds(0.3f);
        gravity = jumpHigh;
    }

    private IEnumerator DelayReadyToJump() {
        yield return new WaitForSeconds(0.25f);
        isReadyToJump = true;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Vector3 grPos = groundPoint.position;
        // Gizmos.DrawLine(grPos, new Vector3(grPos.x, grPos.y - 0.3f, grPos.z));
        Gizmos.DrawSphere(grPos, 0.2f);
    }
}