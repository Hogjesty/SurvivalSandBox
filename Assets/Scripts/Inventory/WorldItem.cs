using UnityEngine;
using UnityEngine.Serialization;

namespace Inventory {
    public class WorldItem : MonoBehaviour {
        [SerializeField] private ResourceSO resourceSo;
        public int amount;

        public ResourceSO ResourceSo => resourceSo;
    }
}
