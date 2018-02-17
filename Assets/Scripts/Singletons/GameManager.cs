using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager instance;
	public PlayerActivity playerActivity;
	public Text feedbackText;

	public Text endingText;
	public string endWarning = "Half a minute left! Speed it up!";
	[Header("Total game time is these together:")]
	public int mainGameDurationInSecs = 5;
	public float fadeTextInDuration = 4.5f;
	public int gameEndingInSecs = 60;
	private Counter endGameCounter;

	public CanvasGroup startCVG;

	[Header("Set to true in builds:")]
	public bool finalBuild;
	public GameObject noBuildObject;

	private bool gameIsRunning;
	private List<IRanByGameManager> gameLoopDependables = new List<IRanByGameManager> ();

	void Awake(){
		instance = this;
	}

	IEnumerator Start(){
		endGameCounter = new Counter ();
		endGameCounter.onCount += ShowEndGameText;
		endGameCounter.StartCounter (mainGameDurationInSecs);
		endingText.text = endWarning;

		if (finalBuild) {
			startCVG.Activate ();
			yield return null;

			Time.timeScale = 0f;
			noBuildObject.SetActive (false);
			SceneManager.LoadSceneAsync (1, LoadSceneMode.Additive);
			SceneManager.sceneLoaded += OnEnvSceneLoaded;
		}
		else {
			gameIsRunning = true;
		}
	}

	void OnEnvSceneLoaded(Scene newScene, LoadSceneMode loadMode){
		SceneManager.SetActiveScene (newScene);		
	}

	public void InitGameLoopDependable(IRanByGameManager behaviour){
		gameLoopDependables.Add (behaviour);
	}

	public void StartGame(){
		Time.timeScale = 1f;
		gameIsRunning = true;
		startCVG.Deactivate ();
	}

	void Update(){
		if (!gameIsRunning || gameLoopDependables.Count == 0)
			return;

		foreach (IRanByGameManager behaviour in gameLoopDependables) {
			behaviour.ManagedUpdate ();
		}
	}

	public bool CanSail(){
		if (playerActivity == PlayerActivity.sailing) {
			return true;
		}

		return false;
	}

	public void GameFeedback(string feedback, bool showAsWarning = false){
		feedbackText.text = feedback;
		feedbackText.color = showAsWarning ? Color.red : Color.black;
	}

	void ShowEndGameText(){
		endGameCounter.onCount -= ShowEndGameText;
		endGameCounter.onCount += StartEndGameTimer;
		endGameCounter.StartCounter (fadeTextInDuration);

		endingText.gameObject.SetActive (true);
	}

	void StartEndGameTimer(){
		endGameCounter.StopCounter ();
		StartCoroutine (EndGameTimer ());
	}

	IEnumerator EndGameTimer(){
		float t = gameEndingInSecs;
		while (t > 0f) {
			int timeLeft = Mathf.CeilToInt (t);
			endingText.text = timeLeft.ToString ();
			yield return null;
			t -= Time.deltaTime;
		}

		endingText.text = "Lol, you lost!";
	}
}

public enum PlayerActivity{
	sailing,
	atDock
}