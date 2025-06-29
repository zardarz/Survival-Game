using UnityEngine;

public class Hand : MonoBehaviour
{
    Vector2 mousePos;
    public Rigidbody2D rb;

    void Update() {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = transform.parent.position;
    }

    void FixedUpdate() {
        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        if(angle >= 90f || angle <= -90f) {
            transform.localScale = new(-1f,1f,1f);
        } else {
            transform.localScale = new(1f,1f,1f);
        }

        if(angle > 90f || angle < -90f) {
            angle += 180;
        }

        rb.rotation = angle;
    }
}