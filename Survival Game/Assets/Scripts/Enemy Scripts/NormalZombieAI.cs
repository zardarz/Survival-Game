using UnityEngine;

public class NormalZombieAI : MonoBehaviour
{

    [SerializeField] private float minDisToChasePlayer;
    private Transform player;

    [SerializeField] private float damage;
    private Animator animator;

    private float disToPlayer;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        disToPlayer = Vector2.Distance(transform.position, player.position);

        if(disToPlayer <= minDisToChasePlayer) {
            animator.SetTrigger("StartChasingPlayer");
        } else {
            animator.SetTrigger("StopChasingPlayer");
        }

        if(disToPlayer <= 1f) {
            animator.SetTrigger("AttackPlayer");
        }
    }

    public void LookAtPlayer()
	{

		if (transform.position.x > player.position.x){
			spriteRenderer.flipX = true;
		} else if (transform.position.x < player.position.x){
            spriteRenderer.flipX = false;
		}
	}

    public float GetDamage() {
        return damage;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, minDisToChasePlayer);
    }
}