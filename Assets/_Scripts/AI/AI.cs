using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AIns.FSM
{
	[SerializeField]
	public class AI : CoroutineMachine
	{

		public TypeOfRobot type;

		new AudioSource audio;
		bool playSound = false;

        Vector3 alarmPosition = new Vector3(0f,0f,0f);
		public bool alarmed = false;

		Quaternion initialRotation;
		Vector3 startPos;
		Vector3 lastSeenPosition;
		public List<Transform> waypoint1 = new List<Transform> ();
		Transform currentTarget;
		int counter = 0;

		public float fireRate = 3f;
		float fireTime = 0f;

		NavMeshAgent nav;
		GameObject player;
		WaitForSeconds m_FramerateWait = new WaitForSeconds (1.0f / 60.0f);

		public bool playerSpotted = false;
		public bool chasing = false;
		public bool lookingAround = false;
		public float detectionRange = 30;

		public bool instaKill = true;

		bool shootCooldown = false;

		// Use this for initialization
		void OnEnable ()
		{
			audio = GetComponent<AudioSource> ();
			if (waypoint1.Count == 0)
				startPos = transform.position;
			nav = GetComponent<NavMeshAgent> ();

			initialRotation = transform.rotation;
			EventManager.Instance.StartListening<PlayerSpottedEvent> (PlayerSpotted);
			EventManager.Instance.StartListening<AlarmEvent> (Alarm);
			EventManager.Instance.StartListening<DeadEvent>(DeadPlayer);


		}

		void OnDestroy ()
		{
			EventManager.Instance.StopListening<PlayerSpottedEvent> (PlayerSpotted);
			EventManager.Instance.StopListening<AlarmEvent> (Alarm);
			EventManager.Instance.StopListening<DeadEvent>(DeadPlayer);
		}
	
		// Update is called once per frame
		void Update ()
		{
			if (shootCooldown == true) {
				if (fireTime > 0) {
					fireTime -= Time.deltaTime;
				} else {
					shootCooldown = false;
				}
			}
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
			if (player == null) {
				player = GameObject.FindGameObjectWithTag ("Player");
			}
			if (playerSpotted) {
				chasing = true; 
				if (!playSound) 
				{
					audio.Play ();
					playSound = true;
				}

				yield return new TransitionTo (ChaseState, DefaultTransition);
			} else {


				if (chasing) {
					yield return new TransitionTo (LostVisionState, DefaultTransition);	
				}
				if (lookingAround) {
					yield return new TransitionTo (LookingAroundState, DefaultTransition);
				}

				yield return new TransitionTo (PatrolState, DefaultTransition);
			}
				


			yield return new TransitionTo (PatrolState, DefaultTransition);
		}

		IEnumerator AttackState ()
		{
			if (nav.remainingDistance <= nav.stoppingDistance) 
			{
				Attack ();
			}
			yield return new TransitionTo (StartState, DefaultTransition);
		}

		IEnumerator PatrolState ()
		{
			nav.stoppingDistance = 0f;
			playSound = false;
			if (waypoint1.Count == 0) {
				nav.SetDestination (startPos);
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
			if (instaKill == true) {
				EventManager.Instance.TriggerEvent (new AlarmEvent (player.transform.position));
			}
			if (type == TypeOfRobot.Brawler) {
				nav.stoppingDistance = 2f;
				nav.speed = 3f;
			} else {
				nav.stoppingDistance = 5f;
				nav.speed = 2f;
			}

			nav.SetDestination (player.transform.position);
			lastSeenPosition = player.transform.position;

			if (nav.remainingDistance <= nav.stoppingDistance && playerSpotted) {
				yield return new TransitionTo (AttackState, DefaultTransition);
			}

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
			if (from == LookingAroundState) { // We can also compare, to see if we come from a specific source coroutine
				yield return new WaitForSeconds (2);
			}
			yield return m_FramerateWait;
		}


		IEnumerator LookingAroundState ()
		{
			if (playerSpotted) {
				lookingAround = false;
				yield return new TransitionTo (StartState, DefaultTransition);
			}
			nav.SetDestination (transform.position + Vector3.right);
			yield return new WaitForSeconds (2);
			if (playerSpotted) {
				lookingAround = false;
				yield return new TransitionTo (StartState, DefaultTransition);
			}
			nav.SetDestination (transform.position - Vector3.right);
			yield return new WaitForSeconds (2);
			lookingAround = false;

			yield return new TransitionTo (StartState, DefaultTransition);

		}

		IEnumerator AlarmedState ()
		{
			nav.SetDestination (alarmPosition);
			if (nav.remainingDistance <= nav.stoppingDistance) {
				alarmed = false;
				yield return new TransitionTo (LookingAroundState, DefaultTransition);
			}

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

		private void Alarm(AlarmEvent e) {
			if (Vector3.Distance(transform.position, e.AlarmAtPosition) < detectionRange) {
				playerSpotted = true;
			}
		}

		public void Attack () 
		{
			
			RaycastHit hit;
			Vector3 direction = player.transform.position - transform.position;
			if (Physics.Raycast (transform.position, direction.normalized, out hit, 100)) {
				if (hit.collider.gameObject.tag == "Player") {
					RotateTowards (hit.transform);
					if (shootCooldown == false) {
				
						shootCooldown = true;
						fireTime = fireRate;
						EventManager.Instance.TriggerEvent (new TakeDamageEvent (3f));
					}
				} 
			}
		}

		private void RotateTowards(Transform target)
		{
			Vector3 direction = (target.position - transform.position).normalized;
			Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));    // flattens the vector3
			transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2);
		}

		private void DeadPlayer(DeadEvent e) 
		{
			chasing = false;
			playerSpotted = false;
		}
	}

	public enum TypeOfRobot {

		Brawler, Shooter

	}

}
