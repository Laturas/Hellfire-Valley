using System;
using TMPro;
using UnityEngine;

public class ShopSign : MonoBehaviour, IInteractable
{
    public SOPlaceable item;
    private TextMeshPro tmpText;
    private Transform tmpTextTransform;
    public float textHeight = 2f;
    [SerializeField] private TextMeshPro textPrefab;

    private HotbarManager hotbarManager;
    
    private void OnEnable()
    {
        hotbarManager = FindFirstObjectByType<HotbarManager>();
        if (!hotbarManager)
        {
            Debug.LogError("Please place the hotbar prefab into the scene");
        }
    }

    void Start() {
        tmpText = Instantiate(textPrefab, transform.position + new Vector3(0,textHeight,0), Quaternion.identity).GetComponent<TextMeshPro>();
        tmpText.text = item.itemName + "\nCost: $" + item.price;
        tmpTextTransform = tmpText.transform;
    }
    void Update() {
        tmpTextTransform.LookAt(GameControl.instance.playerTransform);
    }
    public void Interact()
    {
        // Check if we have enough money, if we do, buy the item, otherwise, send a message/event or something
        if (GameControl.instance.UpdateMoney(-item.price))
        {
            bool itemAdded = hotbarManager.AddItem(item);
            if (!itemAdded) GameControl.instance.UpdateMoney(item.price);
        }
        else
        {
            Debug.Log("Not enough money :(");
        }
    }
}
