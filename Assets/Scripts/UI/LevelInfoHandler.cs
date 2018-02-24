using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class LevelInfoHandler : MonoBehaviour {

	public CanvasGroup startInfoPanel;
	public CanvasGroup endInfoPanel;

	private UnityAction startPlayingAction;

	private FishPoolInfo[] poolInfo;
	private List<FishPool> fishPools = new List<FishPool>();

	public GameObject continueButton;
	public GameObject restartButton;

	void Start(){
		poolInfo = GetComponentsInChildren<FishPoolInfo> ();

		startInfoPanel.Deactivate ();
		endInfoPanel.Deactivate ();
	}

	public void LoadFishPoolsFromFishManager(FishManager fishManager){//called on Start by GameManager
		fishPools.AddRange(fishManager.myFishPools);
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

		bool poolExterminated = false;
		for (int i = 0; i < fishPools.Count; i++) {
			bool _ext = false;
			poolInfo [i].Activate (fishPools [i], out _ext);

			if (_ext) {
				poolExterminated = true;
			}
		}

		if (poolExterminated) {
			restartButton.SetActive (true);
			continueButton.SetActive (false);
		}
		else {
			restartButton.SetActive (false);
			continueButton.SetActive (true);
		}
	}

	public void Continue(){
		endInfoPanel.Deactivate ();
		for (int i = 0; i < fishPools.Count; i++) {
			poolInfo [i].Deactivate ();
		}

		GameManager.instance.LoadNextLevel ();
	}
}
