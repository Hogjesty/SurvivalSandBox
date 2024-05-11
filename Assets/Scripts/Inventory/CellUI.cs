using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CellUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private Image image;

    public Image Image => image;
    public TextMeshProUGUI CountText => countText;
}
