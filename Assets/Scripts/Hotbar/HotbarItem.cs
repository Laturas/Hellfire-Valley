using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HotbarItem : MonoBehaviour
{
    [SerializeField] private Image hotbarIconImage;
    [SerializeField] private TextMeshProUGUI hotbarItemCount;
    private SOShopItem shopItem;
    private int numItems;

    private void OnEnable()
    {
        shopItem = null;
        numItems = 0;
        UpdateDisplay();
    }

    public void IncreaseCount()
    {
        numItems++;
        UpdateDisplay();
    }

    /// <summary>
    /// Decreases the number of this item in the hotbar
    /// </summary>
    /// <returns>False if there are still items left in this hotbar slot, True if there are none</returns>
    public void DecreaseCount()
    {
        numItems--;
        if (numItems == 0)
        {
            shopItem = null;
        }
        UpdateDisplay();
    }

    public int GetNumItems()
    {
        return numItems;
    }

    public void SetShopItem(SOShopItem item)
    {
        shopItem = item;
    }

    public void ResetItemCount()
    {
        numItems = 0;
        UpdateDisplay();
    }

    public SOShopItem GetShopItem()
    {
        return shopItem;
    }

    public void UpdateDisplay()
    {
        hotbarIconImage.enabled = !(numItems == 0 || !shopItem);
        hotbarItemCount.enabled = !(numItems == 0 || !shopItem);

        if (numItems != 0 && shopItem)
        {
            hotbarIconImage.sprite = shopItem.hotbarItem;
            hotbarItemCount.text = numItems.ToString();   
        }
    }

    public bool IsOfType(ItemType type)
    {
        return shopItem && shopItem.type == type;
    }

    public bool IsEmpty()
    {
        return numItems == 0;
    }
}