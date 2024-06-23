using System;
using UnityEngine;
namespace Player.AnimationsEvents {
    public class PlayerAnimationsEvents : MonoBehaviour {
        public static Action endOfSwordEquippingAnim;
        public static Action endOfSwordUnequippingAnim;
        public static Action endOfSwordAttackingAnim;
        public static Action endOfCrossbowEquippingAnim;
        public static Action endOfCrossbowUnequippingAnim;
        public static Action endOfCrossbowAttackingAnim;
        public static Action endOfCrossbowReloadingAnim;
        
        public static Action endOfPickingUpAnim;
        
        public static Action hitPointSwordAttackingAnim;
        
        public static Action pointOfSwordEquippingAnim;
        public static Action pointOfSwordUnequippingAnim;
        public static Action pointOfCrossbowEquippingAnim;
        public static Action pointOfCrossbowUnequippingAnim;
    
        public void EndOfSwordEquippingAnim() => endOfSwordEquippingAnim?.Invoke();
        public void EndOfSwordUnequippingAnim() => endOfSwordUnequippingAnim?.Invoke();
        public void EndOfSwordAttackingAnim() => endOfSwordAttackingAnim?.Invoke();
        public void EndOfCrossbowEquippingAnim() => endOfCrossbowEquippingAnim?.Invoke();
        public void EndOfCrossbowUnequippingAnim() => endOfCrossbowUnequippingAnim?.Invoke();
        public void EndOfCrossbowAttackingAnim() => endOfCrossbowAttackingAnim?.Invoke();
        public void EndOfCrossbowReloadingAnim() => endOfCrossbowReloadingAnim?.Invoke();
        public void EndOfPickingUpAnim() => endOfPickingUpAnim?.Invoke();
    
        public void HitPointSwordAttackingAnim() => hitPointSwordAttackingAnim?.Invoke();
        
        public void PointOfSwordEquippingAnim() => pointOfSwordEquippingAnim?.Invoke();
        public void PointOfSwordUnequippingAnim() => pointOfSwordUnequippingAnim?.Invoke();
        
        public void PointOfCrossbowEquippingAnim() => pointOfCrossbowEquippingAnim?.Invoke();
        public void PointOfCrossbowUnequippingAnim() => pointOfCrossbowUnequippingAnim?.Invoke();
    }
}
