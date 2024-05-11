using UnityEngine;

namespace Inventory {
    [CreateAssetMenu(fileName = "InventoryItems", menuName = "WorldResources/Resource")]
    public class ResourceSO : ScriptableObject {
        [SerializeField] private Sprite icon;
        [SerializeField] private string name;
        [SerializeField] private ResourceType resourceType;
        [SerializeField] private int maxStackSize;
        public Sprite Icon => icon;
        public ResourceType ResourceType => resourceType;
        public int MaxStackSize => maxStackSize;
    }

    public enum ResourceType {
        IRON, STONE, WOOD
    }
}