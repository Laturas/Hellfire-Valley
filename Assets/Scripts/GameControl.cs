using System;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    public static GameControl instance;
    public Transform playerTransform;
    public Camera playerCamera;
    public GameObject ui;
    public int playerHealth {get; private set;}
    public int playerMoney {get; private set;}

    public Action<SOShopItem> OnBuyItem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake() {
        instance = this;
        DontDestroyOnLoad(this);
    }

    public void UpdateMoney(int changeAmount) {
        playerMoney += changeAmount;
    }
    public void UpdateHealth(int changeAmount) {
        playerMoney += changeAmount;
    }
}
