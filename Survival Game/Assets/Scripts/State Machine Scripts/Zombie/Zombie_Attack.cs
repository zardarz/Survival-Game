using UnityEngine;

public class Zombie_Attack : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(animator.transform.position, 1);

        for(int i = 0; i < colliders.Length; i++) {
            if(colliders[i].CompareTag("Player")) {
                colliders[i].GetComponent<Health>().TakeDamage(animator.GetComponent<NormalZombieAI>().GetDamage());
            }
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}