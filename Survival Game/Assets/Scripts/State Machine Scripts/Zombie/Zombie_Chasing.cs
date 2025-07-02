using UnityEngine;

public class Zombie_Walking : StateMachineBehaviour
{
    public float movementSpeed;

    Transform player;

    Rigidbody2D rb;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        animator.speed = movementSpeed;

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector2 vectorToMove = (player.position - animator.transform.position).normalized;
        rb.MovePosition((Vector2) animator.transform.position + vectorToMove * movementSpeed * Time.fixedDeltaTime);

        animator.GetComponent<NormalZombieAI>().LookAtPlayer();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
