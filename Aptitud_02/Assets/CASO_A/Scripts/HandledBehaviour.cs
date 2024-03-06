using UnityEngine;

public class HandledBehaviour : StateMachineBehaviour 
{
	public new string name;
	public bool on_trigger_enter = true;
	public bool on_trigger_exit = true;

	public interface IHandlerBehaviour
	{
		void OnEnterState(string name, Animator animator, AnimatorStateInfo stateInfo);
		void OnExitState(string name, Animator animator, AnimatorStateInfo stateInfo);
	}

	private IHandlerBehaviour handler;
	
	private IHandlerBehaviour GetHandler(Animator animator)
	{
		if (handler == null)
		{
			handler = animator.gameObject.GetComponent<IHandlerBehaviour>();

			if (handler == null)
			{
				handler = animator.gameObject.GetComponentInParent<IHandlerBehaviour>();
			}
		}
        
		return handler;
	}


	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
	{
		if (on_trigger_enter && GetHandler(animator) != null)
			GetHandler(animator).OnEnterState(name, animator, stateInfo);
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
	//
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
	{
		if (on_trigger_exit && GetHandler(animator) != null)
		{
			GetHandler(animator).OnExitState(name, animator, stateInfo);
		}
	}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
	//
	//}
}
