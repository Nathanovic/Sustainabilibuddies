using UnityEngine;
using UnityEngine.UI;

public class PlayerEconomics : MonoBehaviour {

	public int fishCount;
	public int cashCount;
	public Text fishText;

	private FishingNet fishingScript;
	private BoatController boatScript;

	private int myScore;
	public Text scoreText;

	void Start(){
		fishingScript = GetComponentInChildren<FishingNet> ();
		boatScript = GetComponent<BoatController> ();

		fishingScript.onNetUp += AddFish;

		UpdateTexts ();
	}

	private void AddFish(int count){
		fishCount += count;
		myScore += count;
		UpdateTexts ();
	}

	public void UpgradeNet(int fish, int extraNetSize){
		UseFishForUpgrade (fish);
		fishingScript.UpgradeNetSize (extraNetSize);
	}

	public void UpgradeSpeed(int fish, float extraSpeed){
		UseFishForUpgrade (fish);
		boatScript.UpgradeSpeed (extraSpeed);
	}

	void UseFishForUpgrade(int fish){
		fishCount -= fish;
		UpdateTexts ();
	}

	void UpdateTexts(){
		fishText.text = "Fishes: " + fishCount.ToString ();	
		scoreText.text = "Score: " + myScore.ToString ();
	}
}
