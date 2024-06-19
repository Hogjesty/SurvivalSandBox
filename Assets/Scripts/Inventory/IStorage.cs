using System.Collections.Generic;

namespace Inventory {
    public interface IStorage {
        ItemData[] GetStorage();
        StorageType GetStorageType();
    }
    
    public enum StorageType {
        Player = 20, Chest = 16, Hotbar = 4
    }
}