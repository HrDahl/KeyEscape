using UnityEngine;
using System.Collections;
using AIns.FSM;

public class PrototypeAI : CoroutineMachine {

	NavMeshAgent agent;
	public GameObject leader;
	public int index;

	bool enemySpotted = false;
	Vector3 enemyPosition;

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnEnable() {
		EventManager.Instance.StartListening<EnemySpottedEvent> (EnemySpotted);
	}

	void OnDestroy() {
		EventManager.Instance.StopListening<EnemySpottedEvent> (EnemySpotted);
	}

	protected override StateRoutine InitialState {

		get {
			return StartState;
		}
	}

	IEnumerator StartState ()
	{
		if (enemySpotted) {
			yield return new TransitionTo (ChaseState, DefaultTransition);
		}
			Debug.Log ("start state");
		yield return new TransitionTo (FollowState, DefaultTransition);
	}

	IEnumerator ChaseState ()
	{
			Debug.Log ("Chase state");
		agent.SetDestination (enemyPosition);

		yield return new TransitionTo (StartState, DefaultTransition);
	}

	IEnumerator FollowState () {
			Debug.Log ("Follow state");
		if (index == 0) {
			agent.SetDestination (leader.transform.position + Vector3.left+ Vector3.left);
		} else if (index == 1) {
			agent.SetDestination (leader.transform.position + Vector3.right + Vector3.right);
		} else if (index == 2) {
			agent.SetDestination (leader.transform.position + Vector3.forward+ Vector3.forward);
		} else if (index == 3) {
			agent.SetDestination (leader.transform.position + Vector3.back+ Vector3.back);
		}
		yield return new TransitionTo (StartState, DefaultTransition);
	}

	IEnumerator DefaultTransition (StateRoutine from, StateRoutine to)
	{
		Debug.Log (to);
		yield return new WaitForSeconds(0.1f);
	}

	void EnemySpotted(EnemySpottedEvent e)
	{
		enemyPosition = e.pos;
		enemySpotted = true;
	}
}

