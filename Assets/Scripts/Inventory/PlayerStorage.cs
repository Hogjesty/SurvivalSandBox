using System;
using System.Data;
using UnityEngine;

namespace Inventory {
    public class PlayerStorage : MonoBehaviour, IStorage {
        
        [SerializeField] private ItemData[] storage = new ItemData[(int) StorageType.Player];

        public ItemData[] Storage => storage;
        
        public ItemData[] GetStorage() {
            return storage;
        }

        public StorageType GetStorageType() {
            return StorageType.Player;
        }

        private void OnValidate() {
            if (storage.Length != (int) StorageType.Player) {
                throw new DataException($"Player storage should be {(int) StorageType.Player} size");
            }
        }
    }
}