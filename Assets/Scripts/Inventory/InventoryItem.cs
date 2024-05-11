namespace Inventory {
    public class InventoryItem {
        public CellUI cellUI { get; }
        public ResourceSO resourceSo;
        public int amount;

        public InventoryItem(CellUI cellUI, ResourceSO resourceSo, int amount) {
            this.cellUI = cellUI;
            this.resourceSo = resourceSo;
            this.amount = amount;
        }
    }
}