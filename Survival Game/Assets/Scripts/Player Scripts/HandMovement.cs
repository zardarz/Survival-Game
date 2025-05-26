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
        rb.rotation = angle;

        if(angle > 90f || angle < -90) {transform.GetChild(0).GetComponent<SpriteRenderer>().flipY = true;} else {transform.GetChild(0).GetComponent<SpriteRenderer>().flipY = false;}
    }
}