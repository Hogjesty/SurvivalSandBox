using DG.Tweening;
using Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Image = UnityEngine.UI.Image;

public class CellUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler {
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private Image image;
    [SerializeField] private Image hoverImage;
    
    public InventoryItem inventoryItem;

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
    }
    
    public void OnPointerExit(PointerEventData eventData) {
        inventoriesManager.lastHoveredCell = null;
        hoverImage.enabled = false;
    }
    
    public void OnBeginDrag(PointerEventData data) {
        if (inventoryItem.resourceSo is null) return;
        inventoriesManager.EnableWithNewImage(image.sprite);
    }

    public void OnDrag(PointerEventData data) {
        inventoriesManager.SetPosition(data.position);
    }
    
    public void OnEndDrag(PointerEventData data) {
        inventoriesManager.Disable();
        if (inventoriesManager.lastHoveredCell is not null) {
            inventoriesManager.OnDragEnd(this, inventoriesManager.lastHoveredCell.GetComponent<CellUI>());
            AnimateDrop();
        }
    }

    private void AnimateDrop() {
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(inventoriesManager.lastHoveredCell.transform.DOScale(0.96f, 0.05f));
        mySequence.Append(inventoriesManager.lastHoveredCell.transform.DOScale(1, 0.15f));
    }
}