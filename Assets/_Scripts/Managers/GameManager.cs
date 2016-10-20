using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public enum GameState
{
	Playing,
	Paused,
	Won
}

public class GameManager : MonoBehaviour
{
    public GameState _GameState;

	[HideInInspector] 
	public float currentTime = 0.0f;

	public bool isPaused = false;

	void OnEnable ()
	{
	}

	void OnDisable ()
	{
	}

	void Start ()
	{
        if (_instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
	}

	void Awake ()
	{
        Time.timeScale = 1;
		EventManager.Instance.TriggerEvent(new InstantiateGame());
	}

	void Update () {
        if (!isPaused) {
            UpdateTime();
        }
    }

	/// <summary>
	/// Restart Game
	/// Call this function when you want to restart the scene
	/// </summary>
	public void RestartGame ()
	{
		StopAllCoroutines();
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		//  EventManager.TriggerEvent (_eventsContainer.resetGame);
	}

	/// <summary>
	/// Toggle Pause
	/// Call this function when you want to pause
	/// </summary>
	public void TogglePause ()
	{

		isPaused = !isPaused;

		if (isPaused) {
			PauseGame ();
		} else {
			ResumeGame ();
		}
	}

   	private void PauseGame ()
	{
		_GameState = GameState.Paused;
		Time.timeScale = Mathf.Epsilon;
	}

	private void ResumeGame ()
	{
		_GameState = GameState.Playing;
		Time.timeScale = 1;
	}

	private void WinGame () 
	{
		_GameState = GameState.Won;
	}

	private void UpdateTime ()
	{
		currentTime += Time.deltaTime;
	}

	private static GameManager _instance;

    public static GameManager Instance {
		get { 
			if (_instance == null) {
				GameObject go = new GameObject ("GameManager");
				go.AddComponent<GameManager> ();
			} 
			return _instance;
		}
	}
}