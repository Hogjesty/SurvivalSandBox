using System.Data;
using UnityEngine;

namespace Inventory {
    public class Chest : MonoBehaviour, IStorage {
        
        [SerializeField] private ItemData[] storage = new ItemData[(int) StorageType.Chest];

        public ItemData[] GetStorage() {
            return storage;
        }

        public StorageType GetStorageType() {
            return StorageType.Chest;
        }
        
        private void OnValidate() {
            if (storage.Length != (int) StorageType.Chest) {
                throw new DataException($"Chest storage should be {(int) StorageType.Chest} size");
            }
        }
    }
}