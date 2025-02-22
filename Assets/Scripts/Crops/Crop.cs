using System;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{
    [SerializeField] private float timeToMaturity = 10f;

    [SerializeField] private List<GameObject> cropLevels;

    private float timer = 0f;

    private float timeBetweenLevels = 1f;

    private int levelIndex = 0;

    private bool isMature = false;
    
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
        if (isMature) return;
        timer += Time.deltaTime;
        if (!(timer >= timeBetweenLevels)) return;
        timer -= timeBetweenLevels;
        levelIndex++;
        SetLevel(levelIndex);
        if (levelIndex == cropLevels.Count - 1) isMature = true;
    }

    private void SetLevel(int index)
    {

        for (var i = 0; i < cropLevels.Count; i++)
        {
            cropLevels[i].SetActive(i == index);
        }
    }
}
