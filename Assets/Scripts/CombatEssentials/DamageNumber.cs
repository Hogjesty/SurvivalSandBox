using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class DamageNumber : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI damageText;
    private Vector3 lookAtPosition;

    public void PostInit(int number, Vector3 lookAtPos) {
        damageText.text = number.ToString();
        lookAtPosition = lookAtPos;
        StartCoroutine(DelayDestroy());
    }

    private void Update() {
        transform.LookAt(lookAtPosition);
    }

    private IEnumerator DelayDestroy() {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}