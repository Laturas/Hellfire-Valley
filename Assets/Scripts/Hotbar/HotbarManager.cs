using System;
using System.Collections.Generic;
using UnityEngine;

public class HotbarManager : MonoBehaviour
{
    [SerializeField] private GameObject hotbarSelectionBox;
    [SerializeField] private float scrollCooldown = 0.1f;
    private GameObject[] hotbarItems;
    private int hotbarSelectionIndex = 0;

    private bool ableToScroll = true;
    
    private void OnEnable()
    {
        hotbarItems = new GameObject[10];
    }

    private void Update()
    {
        if (ableToScroll && Input.mouseScrollDelta.y != 0)
        {
            Debug.Log($"Scrolled {Input.mouseScrollDelta.y * .1f}");
            hotbarSelectionIndex += ((hotbarSelectionIndex + 1) % hotbarItems.Length);
            ableToScroll = false;
            LeanTween.delayedCall(scrollCooldown, () => ableToScroll = true);
        }
    }
}

public class ShopManager : MonoBehaviour
{
    //[SerializeField] private List<GameObject> 
}