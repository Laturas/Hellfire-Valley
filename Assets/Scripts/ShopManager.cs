using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private Transform cropShopPanel;
    [SerializeField] private Transform buildingShopPanel;
    [SerializeField] private GameObject shopButtonPrefab;

    private ShopItem[] cropShopItems;
    private ShopItem[] buildingShopItems;

    private List<SOPlaceable> crops;
    private List<SOPlaceable> buildings;

    private List<RaycasterWorld> worldRaycasters;

    private void OnEnable()
    {
        PopulatePlaceableLists();

        worldRaycasters ??= GetComponentsInChildren<RaycasterWorld>().ToList();
        GameControl.instance.OnPaused += OnPaused;
        
        cropShopItems = new ShopItem[10];
        for (int i = 0; i < cropShopItems.Length; i++)
        {
            cropShopItems[i] = Instantiate(shopButtonPrefab, cropShopPanel).GetComponent<ShopItem>();
            if (i < crops.Count)
            {
                cropShopItems[i].SetShopItem(crops[i]);
            }
            else
            {
                cropShopItems[i].gameObject.SetActive(false);
            }
        }
        buildingShopItems = new ShopItem[10];
        for (int i = 0; i < buildingShopItems.Length; i++)
        {
            buildingShopItems[i] = Instantiate(shopButtonPrefab, buildingShopPanel).GetComponent<ShopItem>();
            if (i < buildings.Count)
            {
                buildingShopItems[i].SetShopItem(buildings[i]);
            }
            else
            {
                buildingShopItems[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnDisable()
    {
        GameControl.instance.OnPaused -= OnPaused;
    }

    private void OnPaused(bool paused)
    {
        foreach (var worldRaycaster in worldRaycasters)
        {
            worldRaycaster.enabled = !paused;
        }
    }

    private void PopulatePlaceableLists()
    {
        crops = new List<SOPlaceable>();
        buildings = new List<SOPlaceable>();

        var placeables = SOManager.instance.placeables;
        foreach (var placeable in placeables)
        {
            switch (placeable.shopLocation)
            {
                case ShopLocation.Crops:
                    crops.Add(placeable);
                    break;
                case ShopLocation.Buildings:
                    buildings.Add(placeable);
                    break;
            }
        }
    }
}