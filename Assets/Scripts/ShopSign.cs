using TMPro;
using UnityEngine;

public class ShopSign : MonoBehaviour, IInteractable
{
    public SOShopItem item;
    private TextMeshPro tmpText;
    private Transform tmpTextTransform;
    public float textHeight = 2f;
    [SerializeField] private TextMeshPro textPrefab;
    void Start() {
        tmpText = Instantiate(textPrefab, transform.position + new Vector3(0,textHeight,0), Quaternion.identity).GetComponent<TextMeshPro>();
        tmpText.text = item.itemName + "\nCost: $" + item.price;
        tmpTextTransform = tmpText.transform;
    }
    void Update() {
        tmpTextTransform.LookAt(GameControl.instance.playerTransform);
    }
    public void Interact() {
        Debug.Log("Purchased: " + item.itemName);
    }
}
