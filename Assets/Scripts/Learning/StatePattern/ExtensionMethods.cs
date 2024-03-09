using UnityEngine;

namespace StatePattern {
    public static class ExtensionMethods {
        public static void Rotation(this Transform obj, float angle) {
            obj.Rotate(Vector3.up,angle);
        }
        
        public static void Move(this Transform obj, float speed, Vector3 dir) {
            dir = dir.normalized;
            obj.position += dir * speed * Time.deltaTime;
        }
    }
}