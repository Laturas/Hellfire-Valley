using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class HotbarManager : MonoBehaviour
{
    [SerializeField] private GameObject hotbarSelectionBox;
    [SerializeField] private float scrollCooldown = 0.1f;
    [SerializeField] private GameObject hotbarItemPrefab;
    private HotbarItem[] hotbarItems;
    private int hotbarSelectionIndex = 0;
    
    private void OnEnable()
    {
        if (hotbarItems != null)
        {
            foreach (var hotbarItem in hotbarItems)
            {
                Destroy(hotbarItem.gameObject);
            }

            hotbarItems = null;
        }
        hotbarItems = new HotbarItem[10];
        for (int i = 0; i < 10; i++)
        {
            hotbarItems[i] = Instantiate(hotbarItemPrefab, transform).GetComponent<HotbarItem>();
        }

        MoveSelectionBox();
    }

    private void Update()
    {
        var scrollWheelValue = Input.GetAxis("Mouse ScrollWheel");
        if (scrollWheelValue != 0)
        {
            //Debug.Log($"Scrolled {Input.mouseScrollDelta.y * .1f}");
            hotbarSelectionIndex = (hotbarSelectionIndex + (scrollWheelValue < 0 ? 1 : -1) + hotbarItems.Length) % hotbarItems.Length;
            MoveSelectionBox();
        }
    }

    private void MoveSelectionBox()
    {
        hotbarSelectionBox.transform.SetParent(hotbarItems[hotbarSelectionIndex].transform);
        hotbarSelectionBox.transform.localPosition = Vector3.zero;
    }

    public SOPlaceable GetCurrentlySelectedItem()
    {
        return hotbarItems[hotbarSelectionIndex].GetShopItem();
    }

    /// <summary>
    /// Adds the item into an appropriate hotbar slot
    /// </summary>
    /// <param name="shopItem"></param>
    /// <returns>If the add was successful</returns>
    public bool AddItem(SOPlaceable shopItem)
    {
        var item = GetAppropriateHotbarItem(shopItem);
        if (item == null) return false;
        if (item.IsEmpty())
        {
            // Add item to new slot
            item.ResetItemCount();
            item.SetShopItem(shopItem);
            item.IncreaseCount();
        }
        else
        {
            item.IncreaseCount();
        }

        return true;
    }

    public bool RemoveItem(SOPlaceable shopItem)
    {
        var item = hotbarItems.FirstOrDefault(it => it.IsOfType(shopItem.type) && !it.IsEmpty());
        if (item) item.DecreaseCount();
        return item != null;
    }

    private HotbarItem GetAppropriateHotbarItem(SOPlaceable shopItem)
    {
        HotbarItem item = null;
        // First we see if the player already has an item of this type, if so, add it there
        item = hotbarItems.FirstOrDefault(it => it.IsOfType(shopItem.type));
        if (item != null) return item;
        // If we don't already have this item, check if the currently selected hotbar box is empty, if it is, add it there
        if (hotbarItems[hotbarSelectionIndex].IsEmpty()) return hotbarItems[hotbarSelectionIndex];
        // If the current slot is full, get the first free slot, if it exists, add it there
        item = hotbarItems.FirstOrDefault(it => it.IsEmpty());
        
        return item;
    }
}