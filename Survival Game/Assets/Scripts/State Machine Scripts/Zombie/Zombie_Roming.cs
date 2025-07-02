using UnityEngine;

public class Zombie_Roming : StateMachineBehaviour
{
    public float movementSpeed;

    Rigidbody2D rb;

    private Vector2 movementVector;

    private float newDirIn;

    SpriteRenderer spriteRenderer;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb = animator.GetComponent<Rigidbody2D>();
        animator.speed = movementSpeed;

        spriteRenderer = animator.GetComponent<SpriteRenderer>();

        newDirIn = Random.Range(2,3);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        newDirIn -= Time.deltaTime;

        if(newDirIn <= 0) {
            movementVector = Random.insideUnitCircle.normalized;
            animator.SetTrigger("StartIdling");
        }

        if(movementVector.x < 0) {
            spriteRenderer.flipX = true;
        } else {
            spriteRenderer.flipX = false;
        }

        rb.MovePosition((Vector2) animator.transform.position + movementVector * movementSpeed * Time.fixedDeltaTime);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
