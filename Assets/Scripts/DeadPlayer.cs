using System.Data.Common;
using UnityEngine;

public class DeadPlayer : MonoBehaviour
{
    [SerializeField] private float uiAppearDelay = 3f;
    private float delayTimer;
    [SerializeField] private GameObject deathScreen;
    private bool deathScreenSpawned = false;
    private Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 5f, ForceMode.VelocityChange);
        GameControl.instance.TriggerOnDeathUIUpdates();
        delayTimer = uiAppearDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (!deathScreenSpawned && delayTimer <= 0f) {
            deathScreen.SetActive(true);
            deathScreenSpawned = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }
        delayTimer -= Time.deltaTime;
    }
}
