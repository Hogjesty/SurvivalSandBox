using System;
using UnityEngine;

namespace WorldResources {
    [CreateAssetMenu(fileName = "InventoryItems", menuName = "WorldResources/Stone")]
    public class Stone : Resource {
        public Stone ReturnObject() {
            return this;
        }
        protected override void OnCreate() {
            Debug.Log("Stone has been created");
        }
    }
}