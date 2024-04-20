using System;
using UnityEngine;
namespace Player.AnimationsEvents {
    public class PlayerAnimationsEvents : MonoBehaviour {
        public static Action endOfWeaponEquippingAnim;
        public static Action endOfWeaponUnequippingAnim;
        public static Action endOfAttackingAnim;
        public static Action endOfPickingUpAnim;
        
        public static Action hitPointAttackingAnim;
        
        public static Action pointOfWeaponEquippingAnim;
        public static Action pointOfWeaponUnequippingAnim;
    
        public void EndOfWeaponEquippingAnim() => endOfWeaponEquippingAnim?.Invoke();
        public void EndOfWeaponUnequippingAnim() => endOfWeaponUnequippingAnim?.Invoke();
        public void EndOfAttackingAnim() => endOfAttackingAnim?.Invoke();
        public void EndOfPickingUpAnim() => endOfPickingUpAnim?.Invoke();
    
        public void HitPointAttackingAnim() => hitPointAttackingAnim?.Invoke();
        
        public void PointOfWeaponEquippingAnim() => pointOfWeaponEquippingAnim?.Invoke();
        public void PointOfWeaponUnequippingAnim() => pointOfWeaponUnequippingAnim?.Invoke();
    }
}
