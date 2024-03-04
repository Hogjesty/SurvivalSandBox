using System;
using UnityEngine;

namespace ScriptableObjects.WorldResources {
    
    
    public abstract class Resource : ScriptableObject {
        [SerializeField] protected Sprite _icon;
        [SerializeField] protected string _name;

        private void Awake() {
            OnCreate();
        }
        
        protected abstract void OnCreate();

        protected Sprite Icon => _icon;
    }
}