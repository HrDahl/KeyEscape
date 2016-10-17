using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AIns.FSM
{
	public class AI : CoroutineMachine
	{
		Quaternion initialRotation;
		Vector3 startPos;
		Vector3 lastSeenPosition;
		public List<Transform> waypoint1 = new List<Transform> ();
		Transform currentTarget;
		int counter = 0;
		int counter2 = 0;

		NavMeshAgent nav;
		GameObject player;
		WaitForSeconds m_FramerateWait = new WaitForSeconds (1.0f / 60.0f);

		public bool playerSpotted = false;
		public bool chasing = false;
		public bool lookingAround = false;


		// Use this for initialization
		void OnEnable ()
		{
			if (waypoint1.Count == 0)
				startPos = transform.position;
			nav = GetComponent<NavMeshAgent> ();
			player = GameObject.FindGameObjectWithTag ("Player");
			initialRotation = transform.rotation;
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
			if (lookingAround) {
				yield return new TransitionTo (LookingAroundState, DefaultTransition);
			}

			if (playerSpotted) {
				chasing = true; 
				yield return new TransitionTo (ChaseState, DefaultTransition);
			} else {
				if (chasing) {
					yield return new TransitionTo (LostVisionState, DefaultTransition);	
				} 
				yield return new TransitionTo (PatrolState, DefaultTransition);
			}
				


			yield return new TransitionTo (PatrolState, DefaultTransition);
		}

		IEnumerator PatrolState ()
		{
			if (waypoint1.Count == 0) {
				nav.SetDestination (startPos);
				//Debug.Log ("MISSING: Waypoints");
				if (nav.remainingDistance > nav.stoppingDistance) {
					nav.SetDestination (startPos);
				} else {
					ResetRotation ();
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
				lookingAround = true;
				yield return new TransitionTo (LookingAroundState, DefaultTransition);
			} 

			yield return new TransitionTo (StartState, DefaultTransition);
		}

		// Our default transition: stuff we do while changing state.
		IEnumerator DefaultTransition (StateRoutine from, StateRoutine to)
		{
			//Debug.Log (string.Format ("Transitioning from {0} to {1}", from.Method.Name, to == null ? "null" : to.Method.Name));
			if (from == LookingAroundState) { // We can also compare, to see if we come from a specific source coroutine
				yield return new WaitForSeconds (2);
			}
			yield return m_FramerateWait;
		}


		IEnumerator LookingAroundState ()
		{
			nav.SetDestination (transform.position + Vector3.right);
			yield return new WaitForSeconds (2);

			nav.SetDestination (transform.position - Vector3.right);
			yield return new WaitForSeconds (2);
			lookingAround = false;

			yield return new TransitionTo (StartState, DefaultTransition);

		}

		private void PlayerSpotted (PlayerSpottedEvent e)
		{
			playerSpotted = true;
		}

		private void ResetRotation ()
		{
			transform.rotation = Quaternion.Slerp (transform.rotation, initialRotation, Time.deltaTime * 2f);
		}
	}


}
