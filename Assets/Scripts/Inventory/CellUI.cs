using System;
using Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class CellUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler {
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private Image image;
    public InventoryItem inventoryItem;

    private DragManager dragManager;

    public Image Image => image;
    public TextMeshProUGUI CountText => countText;

    private void Start() {
        dragManager = FindObjectOfType<DragManager>();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        dragManager.lastHoveredCell = eventData.pointerEnter;
    }
    
    public void OnPointerExit(PointerEventData eventData) {
        dragManager.lastHoveredCell = null;
    }
    
    public void OnBeginDrag(PointerEventData data) {
        if (inventoryItem.resourceSo is null) return;
        dragManager.EnableWithNewImage(image.sprite);
    }

    public void OnDrag(PointerEventData data) {
        dragManager.SetPosition(data.position);
    }
    
    public void OnEndDrag(PointerEventData data) {
        dragManager.Disable();
        if (dragManager.lastHoveredCell is not null) {
            dragManager.OnDragEnd(this, dragManager.lastHoveredCell.GetComponent<CellUI>());
        }
    }
}