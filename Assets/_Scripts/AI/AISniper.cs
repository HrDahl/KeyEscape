using UnityEngine;
using System.Collections;

namespace AIns.FSM
{
	[SerializeField]
	public class AISniper : CoroutineMachine
	{

	
		AudioSource audio;
		bool playSound = false;

		public float fireRate = 3f;
		float fireTime = 0f;

		GameObject player;
		WaitForSeconds m_FramerateWait = new WaitForSeconds (1.0f / 60.0f);

		public bool playerSpotted = false;
		public float detectionRange = 30;

		bool shootCooldown = false;

		// Use this for initialization
		void OnEnable ()
		{
			audio = GetComponent<AudioSource> ();
			EventManager.Instance.StartListening<AlarmEvent> (Alarm);
			EventManager.Instance.StartListening<DeadEvent> (DeadPlayer);


		}

		void OnDestroy ()
		{
			
			EventManager.Instance.StopListening<AlarmEvent> (Alarm);
			EventManager.Instance.StopListening<DeadEvent> (DeadPlayer);
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

			if (playerSpotted) {
				if (!playSound) {
					audio.Play ();
					playSound = true;
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
			//Debug.Log ("Start state.");
			if (player == null) {
				player = GameObject.FindGameObjectWithTag ("Player");
			}
			if (playerSpotted) {
				

				yield return new TransitionTo (AttackState, DefaultTransition);
			} else {

				yield return new TransitionTo (StartState, DefaultTransition);
			}



			yield return new TransitionTo (StartState, DefaultTransition);
		}

		IEnumerator AttackState ()
		{	
			Attack ();
			yield return new TransitionTo (StartState, DefaultTransition);
		}
			
		IEnumerator DefaultTransition (StateRoutine from, StateRoutine to)
		{

			yield return m_FramerateWait;
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
					if (shootCooldown == false) {

						shootCooldown = true;
						fireTime = fireRate;
						EventManager.Instance.TriggerEvent (new TakeDamageEvent (3f));
					}
				} 
			}
		}

		private void DeadPlayer(DeadEvent e) 
		{
			playerSpotted = false;
		}
	}



}
