using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Inventory {
    public class InventoryUI : MonoBehaviour {
        
        [SerializeField] private GameObject inventoryUIObject;
        [SerializeField] private Transform cellParent;
        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private StorageType storageType;

        private readonly List<InventoryItem> UIitems = new();

        private bool isInventoryOpen;
        
        public GameObject InventoryUIObject => inventoryUIObject;
        public List<InventoryItem> UIItems => UIitems;
        
        private void Awake() {
            for (int i = 0; i < (int) storageType; i++) {
                GameObject cellObject = Instantiate(cellPrefab, cellParent);
                CellUI cell = cellObject.GetComponent<CellUI>();
                cell.gameObject.name = "Cell" + i;
                cell.index = i;
                UIitems.Add(cell.inventoryItem);
                if (storageType == StorageType.Hotbar) {
                    cellObject.transform.GetChild(cellObject.transform.childCount - 1)
                        .GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();
                }
            }
        }

        public void ClearUI() {
            foreach (InventoryItem inventoryItem in UIitems) {
                inventoryItem.resourceSo = null;
                inventoryItem.amount = 0;
                inventoryItem.cellUI.Image.enabled = false;
                inventoryItem.cellUI.CountText.text = null;
            }
        }
    }
}