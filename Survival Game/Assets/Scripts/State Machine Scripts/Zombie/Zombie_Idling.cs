using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Zombie_Idling : StateMachineBehaviour
{

    private float timeLeftForIdling;

    private float timeUntilTurnAround;

    private SpriteRenderer spriteRenderer;

    [SerializeField] private float minIdlingTime;
    [SerializeField] private float maxIdlingTime;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.speed = 1;

        timeLeftForIdling = Random.Range(minIdlingTime, maxIdlingTime);

        spriteRenderer = animator.GetComponent<SpriteRenderer>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeLeftForIdling -= Time.deltaTime;
        timeUntilTurnAround -= Time.deltaTime;

        if(timeUntilTurnAround <= 0) {
            spriteRenderer.flipX = !spriteRenderer.flipX;
            timeUntilTurnAround = Random.Range(minIdlingTime/2, maxIdlingTime/2);
        }

        if(timeLeftForIdling <= 0) {
            animator.SetTrigger("StartRoming");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
