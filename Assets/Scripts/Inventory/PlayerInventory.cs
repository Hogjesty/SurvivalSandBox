using System.Collections.Generic;
using WorldResources;

namespace Inventory {
    public class PlayerInventory {
        private List<Resource> storage = new List<Resource>();

        public void AddItem(Resource item) {
            storage.Add(item);
        }

        public void RemoveItem(Resource item) {
            storage.Remove(item);
        }
    }
}