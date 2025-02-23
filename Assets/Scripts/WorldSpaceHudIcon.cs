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
    public bool outOfView;
    private bool isEnabled;
    public void IconInit(Transform transformToLockTo) {
        isEnabled = true;
        worldPosLock = transformToLockTo;
    }

    void Start()
    {
        thisImage = GetComponent<Image>();
    }

    // DOES NOT DELETE THE GAMEOBJECT. Don't cause a memory leak.
    public void DisableIcon() {
        isEnabled = false;
        thisImage.enabled = false;
    }

    public void EnableIcon() {
        isEnabled = true;
        if (!outOfView) {
            thisImage.enabled = true;
        }
    }


    void Update()
    {
        Camera playCam = GameControl.instance.playerCamera;
        Vector3 pos = playCam.WorldToScreenPoint(worldPosLock.position);

        // This is necessary because otherwise if you face the opposite direction, the icons will still be visible.
        outOfView = Vector3.Angle(worldPosLock.position - playCam.transform.position, playCam.transform.forward) > 120f;
        if (outOfView) {
            thisImage.enabled = false;
        } else {
            if (isEnabled) {
                thisImage.enabled = true;
            }
        }
        transform.position = pos;
    }
}
