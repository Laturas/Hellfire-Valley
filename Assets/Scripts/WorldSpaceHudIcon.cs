using UnityEngine;
using UnityEngine.UI;

public enum HudIconType {
    Water,
    Harvest,
    Heal,
}

public class WorldSpaceHudIcon : MonoBehaviour
{
    private Transform worldPosLock;
    private Image thisImage;
    public void IconInit(Transform transformToLockTo) {
        worldPosLock = transformToLockTo;
    }

    void Start()
    {
        thisImage = GetComponent<Image>();
    }

    // DOES NOT DELETE THE GAMEOBJECT. Don't cause a memory leak.
    public void DisableIcon() {
        thisImage.enabled = false;
    }

    public void EnableIcon() {
        thisImage.enabled = true;
    }


    void Update()
    {
        Vector3 pos = GameControl.instance.playerCamera.WorldToScreenPoint(worldPosLock.position);
        transform.position = pos;
    }
}
