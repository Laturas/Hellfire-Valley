using System;
using UnityEngine;

public class GenericBullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private int bulletDamage = 10;
    [SerializeField] private GameObject bloodSplatter;

    private Damageable target = null;
    public void SetTarget(Damageable t)
    {
        target = t;
    }

    private void Update()
    {
        var dir = target.transform.position - transform.position;
        transform.forward = dir;
        transform.position =
            Vector3.MoveTowards(transform.position, target.transform.position, bulletSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Damageable>().DealDamage(bulletDamage, Team.FriendlyTeam);
            Instantiate(bloodSplatter, transform.position, Quaternion.identity);
            DespawnBullet();
        }
    }

    private void DespawnBullet()
    {
        Destroy(gameObject);
    }
}
