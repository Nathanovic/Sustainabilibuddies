using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager instance;
	public PlayerActivity playerActivity;
	public Text feedbackText;

	public Text endingText;
	[Header("Total game time is these together:")]
	public int mainGameDurationInSecs = 5;
	public float fadeTextInDuration = 4.5f;
	public int gameEndingInSecs = 60;
	private Counter endGameCounter;

	void Awake(){
		instance = this;
	}

	void Start(){
		endGameCounter = new Counter ();
		endGameCounter.onCount += ShowEndGameText;
		endGameCounter.StartCounter (mainGameDurationInSecs);
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

	}
}

public enum PlayerActivity{
	sailing,
	atDock
}