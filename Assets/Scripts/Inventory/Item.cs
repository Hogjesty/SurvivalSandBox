using System;
using JetBrains.Annotations;
using UnityEngine;
using WorldResources;

namespace Inventory {
    public class Item : MonoBehaviour {
        public enum  Type {
            Stone,
            Tree
        }

         private Resource r;
        [SerializeField] private Type type;
        private void Awake() 
        {
            switch (type) {
                case Type.Stone:
                    r = (r as Stone).ReturnObject();
                    break;
                case  Type.Tree:
                    r = (r as Stone).ReturnObject();
                    break;
            }
        }
    }
}