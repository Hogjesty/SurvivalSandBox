using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkeletonManager : MonoBehaviour {

    [SerializeField] private float speed;
    [SerializeField] private Transform player;
    [SerializeField] private Transform hitPoint;
    
    [SerializeField] private Animator skeletonAnimator;
    
    [SerializeField] private TextMeshProUGUI healthAmount;
    [SerializeField] private Slider healthLine;

    private bool isHittingPlayer;
    private bool isHitOnCooldown;
    private bool isDying;

    private float health = 100f;

    private void Start() {
        healthAmount.text = health.ToString(CultureInfo.CurrentCulture);
    }

    private void Update() {

        if (isDying) {
            return;
        }
        
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

    public void SetDamage(float damage) {
        health -= damage;
        healthAmount.text = health.ToString(CultureInfo.CurrentCulture);
        healthLine.value = health;
        if (health <= 0) {
            healthAmount.text = "0";
            healthLine.value = 0f;
            isDying = true;
            StartCoroutine(ProceedDeath());
        }

        if (!isHittingPlayer) {
            skeletonAnimator.SetTrigger("HitBy");
        }
    }

    private IEnumerator ProceedDeath() {
        skeletonAnimator.SetTrigger("Death");
        yield return new WaitForSeconds(1.2f);
        Destroy(gameObject);
    }

    private IEnumerator HandleHit() {
        skeletonAnimator.SetBool("IsHittingPlayer", true);
        yield return new WaitForSeconds(0.7f);

        bool playerDetected = Physics.OverlapSphere(hitPoint.position, 0.5f)
            .Where(x => x.gameObject.CompareTag("Player"))
            .ToList().Count > 0;

        if (playerDetected) {
            Debug.Log("Player got hit!!!");
        }
        
        skeletonAnimator.SetBool("IsHittingPlayer", false);
        yield return new WaitForSeconds(0.2f);
        isHittingPlayer = false;
        yield return new WaitForSeconds(2f);
        isHitOnCooldown = false;
    }

    private void RotateToPlayer() {
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.05f);
    }
    
    // private void OnDrawGizmos() {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawSphere(hitPoint.position, 0.5f);
    // }
}
