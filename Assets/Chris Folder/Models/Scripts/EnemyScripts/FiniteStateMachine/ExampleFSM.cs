using UnityEngine;
using System.Collections;


namespace Examples.FSM
{
	public class ExampleFSM : CoroutineMachine
	{
		WaitForSeconds m_FramerateWait = new WaitForSeconds (1.0f / 20.0f);

        // We need to tell the state machine where to start
        // StateRoutine is a function pointer of type IEnumerator function()
		protected override StateRoutine InitialState
		{
			get
			{
				return StartState;
			}
		}


		int m_RunCount = 0;


		IEnumerator StartState ()
		{
			Debug.Log ("Start state.");

            // When we want to transition, use TransitionTo.
            // This can be wrapped up with if statements to get different behaviors
			yield return new TransitionTo (RunningState, DefaultTransition);
		}


		IEnumerator RunningState ()
		{
			while (++m_RunCount <= 3)
			{
				Debug.Log ("Running state.");
				yield return m_FramerateWait;
			}

            // Note also, a transition requires a start and an endpoint
			yield return new TransitionTo (EndState, DefaultTransition);
		}


		IEnumerator EndState ()
		{
			Debug.Log ("End state.");
			yield return new TransitionTo (null, DefaultTransition);
		}

        // Our default transition: stuff we do while changing state.
		IEnumerator DefaultTransition (StateRoutine from, StateRoutine to)
		{
			Debug.Log (string.Format ("Transitioning from {0} to {1}", from.Method.Name, to == null ? "null" : to.Method.Name));
			if (from == RunningState) // We can also compare, to see if we come from a specific source coroutine
			{
				m_RunCount = 0;
			}
			yield return m_FramerateWait;
		}
	}
}
