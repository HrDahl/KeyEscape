using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public enum GameState
{
	Playing,
	Paused,
	Won
}

public class GameManager : MonoBehaviour
{
    public GameState _GameState;
    GameObject player;
    public GameObject camera;

	[HideInInspector] 
	public float currentTime = 0.0f;

	public bool isPaused = false;
	public float overallTimer = 180f;

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
        EventManager.Instance.TriggerEvent(new StartTimer(overallTimer));
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

        player = GameObject.FindGameObjectWithTag("Player");

        foreach (var key in player.GetComponent<PlayerController>().keysObtained) {
            GameObject k = (GameObject) Instantiate(key, key.transform.position, Quaternion.identity);
            k.SetActive(true);
        }

        player.GetComponent<PlayerController>().keysObtained = new List<GameObject>();
        player.transform.position = new Vector3(7.003318f, 0.537219f, -3.009342f);
        player.transform.rotation = Quaternion.Euler(0, 90, 0);
        player.GetComponentsInChildren<Transform>()[1].GetComponent<Renderer> ().material.color = Color.gray;

        camera.transform.position = new Vector3(5, 4f, -3f);

        GameObject menu = (GameObject)GameObject.FindGameObjectWithTag("MenuUI");

        int counter = 0;

        foreach (Transform child in menu.transform) {
            child.gameObject.SetActive(true);

            if (counter == 3) {
                child.gameObject.SetActive(false);
            }
            counter++;
        }

        EventManager.Instance.TriggerEvent(new StartTimer(overallTimer));
        EventManager.Instance.TriggerEvent(new RemoveUI(4));
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

	public void OpenGate() {
		EventManager.Instance.TriggerEvent (new OpenGate ());
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