using UnityEngine;

public class NormalZombieAI : MonoBehaviour
{

    private Transform playerTransform;

    private Rigidbody2D rb;

    [SerializeField] private float movmentSpeed;

    void Start() {
        playerTransform = PlayerMovement.GetPlayerTransform();
        rb = gameObject.GetComponent<Rigidbody2D>();
        gameObject.GetComponent<Animator>().speed = movmentSpeed;
    }

    void Update()
    {
        Vector2 vectorToMove = new Vector2(playerTransform.position.x - rb.position.x, playerTransform.position.y - rb.position.y).normalized * movmentSpeed;

        rb.MovePosition(rb.position + vectorToMove * Time.fixedDeltaTime);
    }
}