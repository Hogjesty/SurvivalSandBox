using System.Collections;
using System.Linq;
using UnityEngine;

public class SkeletonManager : MonoBehaviour {

    [SerializeField] private float speed;
    [SerializeField] private Transform player;
    [SerializeField] private Transform hitPoint;
    
    [SerializeField] private Animator skeletonAnimator;

    private bool isHittingPlayer;
    private bool isHitOnCooldown;

    private void Update() {
        float distance = Vector3.Distance(player.position, transform.position);
        
        bool canHit = distance <= 1.01f && !isHitOnCooldown;
        
        if (canHit) {
            isHitOnCooldown = true;
            isHittingPlayer = true;
            StartCoroutine(HandleHit());
        }
        if (isHittingPlayer) {
            RotateToPlayer();
        }
        
        skeletonAnimator.SetBool("IsWalking", false);

        bool isHittingPlayer2 = skeletonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack");
        if (distance is > 1f and < 10f && !isHittingPlayer2) {
            skeletonAnimator.SetBool("IsWalking", true);
            RotateToPlayer();
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }

    private IEnumerator HandleHit() {
        skeletonAnimator.SetBool("IsHittingPlayer", true);
        yield return new WaitForSeconds(1f);

        bool playerDetected = Physics.OverlapSphere(hitPoint.position, 0.5f)
            .Where(x => x.gameObject.CompareTag("Player"))
            .ToList().Count > 0;

        if (playerDetected) {
            Debug.Log("Player got hit!!!");
        }
        
        skeletonAnimator.SetBool("IsHittingPlayer", false);
        yield return new WaitForSeconds(4f);
        isHitOnCooldown = false;
        isHittingPlayer = false;
    }

    private void RotateToPlayer() {
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.05f);
    }
}
