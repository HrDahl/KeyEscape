using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AI.FSM
{
	public class AI : CoroutineMachine
	{
		Vector3 startPos;
		Vector3 lastSeenPosition;
		public List<Transform> waypoint1 = new List<Transform> ();
		Transform currentTarget;
		int counter = 0;

		NavMeshAgent nav;
		GameObject player;
		WaitForSeconds m_FramerateWait = new WaitForSeconds (1.0f / 100.0f);

		public bool playerSpotted = false;
		public bool chasing = false;


		// Use this for initialization
		void OnEnable ()
		{
			if (waypoint1.Count == 0)
				startPos = transform.position;
			nav = GetComponent<NavMeshAgent> ();
			player = GameObject.FindGameObjectWithTag ("Player");
			EventManager.Instance.StartListening<PlayerSpottedEvent> (PlayerSpotted);
		}

		void OnDestroy ()
		{
			EventManager.Instance.StopListening<PlayerSpottedEvent> (PlayerSpotted);
		}
	
		// Update is called once per frame
		void Update ()
		{
		
		}

		// We need to tell the state machine where to start
		// StateRoutine is a function pointer of type IEnumerator function()
		protected override StateRoutine InitialState {

			get {
				return StartState;
			}
		}

		IEnumerator StartState ()
		{
			//Debug.Log ("Start state.");

			if (playerSpotted) {
				chasing = true; 
				yield return new TransitionTo (ChaseState, DefaultTransition);
			} else {
				if (chasing) {
					yield return new TransitionTo (LostVisionState, DefaultTransition);	
				} 
				yield return new TransitionTo (PatrolState, DefaultTransition);
			}


			// When we want to transition, use TransitionTo.
			// This can be wrapped up with if statements to get different behaviors
			yield return new TransitionTo (PatrolState, DefaultTransition);
		}

		IEnumerator PatrolState ()
		{
			if (waypoint1.Count == 0) {
				//Debug.Log ("MISSING: Waypoints");
				if (nav.remainingDistance <= nav.stoppingDistance) {
				} else {
					nav.SetDestination (startPos);
				}
			} else {
				if (nav.remainingDistance <= nav.stoppingDistance) {
					if (counter == waypoint1.Count - 1) {

						counter = 0;

					} else {
				
						counter++;
				
					}
				}
				nav.SetDestination (waypoint1 [counter].position);
			}


			// Note also, a transition requires a start and an endpoint
			yield return new TransitionTo (StartState, DefaultTransition);
		}

		IEnumerator ChaseState ()
		{
			nav.SetDestination (player.transform.position);
			lastSeenPosition = player.transform.position;
			// Note also, a transition requires a start and an endpoint
			chasing = true;
			if (!GetComponentInChildren<AIvision> ().inSight) {
				playerSpotted = false;
				yield return new TransitionTo (LostVisionState, DefaultTransition);

			} else {
				
				yield return new TransitionTo (StartState, DefaultTransition);

			}
		}

		IEnumerator LostVisionState ()
		{
			nav.SetDestination (lastSeenPosition);

			if (nav.remainingDistance <= nav.stoppingDistance) {
				chasing = false;

				yield return new TransitionTo (StartState, DefaultTransition);
			} else {
				yield return new TransitionTo (LostVisionState, DefaultTransition);
			
			}
		}

		// Our default transition: stuff we do while changing state.
		IEnumerator DefaultTransition (StateRoutine from, StateRoutine to)
		{
			Debug.Log (string.Format ("Transitioning from {0} to {1}", from.Method.Name, to == null ? "null" : to.Method.Name));
			if (from == PatrolState) { // We can also compare, to see if we come from a specific source coroutine
			
			}
			yield return m_FramerateWait;
		}

		private void PlayerSpotted (PlayerSpottedEvent e)
		{
			playerSpotted = true;
		}
	}
}
