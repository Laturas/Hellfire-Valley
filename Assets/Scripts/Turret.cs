using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private SOTurret turretType;
    
    Collider[] colliders = new Collider[1];
    public Transform yPivot;
    public Transform xPivot;
    [SerializeField] private Transform shootTransform;
    private Transform targetTransform;
    private Damageable targetDamageable;

    private bool ableToFire = true;

    void FixedUpdate()
    {
        if (!targetTransform || Vector3.Distance(targetTransform.position, transform.position) > turretType.range) {
            Retarget();
        }
        if (targetTransform) {
            Track();
            if (ableToFire) Fire();
        }
    }
    private void Track() {
        yTracking();
        xzTracking();
    }

    private void yTracking() {
        Vector3 flatten = targetTransform.position - yPivot.position;
        flatten.y = 0;
        float yRot = Vector3.SignedAngle(Vector3.forward, flatten, Vector3.up);
        Quaternion slerpedRot = Quaternion.Slerp(yPivot.rotation, Quaternion.Euler(0, yRot, 0), turretType.rotateSpeed * Time.fixedDeltaTime);
        yPivot.rotation = slerpedRot;
    }

    private void xzTracking() {
        Vector3 flatten = targetTransform.position - xPivot.position;
        flatten = Vector3.ProjectOnPlane(flatten, xPivot.right).normalized;
        var changed = xPivot.forward;
        changed.y = 0;
        changed = changed.normalized;
        float xRot = Vector3.SignedAngle(changed, flatten, xPivot.right);
        Quaternion slerpedRot = Quaternion.Slerp(xPivot.localRotation, Quaternion.Euler(xRot, 0, 0), turretType.rotateSpeed * Time.fixedDeltaTime);
        xPivot.localRotation = slerpedRot;
    }

    private void Fire() {
        // Method stub
        ableToFire = false;
        var bullet = Instantiate(turretType.bulletPrefab, shootTransform.position, Quaternion.identity);
        var bulletComponent = bullet.GetComponent<GenericBullet>();
        bulletComponent.SetTarget(targetDamageable);
        LeanTween.delayedCall(turretType.fireDelay, () =>
        {
            ableToFire = true;
        });
    }
    private void Retarget() {
        int count = Physics.OverlapSphereNonAlloc(transform.position, turretType.range, colliders, 1 << 7, QueryTriggerInteraction.Collide);
        if (count != 0) {
            targetTransform = colliders[0].transform;
            targetDamageable = colliders[0].GetComponent<Damageable>();
        }
    }
}
