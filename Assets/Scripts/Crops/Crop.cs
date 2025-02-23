using System.Collections.Generic;
using Unity.Entities.UniversalDelegates;
using UnityEngine;

public class Crop : AbstractPlaceable, IInteractable
{
    public int sellValue;
    [SerializeField] private float baseTimeToMaturity = 10f;
    private float timeToMaturity => baseTimeToMaturity / growthModifier;
    [SerializeField] private float timeToWater = 5f;

    [SerializeField] private List<GameObject> cropLevels;

    private float timer = 0f;

    private float growthModifier = 1f;
    public void SetGrowthModifier(float newGrowthModifier) {
        if (newGrowthModifier >= growthModifier) {
            growthModifier = newGrowthModifier;
        }
    }
    private float timeBetweenLevels => timeToMaturity / (cropLevels.Count - 1);

    private int levelIndex = 0;

    private bool isMature = false;
    private float waterTimer;

    public bool isWatered;
    [SerializeField] GameObject waterIcon;
    [SerializeField] WorldSpaceHudIcon waterIconScript;
    GameObject harvestIcon;
    
    private void OnEnable()
    {
        // These have to be present in OnEnable() and not Start() otherwise the models are not visible
        SetLevel(0);
    }

    private void OnDisable()
    {
        
    }

    void Start() {
        // DO NOT put these in OnEnable(). It breaks everything.
        waterIcon = Instantiate(SOManager.instance.hudIcons.waterIcon, GameControl.instance.ui.transform);
        waterIconScript = waterIcon.GetComponent<WorldSpaceHudIcon>();
        waterIconScript.IconInit(transform);
        InformTowers();
    }

    private const float informRadius = 15f;
    private void InformTowers() {
        Collider[] towers = Physics.OverlapSphere(transform.position, informRadius, 1 << 11, QueryTriggerInteraction.Collide);

        foreach (Collider towerCollider in towers) {
            IAOEBuff tower;
            if (towerCollider.TryGetComponent(out tower)) {
                tower.Inform(this);
            }

        }
    }

    private bool needsWater = true;
    public void DisableNeedingWater() {
        needsWater = false;
        isWatered = true;
        if (waterIcon != null) {
            Destroy(waterIcon);
        }
    }

    private void Update()
    {
        if (isMature) return;

        if (needsWater) {
            if (!isWatered) {
                return;
            }
            waterTimer -= Time.deltaTime;
            if (waterTimer <= 0) {
                waterIconScript.EnableIcon();
                isWatered = false;
            }
        }


        timer += Time.deltaTime;
        if (timer < timeBetweenLevels) return;
        timer -= timeBetweenLevels;
        SetLevel(levelIndex + 1);
        if (levelIndex == cropLevels.Count - 1) isMature = true;

        if (isMature) {
            if (waterIcon) {
                Destroy(waterIcon);
                waterIcon = null;
                waterIconScript = null;
            }
            harvestIcon = Instantiate(SOManager.instance.hudIcons.harvestIcon, GameControl.instance.ui.transform);
            harvestIcon.GetComponent<WorldSpaceHudIcon>().IconInit(transform);
        }
    }

    public void WaterThisPlant()
    {
        isWatered = true;
        waterTimer = timeToWater;
        if (waterIconScript) waterIconScript.DisableIcon();
    }

    private void SetLevel(int index)
    {
        levelIndex = index;
        for (var i = 0; i < cropLevels.Count; i++)
        {
            cropLevels[i].SetActive(i == index);
        }
    }

    public void Interact()
    {
        if (isMature) {
            if (sellValue == 800) {
                GameControl.instance.UpdateHealth(5);
            }
            GameControl.instance.UpdateMoney(sellValue);
            ReleaseSnappable();
            Debug.Log("Sold! Money = " + GameControl.instance.playerMoney);
            Destroy(harvestIcon);
            Destroy(gameObject);
        }
    }
}
