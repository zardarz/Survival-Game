using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private TargetJoint2D targetJoint;

    [SerializeField] private Transform target;

    [SerializeField] private float minZoom;
    [SerializeField] private float maxZoom;

    void Start() {
        targetJoint = gameObject.GetComponent<TargetJoint2D>();
    }

    void FixedUpdate() {
        targetJoint.target = (Vector2) target.position;
    }

    void Update() {
        if(Input.GetKey(KeyCode.Equals)) {gameObject.GetComponent<Camera>().orthographicSize += 0.02f;}
        if(Input.GetKey(KeyCode.Minus)) {gameObject.GetComponent<Camera>().orthographicSize -= 0.02f;}

        gameObject.GetComponent<Camera>().orthographicSize = Math.Min(Math.Max(gameObject.GetComponent<Camera>().orthographicSize, minZoom), maxZoom);
    }
}