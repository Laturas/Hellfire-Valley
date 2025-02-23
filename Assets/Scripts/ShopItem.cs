using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    [SerializeField] private Image shopItemImage;
    [SerializeField] private TextMeshProUGUI shopItemName;
    [SerializeField] private Button shopItemButton;
    private SOPlaceable shopItem;
    
    private HotbarManager hotbarManager;
    
    private void OnEnable()
    {
        hotbarManager = FindFirstObjectByType<HotbarManager>();
        if (!hotbarManager)
        {
            Debug.LogError("Please place the hotbar prefab into the scene");
        }
    }

    public void SetShopItem(SOPlaceable item)
    {
        shopItem = item;
        UpdateButtonAction();
        UpdateDisplay();
    }

    public void UpdateButtonAction()
    {
        shopItemButton.onClick.RemoveAllListeners();
        shopItemButton.onClick.AddListener(() =>
        {
            // Check if we have enough money, if we do, buy the item, otherwise, send a message/event or something
            if (GameControl.instance.UpdateMoney(-shopItem.price))
            {
                bool itemAdded = hotbarManager.AddItem(shopItem);
                if (!itemAdded) GameControl.instance.UpdateMoney(shopItem.price);
            }
            else
            {
                Debug.Log("Not enough money :(");
            }
        });
    }

    public void UpdateDisplay()
    {
        shopItemImage.sprite = shopItem.hotbarIcon;
        shopItemImage.preserveAspect = true;
        shopItemName.text = $"{shopItem.name}<br>${shopItem.price}";
    }
}