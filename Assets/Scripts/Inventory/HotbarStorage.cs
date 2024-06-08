using System.Data;
using UnityEngine;

namespace Inventory {
    public class HotbarStorage : MonoBehaviour, IStorage {
        
        [SerializeField] private ItemData[] storage = new ItemData[(int) StorageType.Hotbar];
        
        public ItemData[] GetStorage() {
            return storage;
        }

        public StorageType GetStorageType() {
            return StorageType.Hotbar;
        }
        
        private void OnValidate() {
            if (storage.Length != (int) StorageType.Hotbar) {
                throw new DataException($"Hotbar storage should be {(int) StorageType.Hotbar} size");
            }
        }
    }
}