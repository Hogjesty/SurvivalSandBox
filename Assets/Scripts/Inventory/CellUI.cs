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
    
    public readonly InventoryItem inventoryItem;
    [HideInInspector] public ItemData[] storage;
    [HideInInspector] public int index;

    private InventoriesManager inventoriesManager;
    private bool isDraggingRightButton;
    private int draggedAmount;

    public Image Image => image;
    public TextMeshProUGUI CountText => countText;

    public CellUI() {
        inventoryItem = new InventoryItem(this, null, 0);
    }

    private void Start() {
        inventoriesManager = FindObjectOfType<InventoriesManager>();
    }

    private void Update() {
        if (isDraggingRightButton) {
            CalculateNewDraggedAmount();
            inventoriesManager.EnableDraggedImageWithNewParams(image.sprite, draggedAmount);
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        inventoriesManager.lastHoveredCell = eventData.pointerEnter;
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(transform.DOScale(1.04f, 0.05f));
        mySequence.Append(transform.DOScale(1, 0.15f));
        hoverImage.enabled = true;
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Mouse0) && inventoryItem.resourceSo is not null) {
            inventoriesManager.TransferItemToAnotherInventory(this);
        }
    }
    
    public void OnPointerExit(PointerEventData eventData) {
        inventoriesManager.lastHoveredCell = null;
        hoverImage.enabled = false;
    }
    
    public void OnBeginDrag(PointerEventData data) {
        if (inventoryItem.resourceSo is null) return;

        if (Input.GetKey(KeyCode.LeftShift)) {
            inventoriesManager.TransferItemToAnotherInventory(this);
            return;
        }
        
        draggedAmount = inventoryItem.amount;
        if (data.button == PointerEventData.InputButton.Right) {
            if (inventoryItem.amount == 1) return;
            isDraggingRightButton = true;
            draggedAmount = Mathf.CeilToInt(inventoryItem.amount / 2.0f);
        }

        spriteMask.enabled = true;
        inventoriesManager.EnableDraggedImageWithNewParams(image.sprite, draggedAmount);
    }

    public void OnDrag(PointerEventData data) {
        inventoriesManager.SetDraggedImagePosition(data.position);
    }
    
    public void OnEndDrag(PointerEventData data) {
        if (inventoryItem.resourceSo is null) return;
        inventoriesManager.DisableDraggedImage();
        spriteMask.enabled = false;
        isDraggingRightButton = false;
        if (inventoriesManager.lastHoveredCell is not null) {
            CellUI destinationCell = inventoriesManager.lastHoveredCell.GetComponent<CellUI>();//todo перенести ластХоверед під капот методів???
            if (data.button == PointerEventData.InputButton.Right) {
                inventoriesManager.OnRightClickDragEnd(this, destinationCell, draggedAmount);
            } else {
                inventoriesManager.OnDragEnd(this, destinationCell);
            }
            destinationCell.AnimateDrop();
        }
    }
    
    public void OnPointerClick(PointerEventData eventData) {
        if (Input.GetKey(KeyCode.LeftShift) && inventoryItem.resourceSo is not null) {
            inventoriesManager.TransferItemToAnotherInventory(this);
        }
    }

    public void Redraw() {
        image.sprite = inventoryItem.resourceSo ? inventoryItem.resourceSo.Icon : null;
        image.enabled = inventoryItem.resourceSo;
        countText.text = inventoryItem.resourceSo ? inventoryItem.amount.ToString() : null;
    }

    public void AnimateDrop() {
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(transform.DOScale(0.96f, 0.05f));
        mySequence.Append(transform.DOScale(1, 0.15f));
    }

    private void CalculateNewDraggedAmount() {
        float mouseWheelInput = Input.GetAxis("Mouse ScrollWheel");
        if (mouseWheelInput == 0) return;

        int newDraggedAmount = draggedAmount;
        if (Input.GetKey(KeyCode.LeftShift)) {
            int percent = Mathf.CeilToInt(inventoryItem.amount / 100.0f);
            newDraggedAmount = mouseWheelInput > 0 ? newDraggedAmount + percent : newDraggedAmount - percent;
        } else if (Input.GetKey(KeyCode.LeftControl)) {
            int percent = Mathf.CeilToInt(inventoryItem.amount / 1000.0f);
            newDraggedAmount = mouseWheelInput > 0 ? newDraggedAmount + percent : newDraggedAmount - percent;
        } else {
            newDraggedAmount = mouseWheelInput > 0 ? newDraggedAmount + 1 : newDraggedAmount - 1;
        }

        if (newDraggedAmount > inventoryItem.amount) {
            newDraggedAmount = inventoryItem.amount;
        }

        if (newDraggedAmount < 1) {
            newDraggedAmount = 1;
        }

        draggedAmount = newDraggedAmount;
    }
}