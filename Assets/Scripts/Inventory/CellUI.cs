using Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class CellUI : MonoBehaviour, IPointerClickHandler {
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private Image image;
    public InventoryItem inventoryItem;

    public Image Image => image;
    public TextMeshProUGUI CountText => countText;
    
    public void OnPointerClick(PointerEventData pointerEventData) {
        image.sprite = null;
        countText.text = "";
        inventoryItem.resourceSo = null;
    }
}
