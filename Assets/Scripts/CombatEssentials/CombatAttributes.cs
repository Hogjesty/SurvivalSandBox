using System;
using UnityEngine;

namespace CombatEssentials {
    public class CombatAttributes : MonoBehaviour {
        [SerializeField] private int health;
        public Action<int> takingDamage;
        public Action death;
        
        public void TakeDamage(int damage) {
            health -= damage;
            if (health <= 0) {
                death.Invoke();
                return;
            }
            takingDamage?.Invoke(damage);
        }

        public int GetHealth => health;
    }
}