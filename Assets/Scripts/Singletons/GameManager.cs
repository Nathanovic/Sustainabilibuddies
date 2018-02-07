using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance;
	public PlayerActivity playerActivity;

	void Awake(){
		instance = this;
	}

	public bool CanSail(){
		if (playerActivity == PlayerActivity.sailing) {
			return true;
		}

		return false;
	}
}

public enum PlayerActivity{
	sailing,
	atDock
}