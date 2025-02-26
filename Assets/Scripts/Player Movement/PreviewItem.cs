using UnityEngine;

public class PreviewItem
{
    public ItemType type;
    public GameObject greenPreviewObject;
    public GameObject redPreviewObject;

    private bool isGreen = false;
    private bool enabled = false;
    
    public PreviewItem(ItemType type, GameObject greenPreviewObject, GameObject redPreviewObject)
    {
        this.type = type;
        this.greenPreviewObject = greenPreviewObject;
        this.redPreviewObject = redPreviewObject;
    }

    public void EnablePreview(bool greenPreview)
    {
        if (greenPreview == isGreen && enabled) return;
        enabled = true;
        isGreen = greenPreview;
        greenPreviewObject.SetActive(greenPreview);
        redPreviewObject.SetActive(!greenPreview);
    }

    public void DisablePreview()
    {
        enabled = false;
        greenPreviewObject.SetActive(false);
        redPreviewObject.SetActive(false);
    }

    public void PlacePreviewAt(Vector3 position, Vector3 rotation)
    {
        greenPreviewObject.transform.position = position;
        redPreviewObject.transform.position = position;
    }

    public bool IsEnabled()
    {
        return enabled;
    }
}