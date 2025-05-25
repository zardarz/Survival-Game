using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    [Header("Settings")]
    [SerializeField] private float speed;

    public Vector2 movement;

    void Start() {
        // get the rigidbody, sprite rednerer and animator at the start of the game
        rb = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();
    }

    void Update() {
        // get the x and y inputs
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if(movement.x == 1) {spriteRenderer.flipX = false;}
        if(movement.x == -1) {spriteRenderer.flipX = true;}

        flipPlayerBasedOnMousePos();

        if(movement.x != 0 || movement.y != 0) {animator.SetBool("IsMoving", true);}else {animator.SetBool("IsMoving", false);}
    }

    void FixedUpdate() {
        // normalize the vector to make the player move the same speed diagonaly 
        Vector2 vectorToMove = new Vector2(movement.x, movement.y).normalized * speed;

        // move the player based on the movemnt vector
        rb.MovePosition(rb.position + vectorToMove * Time.fixedDeltaTime);
    }

    void flipPlayerBasedOnMousePos() {
        if(!Input.GetMouseButton(0)) {return;} // make sure the player is holding the mouse down

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if(mousePos.x - transform.position.x > 0) {spriteRenderer.flipX = false;} else {spriteRenderer.flipX = true;}
    }
}