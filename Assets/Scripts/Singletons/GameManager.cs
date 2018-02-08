using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager instance;
	public PlayerActivity playerActivity;
	public Text feedbackText;

	void Awake(){
		instance = this;
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
}

public enum PlayerActivity{
	sailing,
	atDock
}