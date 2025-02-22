using System;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour, IInteractable
{
    public int sellValue;
    [SerializeField] private float timeToMaturity = 10f;
    [SerializeField] private float timeToWater = 5f;

    [SerializeField] private List<GameObject> cropLevels;

    private float timer = 0f;

    private float timeBetweenLevels = 1f;

    private int levelIndex = 0;

    private bool isMature = false;
    private float waterTimer;

    public bool isWatered;
    
    private void OnEnable()
    {
        timeBetweenLevels = timeToMaturity / (cropLevels.Count - 1);
        SetLevel(0);
    }

    private void OnDisable()
    {
        
    }

    private void Update()
    {
        if (!isWatered) return;
        waterTimer -= Time.deltaTime;
        if (waterTimer <= 0) isWatered = false;
        if (isMature) return;
        timer += Time.deltaTime;
        if (!(timer >= timeBetweenLevels)) return;
        timer -= timeBetweenLevels;
        levelIndex++;
        SetLevel(levelIndex);
        if (levelIndex == cropLevels.Count - 1) isMature = true;
    }

    public void WaterThisPlant() {
        isWatered = true;
        waterTimer = timeToWater;
    }

    private void SetLevel(int index)
    {

        for (var i = 0; i < cropLevels.Count; i++)
        {
            cropLevels[i].SetActive(i == index);
        }
    }

    public void Interact()
    {
        if (isMature) {
            GameControl.instance.UpdateMoney(sellValue);
            Debug.Log("Sold! Money = " + GameControl.instance.playerMoney);
            Destroy(gameObject);
        }
    }
}
