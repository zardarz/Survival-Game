using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Settings")]
    [SerializeField] private float speed;

    public Vector2 movement;

    void Start() {
        // get the rigidbody at the start of the game
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update() {
        // get the x and y inputs
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate() {
        // move the player based on the movemnt vector
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }
}