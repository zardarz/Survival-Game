using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private TargetJoint2D targetJoint;

    [SerializeField] private Transform target;

    void Start() {
        targetJoint = gameObject.GetComponent<TargetJoint2D>();
    }

    void FixedUpdate() {
        targetJoint.target = (Vector2) target.position;
    }
}