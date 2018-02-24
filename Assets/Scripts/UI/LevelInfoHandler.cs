using UnityEngine;
using UnityEngine.Events;

public class LevelInfoHandler : MonoBehaviour {

	public CanvasGroup startInfoPanel;
	public CanvasGroup endInfoPanel;

	private UnityAction startPlayingAction;

	void Start(){
		startInfoPanel.Deactivate ();
		endInfoPanel.Deactivate ();
	}

	public void ShowLevelInfo(UnityAction startPlaying){
		startInfoPanel.Activate ();
		startPlayingAction = startPlaying;
	}

	public void DeactivateStartInfo(){
		startPlayingAction ();
		startInfoPanel.Deactivate ();
	}

	public void ShowLevelResults(){
		endInfoPanel.Activate ();
	}
}
