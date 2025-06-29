using System;
using UnityEngine;

public class Sensor
{
    public float weight;

    public float angle;

    public RaycastHit2D hit;

    public Transform origin;

    public float awareness;

    public Vector2 vector2;

    public LayerMask mask;

    public void UpdateRayCast() {
        hit = Physics2D.Raycast(origin.position, vector2, awareness, mask);

        //if(weight < 0 ) return;

        Color colorOfRay = weight < 0 ? Color.red : Color.green;
        Vector3 end = origin.position + (Vector3)vector2 * Math.Abs(weight);

        Debug.DrawLine(origin.position, end, colorOfRay);
    }


    public void UpdateWeights() {
        weight = 0f;

        // if there is no collider, we dont care. we can move there if we want
        if(hit.collider == null) return;

        // but if there is a collider and it is the player, then we want to move there
        if(hit.collider.CompareTag("Player")) {
            weight += 0.5f;
        } else if (hit.distance <= 0.8f) {
            // if there is a collider but it is not the player. We dont wanna move there
            weight -= 1f;
        }

        weight = Mathf.Clamp(weight, -1f, 1f);

    }

    public bool Equals(Sensor sensor) {
        return sensor.angle == angle;
    }
}