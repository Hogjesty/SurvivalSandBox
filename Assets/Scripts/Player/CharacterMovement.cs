using System.Collections;
using System.Linq;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {
    [SerializeField] private float speed; //12
    [SerializeField] private float rotationSpeed; // 0.11
    [SerializeField] private float gravitySpeed; // 35
    [SerializeField] private float jumpHigh; // 11

    [SerializeField] private Transform camera;
    [SerializeField] private Transform groundPoint;

    [SerializeField] private Animator playerAnimator;

    private Rigidbody rigidbody;

    private bool onGround;
    private float gravity = -10;
    private float moveForward;

    private bool isReadyToJump = true;
    private bool isJumping;
    private bool isReadyToLand;

    private void Awake() {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update() {
        Move();
    }

    private void Move() {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        moveForward = (new Vector3(horizontal, 0, vertical).normalized * speed).magnitude;

        bool isPlayerMoving = moveForward > 0;
        
        playerAnimator.SetBool("IsRunning", isPlayerMoving);
        
        if (isPlayerMoving) {
            float rotationY = camera.rotation.eulerAngles.y + Mathf.Atan2(horizontal, vertical) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, rotationY, 0), rotationSpeed);
        }

        HandleJump();
        
        rigidbody.velocity = transform.TransformDirection(0, gravity, moveForward);
    }

    private void HandleJump() {
        bool isPlayerOnGround = Physics.OverlapSphere(groundPoint.position, 0.2f)
                .Where(x => !x.gameObject.CompareTag("Player"))
                .ToList().Count > 0;

        if (Input.GetKeyDown(KeyCode.Space) && isReadyToJump) {
            isJumping = true;
            isReadyToJump = false;
            StartCoroutine(DelayJump());
        }

        if (isJumping && !isPlayerOnGround) {
            isReadyToLand = true;
        }

        if (isReadyToLand && isPlayerOnGround) {
            isJumping = false;
            isReadyToLand = false;
            StartCoroutine(DelayReadyToJump());
        }

        if (moveForward != 0 && !isReadyToJump) {
            moveForward /= 5;
        }

        playerAnimator.SetBool("IsPlayerJumping", isJumping);

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