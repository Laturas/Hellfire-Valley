using UnityEngine;

public class Turret : MonoBehaviour
{

    Collider[] colliders = new Collider[1];
    public Transform targetTransform;
    public Transform yPivot;
    public Transform xPivot;

    void FixedUpdate()
    {
        if (targetTransform == null || Vector3.Distance(targetTransform.position, transform.position) > SOManager.instance.turretTypes[0].range) {
            Retarget();
        }
        if (targetTransform != null) {
            Track();
            Fire();
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
        Quaternion slerpedRot = Quaternion.Slerp(yPivot.rotation, Quaternion.Euler(0, yRot, 0), SOManager.instance.turretTypes[0].rotateSpeed * Time.fixedDeltaTime);
        yPivot.rotation = slerpedRot;
    }

    private void xzTracking() {
        Vector3 flatten = targetTransform.position - xPivot.position;
        flatten = Vector3.ProjectOnPlane(flatten, xPivot.right).normalized;
        var changed = xPivot.forward;
        changed.y = 0;
        changed = changed.normalized;
        float xRot = Vector3.SignedAngle(changed, flatten, xPivot.right);
        Quaternion slerpedRot = Quaternion.Slerp(xPivot.localRotation, Quaternion.Euler(xRot, 0, 0), SOManager.instance.turretTypes[0].rotateSpeed * Time.fixedDeltaTime);
        xPivot.localRotation = slerpedRot;
    }

    private void Fire() {
        // Method stub
    }
    private void Retarget() {
        int count = Physics.OverlapSphereNonAlloc(transform.position, SOManager.instance.turretTypes[0].range, colliders, 1 << 7, QueryTriggerInteraction.Collide);
        if (count != 0) {
            targetTransform = colliders[0].transform;
        }
    }
}
