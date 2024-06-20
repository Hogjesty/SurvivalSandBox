using System.Collections.Generic;

namespace Inventory {
    public interface IStorage {
        ItemData[] GetStorage();
        StorageType GetStorageType();
    }
    
    public enum StorageType {
        Player = 20, Chest = 21, Hotbar = 4
    }
}