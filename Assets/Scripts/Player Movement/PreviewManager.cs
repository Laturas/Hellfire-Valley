using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PreviewManager : MonoBehaviour
{
    [SerializeField] private List<SOPlaceable> placeableItemSOs;
    [SerializeField] private Material greenPreviewMaterial;
    [SerializeField] private Material redPreviewMaterial;
    private List<PreviewItem> generatedPreviewItems;
    
    private PreviewItem currentPreviewItem;
    
    private void Start()
    {
        generatedPreviewItems = new List<PreviewItem>();
        foreach (var generatedItem in placeableItemSOs.Select(GeneratePreviewItem))
        {
            generatedItem.DisablePreview();
            generatedPreviewItems.Add(generatedItem);
        }
    }

    public bool IsPreviewActive()
    {
        return currentPreviewItem != null && currentPreviewItem.IsEnabled();
    }

    public void PlacePreview(SOPlaceable item, Vector3 position, bool isGreen)
    {
        if (currentPreviewItem != null && item.type == currentPreviewItem.type)
        {
            // If the previewed item is the same type of item as last time, just place it
        }
        else
        {
            currentPreviewItem?.DisablePreview();
            currentPreviewItem = generatedPreviewItems.FirstOrDefault(it => it.type == item.type);
            if (currentPreviewItem == null)
            {
                Debug.LogError("Could not find preview item, make sure it is assigned in the PreviewManager");
                return;
            }
        }

        currentPreviewItem.EnablePreview(isGreen);
        currentPreviewItem.PlacePreviewAt(position);
    }

    // private void SetCurrentPreviewItem(SOShopItem item, bool greenPreview)
    // {
    //     type = item.type;
    //     isGreenPreview = greenPreview;
    //     currentPreviewItem = generatedPreviewItems.FirstOrDefault(it => it.type == type);
    //     if (currentPreviewItem == null)
    //     {
    //         Debug.LogError("Could not find preview item, make sure it is assigned in the PreviewManager");
    //         return;
    //     }
    //
    //     if (greenPreview) currentPreviewItem.greenPreviewObject.SetActive(true);
    //     else currentPreviewItem.redPreviewObject.SetActive(true);
    // }
    //
    // private void UpdatePreviewPosition(Vector3 position)
    // {
    //     
    // }

    public void DisablePreview()
    {
        currentPreviewItem?.DisablePreview();
    }

    private PreviewItem GeneratePreviewItem(SOPlaceable item)
    {
        
        return new PreviewItem(item.type,
            GeneratePreviewWithMaterial(item, greenPreviewMaterial),
            GeneratePreviewWithMaterial(item, redPreviewMaterial));
    }

    private GameObject GeneratePreviewWithMaterial(SOPlaceable item, Material previewMaterial)
    {
        var previewObject = Instantiate(item.itemPrefab);
        
        // Disable all scripts
        var scripts = previewObject.GetAllNestedScripts();
        foreach (var monoBehaviour in scripts)
        {
            monoBehaviour.enabled = false;
        }
        
        // Disable all colliders
        var colliders = previewObject.GetAllNestedComponentsOfType<Collider>();
        foreach (var col in colliders)
        {
            col.enabled = false;
        }
        
        // Change all materials to be the preview
        var meshRenderers = previewObject.GetAllNestedComponentsOfType<MeshRenderer>();
        foreach (var meshRenderer in meshRenderers)
        {
            meshRenderer.materials = new Material[] { previewMaterial };
        }

        return previewObject;
    }
}