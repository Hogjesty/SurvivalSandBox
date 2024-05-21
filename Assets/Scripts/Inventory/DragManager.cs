using System;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory {
    public class DragManager : MonoBehaviour {
        [SerializeField] private GameObject draggedImage;
        private Image image;

        [HideInInspector] public GameObject lastHoveredCell;

        private void Start() {
            image = draggedImage.GetComponent<Image>();
            draggedImage.SetActive(false);
        }

        public void EnableWithNewImage(Sprite sprite) {
            image.sprite = sprite;
            draggedImage.SetActive(true);
        }

        public void SetPosition(Vector2 vector2) {
            draggedImage.transform.position = vector2;
        }

        public void Disable() {
            draggedImage.SetActive(false);
        }

        public void OnDragEnd(CellUI departmentCell, CellUI destinationCell) {
            if (departmentCell.gameObject == destinationCell.gameObject) {
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
                    departmentCell.inventoryItem.amount = departmentAmount;
                    departmentCell.CountText.text = departmentAmount.ToString();
                } else {
                    ResetCell(departmentCell);
                }
                
                destinationCell.inventoryItem.amount = newAmount;
                destinationCell.CountText.text = newAmount.ToString();
                return;
            }

            SwapItems(departmentCell, destinationCell);
        }

        private void SwapItems(CellUI departmentCell, CellUI destinationCell) {
            ResourceSO tempResourceSo = destinationCell.inventoryItem.resourceSo;
            int tempAmount = destinationCell.inventoryItem.amount;
            Sprite tempSprite = destinationCell.Image.sprite;
            string tempText = destinationCell.CountText.text;
            
            destinationCell.inventoryItem.resourceSo = departmentCell.inventoryItem.resourceSo;
            destinationCell.inventoryItem.amount = departmentCell.inventoryItem.amount;
            destinationCell.Image.sprite = departmentCell.Image.sprite;
            destinationCell.CountText.text = departmentCell.CountText.text;

            departmentCell.inventoryItem.resourceSo = tempResourceSo;
            departmentCell.inventoryItem.amount = tempAmount;
            departmentCell.Image.sprite = tempSprite;
            departmentCell.CountText.text = tempText;
        }

        private void TransferItem(CellUI departmentCell, CellUI destinationCell) {
            destinationCell.inventoryItem.resourceSo = departmentCell.inventoryItem.resourceSo;
            destinationCell.inventoryItem.amount = departmentCell.inventoryItem.amount;
            destinationCell.Image.sprite = departmentCell.Image.sprite;
            destinationCell.CountText.text = departmentCell.CountText.text;

            ResetCell(departmentCell);
        }

        private void ResetCell(CellUI cellUI) {
            cellUI.inventoryItem.resourceSo = null;
            cellUI.inventoryItem.amount = 0;
            cellUI.Image.sprite = null;
            cellUI.CountText.text = null;
        }
    }
}