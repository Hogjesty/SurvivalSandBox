using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Inventory {
    public class InventoriesManager : MonoBehaviour {
        [SerializeField] private Image draggedImage;
        [SerializeField] private TextMeshProUGUI draggedAmountText;
        [SerializeField] private InventoryUI playerInventory;
        [SerializeField] private InventoryUI chestInventory;
        [SerializeField] private InventoryUI hotbarInventory;
        
        [SerializeField] private PlayerStorage playerStorage;
        [SerializeField] private HotbarStorage hotbarStorage;
        
        [SerializeField] private CameraScript playerCamera;

        [HideInInspector] public GameObject lastHoveredCell;

        private InventoryUI currentActiveInventory;
        
        private bool isInventoryOpen;

        private void Start() {
            FillInventoryUI(hotbarStorage.GetStorage(), hotbarInventory, StorageType.Hotbar);
            draggedImage.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
        private void Update() {
            if (Input.GetKeyDown(KeyCode.Tab)) {
                isInventoryOpen = !isInventoryOpen;
                playerInventory.InventoryUIObject.SetActive(isInventoryOpen);
                if (!isInventoryOpen) {
                    draggedImage.gameObject.SetActive(false);
                    chestInventory.InventoryUIObject.SetActive(false);
                    currentActiveInventory = null;
                } else {
                    playerInventory.ClearUI();
                    FillInventoryUI(playerStorage.Storage, playerInventory, StorageType.Player);
                }
                playerCamera.isRotationFrozen = isInventoryOpen;
                Cursor.lockState = isInventoryOpen ? CursorLockMode.None : CursorLockMode.Locked;
                Cursor.visible = isInventoryOpen;
            }
            if (Input.GetKeyDown(KeyCode.Escape)) {
                isInventoryOpen = false;
                playerInventory.InventoryUIObject.SetActive(false);
                draggedImage.gameObject.SetActive(false);
                chestInventory.InventoryUIObject.SetActive(false);
                currentActiveInventory = null;
                playerCamera.isRotationFrozen = false;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            for (int i = 49, j = 0; i <= 57; i++, j++) { // key codes
                if (Input.GetKeyDown((KeyCode)i)) {
                    TryToMove(hotbarInventory.UIItems[j].cellUI);
                }
            }
        }

        public void EnableDraggedImageWithNewParams(Sprite sprite, int draggedAmount) {
            draggedImage.sprite = sprite;
            draggedAmountText.text = draggedAmount.ToString();
            draggedImage.gameObject.SetActive(true);
        }

        public void SetDraggedImagePosition(Vector2 vector2) {
            draggedImage.transform.position = vector2;
        }

        public void DisableDraggedImage() {
            draggedImage.gameObject.SetActive(false);
        }

        public void OnDragEnd(CellUI departmentCell, CellUI destinationCell) {
            if (departmentCell.gameObject == destinationCell.gameObject || departmentCell.inventoryItem.resourceSo is null) {
                return;
            }
            
            if (destinationCell.inventoryItem.resourceSo is null) {
                TransferItem(departmentCell, destinationCell);
                return;
            }

            if (destinationCell.inventoryItem.resourceSo.ResourceType == departmentCell.inventoryItem.resourceSo.ResourceType) {
                int newAmount = destinationCell.inventoryItem.amount + departmentCell.inventoryItem.amount;

                if (newAmount > destinationCell.inventoryItem.resourceSo.MaxStackSize) {
                    int departmentAmount = newAmount - destinationCell.inventoryItem.resourceSo.MaxStackSize;
                    newAmount = destinationCell.inventoryItem.resourceSo.MaxStackSize;
                    SetCell( departmentCell, departmentCell.inventoryItem.resourceSo, departmentAmount);
                } 
                else ResetCell(departmentCell);
                
                SetCell(destinationCell,destinationCell.inventoryItem.resourceSo, newAmount);
                return;
            }

            SwapItems(departmentCell, destinationCell);
        }

        public void OnRightClickDragEnd(CellUI departmentCell, CellUI destinationCell, int newAmount) {
            if (departmentCell.gameObject == destinationCell.gameObject 
                || departmentCell.inventoryItem.resourceSo is null 
                || departmentCell.inventoryItem.amount == 1) {
                return;
            }

            if (destinationCell.inventoryItem.resourceSo is not null &&
                destinationCell.inventoryItem.resourceSo.ResourceType != departmentCell.inventoryItem.resourceSo.ResourceType) {
                return;
            }
            
            TransferAmountOfItem(departmentCell, destinationCell, newAmount);
        }

        private void SwapItems(CellUI departmentCell, CellUI destinationCell) {
            ResourceSO tempResourceSo = destinationCell.inventoryItem.resourceSo;
            int tempAmount = destinationCell.inventoryItem.amount;
            SetCell(destinationCell, departmentCell.inventoryItem.resourceSo, departmentCell.inventoryItem.amount);
            SetCell(departmentCell, tempResourceSo, tempAmount);
        }
        
        private void TransferItem(CellUI departmentCell, CellUI destinationCell) {
            SetCell(destinationCell, departmentCell.inventoryItem.resourceSo, departmentCell.inventoryItem.amount);
            ResetCell(departmentCell);
        }

        private void TransferAmountOfItem(CellUI departmentCell, CellUI destinationCell, int amount) {
            ResourceSO resourceSo = departmentCell.inventoryItem.resourceSo;
            departmentCell.inventoryItem.amount -= amount;
            amount += destinationCell.inventoryItem.amount;
            if (amount > resourceSo.MaxStackSize) {
                departmentCell.inventoryItem.amount += amount - resourceSo.MaxStackSize;
                amount = resourceSo.MaxStackSize;
            }
            SetCell(departmentCell, resourceSo, departmentCell.inventoryItem.amount);
            SetCell(destinationCell,resourceSo,amount);
            if (departmentCell.inventoryItem.amount == 0) {
                ResetCell(departmentCell);
            }
        }
        
        public void TryToAddItemIntoPlayerInventory(WorldItem newItem) {
            foreach (ItemData item in playerStorage.Storage) {
                if (item.resourceSo is null || item.resourceSo.MaxStackSize == 1) {
                    continue;
                }

                if (item.resourceSo.ResourceType == newItem.ResourceSo.ResourceType) {
                    if (item.amount < item.resourceSo.MaxStackSize) {
                        AddItem(item, newItem);
                        if (isInventoryOpen) {
                            FillInventoryUI(playerStorage.Storage, playerInventory, StorageType.Player);
                        }
                        return;
                    }
                }
            }

            foreach (ItemData item in playerStorage.Storage) {
                if (item.resourceSo is null) {
                    AddItem(item, newItem);
                    if (isInventoryOpen) {
                        FillInventoryUI(playerStorage.Storage, playerInventory, StorageType.Player);
                    }
                    return;
                }
            }

            print("Not enough space!");
        }

        public void OpenStorage(ItemData[] storage, StorageType storageType) {
            playerInventory.InventoryUIObject.SetActive(true);
            playerInventory.ClearUI();
            FillInventoryUI(playerStorage.Storage, playerInventory, StorageType.Player);
            isInventoryOpen = true;
            playerCamera.isRotationFrozen = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            if (storageType == StorageType.Chest) {
                chestInventory.InventoryUIObject.SetActive(true);
                currentActiveInventory = chestInventory;
                chestInventory.ClearUI();
                FillInventoryUI(storage, chestInventory, storageType);
            }
        }

        public void TransferItemToAnotherInventory(CellUI departmentCell) {
            if (currentActiveInventory is null) {
                return;
            }

            bool isPlayerInventory = !currentActiveInventory.UIItems.Contains(departmentCell.inventoryItem);
            TryToTransferItemToAnotherInventory(departmentCell, isPlayerInventory ? currentActiveInventory : playerInventory);
        }

        private void TryToTransferItemToAnotherInventory(CellUI departmentCell, InventoryUI destinationInventory) {
            foreach (InventoryItem destinationCell in destinationInventory.UIItems) {
                if (destinationCell.resourceSo is null || destinationCell.resourceSo.MaxStackSize == 1) {
                    continue;
                }

                if (destinationCell.resourceSo.ResourceType == departmentCell.inventoryItem.resourceSo.ResourceType) {
                    if (destinationCell.amount < destinationCell.resourceSo.MaxStackSize) {
                        int availableSpace = destinationCell.resourceSo.MaxStackSize - destinationCell.amount;
                        int transferAmount = Math.Min(availableSpace, departmentCell.inventoryItem.amount);
                        
                        SetCell(destinationCell.cellUI, destinationCell.resourceSo,destinationCell.amount + transferAmount);
                        SetCell(departmentCell, departmentCell.inventoryItem.resourceSo,departmentCell.inventoryItem.amount - transferAmount);
                        
                        if (departmentCell.inventoryItem.amount <= 0) {
                            ResetCell(departmentCell);
                        } else {
                            TryToTransferItemToAnotherInventory(departmentCell, destinationInventory);
                        }
                        destinationCell.cellUI.AnimateDrop();
                        return;
                    }
                }
            }

            foreach (InventoryItem item in destinationInventory.UIItems) {
                if (item.resourceSo is null) {
                    TransferItem(departmentCell, item.cellUI);
                    item.cellUI.AnimateDrop();
                    return;
                }
            }

            print("Not enough space!");
        }

        private void FillInventoryUI(ItemData[] storage, InventoryUI inventoryUI, StorageType storageType) {
            for (int i = 0; i < (int)storageType; i++) {
                ItemData itemData = storage[i];
                InventoryItem inventoryItem = inventoryUI.UIItems[i];
                inventoryItem.cellUI.storage = storage;
                inventoryItem.resourceSo = itemData.resourceSo;
                inventoryItem.amount = itemData.amount;
                inventoryItem.cellUI.Redraw();
            }
        }
        
        private void TryToMove(CellUI hotbarCell) {
            if (lastHoveredCell is null) {
                return;
            }
            CellUI departmentCell = lastHoveredCell.GetComponent<CellUI>();

            if (hotbarCell.inventoryItem.resourceSo is null) {
                TransferItem(departmentCell, hotbarCell);
                return;
            }
            
            SwapItems(departmentCell, hotbarCell);
        }

        private void AddItem(ItemData item, WorldItem newItem) {
            item.resourceSo = newItem.ResourceSo;
            item.amount += newItem.amount;

            if (item.amount > item.resourceSo.MaxStackSize) {
                newItem.amount = item.amount - item.resourceSo.MaxStackSize;
                item.amount = item.resourceSo.MaxStackSize;
                TryToAddItemIntoPlayerInventory(newItem);
            }
        }

        private void ResetCell(CellUI cellUI) {
            cellUI.inventoryItem.resourceSo = null;
            cellUI.inventoryItem.amount = 0;
            cellUI.Image.enabled = false;
            cellUI.CountText.text = null;
            cellUI.storage[cellUI.index].resourceSo = null;
            cellUI.storage[cellUI.index].amount = 0;
        }
        
        private void SetCell(CellUI cellUI, ResourceSO resourceSo, int itemAmount) {
            cellUI.inventoryItem.resourceSo = resourceSo;
            cellUI.inventoryItem.amount = itemAmount;
            cellUI.storage[cellUI.index].resourceSo = resourceSo;
            cellUI.storage[cellUI.index].amount = itemAmount;
            cellUI.Redraw();
        }
    }
}