using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory {
    public class Hotbar : MonoBehaviour {
        [SerializeField] private List<CellUI> hotbarCells;
        [SerializeField] private InventoriesManager inventoriesManager;

        private void Update() {
            for (int i = 49, j = 0; i <= 57; i++, j++) { // key codes
                if (Input.GetKeyDown((KeyCode)i)) {
                    TryToMove(hotbarCells[j]);
                }
            }
        }

        private void TryToMove(CellUI hotbarCell) {
            if (inventoriesManager.lastHoveredCell is null) {
                return;
            }

            CellUI departmentCell = inventoriesManager.lastHoveredCell.GetComponent<CellUI>();

            if (hotbarCell.inventoryItem.resourceSo is null) {
                inventoriesManager.TransferItem(departmentCell, hotbarCell);
                return;
            }
            
            inventoriesManager.SwapItems(departmentCell, hotbarCell);
        }
    }
}