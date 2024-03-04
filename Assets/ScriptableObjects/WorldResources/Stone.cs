using System;
using UnityEngine;

namespace ScriptableObjects.WorldResources {
    [CreateAssetMenu(fileName = "InventoryItems", menuName = "WorldResources/Stone")]
    public class Stone : Resource {
        protected override void OnCreate() {
            Debug.Log("Stone has been created");
        }
    }
}