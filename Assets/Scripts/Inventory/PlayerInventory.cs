using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Inventory {
    public class PlayerInventory : MonoBehaviour {
        [SerializeField] private Transform inventoryGrid;
        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private int initInventorySize;

        private List<InventoryItem> storage = new List<InventoryItem>();

        private void Start() {
            for (int i = 0; i < initInventorySize; i++) {
                CellUI instantiate = Instantiate(cellPrefab, inventoryGrid).GetComponent<CellUI>();
                storage.Add(new InventoryItem(instantiate, null, 0));
            }
        }

        public void TryToAddItem(WorldItem newItem) {
            foreach (InventoryItem item in storage) {
                if (item.resourceSo is null || item.resourceSo.MaxStackSize == 1) {
                    continue;
                }
                if (item.resourceSo.ResourceType == newItem.ResourceSo.ResourceType) {
                    if (item.amount < item.resourceSo.MaxStackSize) {
                        AddItem(item, newItem);
                        return;
                    }
                }
            }
            
            foreach (InventoryItem item in storage) {
                if (item.resourceSo is null) {
                    AddItem(item, newItem);
                    return;
                }
            }
            
            print("Not enough space!");
        }

        private void AddItem(InventoryItem item, WorldItem newItem) {
            item.resourceSo = newItem.ResourceSo;
            item.amount += newItem.amount;
            
            if (item.amount > item.resourceSo.MaxStackSize) {
                newItem.amount = item.amount - item.resourceSo.MaxStackSize;
                item.amount = item.resourceSo.MaxStackSize;
                TryToAddItem(newItem);
            }

            item.cellUI.CountText.text = item.amount.ToString();
            item.cellUI.Image.sprite = newItem.ResourceSo.Icon;
        }
    }
}