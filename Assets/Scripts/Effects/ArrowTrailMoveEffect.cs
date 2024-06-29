using System;
using System.Collections;
using UnityEngine;

namespace Effects {
    public class ArrowTrailMoveEffect : MonoBehaviour {
        [SerializeField] private float speed;
        
        public Vector3 Destination { set; private get; }

        private bool isInDestroying;

        private void Update() {
            if (isInDestroying) return;
            Vector3 moveTowards = Vector3.MoveTowards(transform.position, Destination, speed * Time.deltaTime);
            transform.position = moveTowards;
            
            if (Vector3.Distance(transform.position, Destination) <= 0.1f) {
                Destroy(gameObject,2);
                isInDestroying = true;
            }
        }
        
       
    }
}