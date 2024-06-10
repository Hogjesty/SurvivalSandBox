using System;
using DG.Tweening;
using Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Image = UnityEngine.UI.Image;

public class CellUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler {
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private Image image;
    [SerializeField] private Image hoverImage;
    [SerializeField] private Image spriteMask;
    
    public InventoryItem inventoryItem;
    public ItemData[] storage;
    public int index;

    private InventoriesManager inventoriesManager;

    public Image Image => image;
    public TextMeshProUGUI CountText => countText;

    public CellUI() {
        inventoryItem = new InventoryItem(this, null, 0);
    }

    private void Start() {
        inventoriesManager = FindObjectOfType<InventoriesManager>();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        inventoriesManager.lastHoveredCell = eventData.pointerEnter;
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(transform.DOScale(1.04f, 0.05f));
        mySequence.Append(transform.DOScale(1, 0.15f));
        hoverImage.enabled = true;
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Mouse0)) {
            inventoriesManager.TransferItemToAnotherInventory(this);
        }
    }
    
    public void OnPointerExit(PointerEventData eventData) {
        inventoriesManager.lastHoveredCell = null;
        hoverImage.enabled = false;
    }
    
    public void OnBeginDrag(PointerEventData data) {
        if (inventoryItem.resourceSo is null || Input.GetKey(KeyCode.LeftShift)) return;
        int draggedAmount = inventoryItem.amount;
        if (data.button == PointerEventData.InputButton.Right) {
            if (draggedAmount == 1) return;
            draggedAmount = Mathf.CeilToInt(inventoryItem.amount / 2.0f);
        }

        spriteMask.enabled = true;
        inventoriesManager.EnableDraggedImageWithNewParams(image.sprite, draggedAmount);
    }

    public void OnDrag(PointerEventData data) {
        inventoriesManager.SetDraggedImagePosition(data.position);
    }
    
    public void OnEndDrag(PointerEventData data) {
        if (inventoryItem.resourceSo is null || Input.GetKey(KeyCode.LeftShift)) return;
        inventoriesManager.DisableDraggedImage();
        spriteMask.enabled = false;
        if (inventoriesManager.lastHoveredCell is not null) {
            CellUI destinationCell = inventoriesManager.lastHoveredCell.GetComponent<CellUI>();//todo перенести ластХоверед під капот методів???
            if (data.button == PointerEventData.InputButton.Right) {
                inventoriesManager.OnRightClickDragEnd(this, destinationCell);
            } else {
                inventoriesManager.OnDragEnd(this, destinationCell);
            }
            AnimateDrop();
        }
    }
    
    public void OnPointerClick(PointerEventData eventData) {
        if (Input.GetKey(KeyCode.LeftShift)) {
            inventoriesManager.TransferItemToAnotherInventory(this);
        }
    }

    public void Redraw() {
        image.sprite = inventoryItem.resourceSo ? inventoryItem.resourceSo.Icon : null;
        image.enabled = inventoryItem.resourceSo;
        countText.text = inventoryItem.resourceSo ? inventoryItem.amount.ToString() : null;
    }

    private void AnimateDrop() {
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(inventoriesManager.lastHoveredCell.transform.DOScale(0.96f, 0.05f));
        mySequence.Append(inventoriesManager.lastHoveredCell.transform.DOScale(1, 0.15f));
    }
}