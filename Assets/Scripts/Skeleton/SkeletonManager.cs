using UnityEngine;

public class SkeletonManager : MonoBehaviour {

    [SerializeField] private float speed;
    [SerializeField] private Transform player;
    
    [SerializeField] private Animator skeletonAnimator;

    private bool isHittingPlayer;

    private void Update() {
        float distance = Vector3.Distance(player.position, transform.position);
        bool canHit = distance <= 1.01f;
        
        skeletonAnimator.SetBool("IsHittingPlayer", canHit);
        if (canHit) {
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

    private void RotateToPlayer() {
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.05f);
    }
}
