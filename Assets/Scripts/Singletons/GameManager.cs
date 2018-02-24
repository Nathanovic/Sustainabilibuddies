using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	
	public static GameManager instance;
	public LevelInfoHandler infoHandler;
	public Transform sea;
	public Text feedbackText;
	private int currentLevel = 1;
	[SerializeField]private int finalLevel = 2;

	public Text gameText;
	public string endWarning = "Half a minute left! Speed it up!";
	[Header("Total game time is these together:")]
	public int mainGameDurationInSecs = 5;
	public float fadeTextInDuration = 4.5f;
	public int showBigTextDuration = 60;
	private Counter endGameCounter;

	[Header("Canvas components:")]
	public CanvasGroup menuPanel;
	public LevelInfoHandler levelInfoScript;

	[Header("Set to true in builds:")]
	public bool finalBuild;
	public GameObject noBuildObject;

	private bool gameIsRunning;
	private List<ManagedBehaviour> gameLoopDependables = new List<ManagedBehaviour> ();

	void Awake(){
		instance = this;
	}

	void Start(){
		endGameCounter = new Counter ();
		endGameCounter.onCount += ShowEndGameText;
		endGameCounter.StartCounter (mainGameDurationInSecs);
		gameText.text = endWarning;

		FishManager currentFishManager = sea.GetChild (currentLevel).GetComponent<FishManager> ();
		currentFishManager.StartSpawning ();
		infoHandler.LoadFishPoolsFromFishManager (currentFishManager);

		if (currentLevel == 1) {
			gameLoopDependables.Sort ();
			foreach (ManagedBehaviour behav in gameLoopDependables) {
				//Debug.Log (behav.name + " sortValue: " + behav.sortValue);
			}

			if (finalBuild) {
				menuPanel.Activate ();

				Time.timeScale = 0f;
				noBuildObject.SetActive (false);
				SceneManager.LoadSceneAsync (1, LoadSceneMode.Additive);
				SceneManager.sceneLoaded += OnEnvSceneLoaded;
			}
			else {
				gameIsRunning = true;
			}
		}
		else {
			gameIsRunning = true;
		}
	}

	void OnEnvSceneLoaded(Scene newScene, LoadSceneMode loadMode){
		SceneManager.SetActiveScene (newScene);		
	}

	//called on Awake
	public void InitGameLoopDependable(ManagedBehaviour behaviour){
		gameLoopDependables.Add (behaviour);
	}

	public void StartGame(){
		menuPanel.Deactivate ();
		levelInfoScript.ShowLevelInfo (StartPlaying);
	}

	private void StartPlaying(){
		Time.timeScale = 1f;
		gameIsRunning = true;		
	}

	void Update(){
		if (!gameIsRunning || gameLoopDependables.Count == 0)
			return;

		foreach (ManagedBehaviour behaviour in gameLoopDependables) {
			behaviour.ManagedUpdate ();
		}
	}

	void FixedUpdate(){
		if (!gameIsRunning || gameLoopDependables.Count == 0)
			return;

		foreach (ManagedBehaviour behaviour in gameLoopDependables) {
			behaviour.ManagedFixedUpdate ();
		}
	}

	public void GameFeedback(string feedback, bool showAsWarning = false){
		feedbackText.text = feedback;
		feedbackText.color = showAsWarning ? Color.red : Color.black;
	}

	void ShowEndGameText(){
		endGameCounter.onCount -= ShowEndGameText;
		endGameCounter.onCount += StartGameTextTimer;
		endGameCounter.StartCounter (fadeTextInDuration);

		gameText.enabled = true;
	}

	void StartGameTextTimer(){
		endGameCounter.StopCounter ();
		endGameCounter.onCount -= StartGameTextTimer;
		StartCoroutine (GameTextTimer ());
	}

	IEnumerator GameTextTimer(){
		float t = showBigTextDuration;
		while (t > 0f) {
			int timeLeft = Mathf.CeilToInt (t);
			gameText.text = timeLeft.ToString ();
			yield return null;
			t -= Time.deltaTime;
		}

		gameText.enabled = false;
		gameIsRunning = false;
		levelInfoScript.ShowLevelResults ();
	}

	public void LoadNextLevel(){
		currentLevel++;
		gameText.enabled = true;

		if (currentLevel > finalLevel) {
			gameText.text = "You completed the game!";
			gameIsRunning = false;
		}
		else {
			gameText.text = "Level " + currentLevel.ToString() + " begins!";
			endGameCounter.onCount += DisableGameText;
			endGameCounter.StartCounter (3f);
			Start ();
		}
	}

	void DisableGameText(){
		endGameCounter.onCount -= DisableGameText;
		gameText.enabled = false;
	}

	public void Restart(){
		SceneManager.LoadScene (0);
	}
}

public enum PlayerActivity{
	sailing,
	atDock
}