using System;
using System.Collections.Generic;
using System.Linq;
using Player.StateMachines.Movement;
using UnityEngine;

namespace Inventory {
    public class PlayerInventory : MonoBehaviour {
        [SerializeField] private Transform inventory;
        [SerializeField] private Transform inventoryGrid;
        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private int initInventorySize;
        
        [SerializeField] private CameraScript cameraScript;
        [SerializeField] private MovementStateMachine movementStateMachine;

        private List<InventoryItem> storage = new();

        private bool isInventoryOpen;

        private void Start() {
            for (int i = 0; i < initInventorySize; i++) {
                CellUI cell = Instantiate(cellPrefab, inventoryGrid).GetComponent<CellUI>();
                cell.gameObject.name = "Cell" + i;
                storage.Add(cell.inventoryItem);
            }

            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Tab)) {
                isInventoryOpen = !isInventoryOpen;
                inventory.gameObject.SetActive(isInventoryOpen);
                cameraScript.enabled = !isInventoryOpen;
                movementStateMachine.SetEnabled(!isInventoryOpen);
                Cursor.lockState = isInventoryOpen ? CursorLockMode.None : CursorLockMode.Locked;
                Cursor.visible = isInventoryOpen;
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
            item.cellUI.Image.enabled = true;
        }
    }
}