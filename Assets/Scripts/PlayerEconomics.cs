using UnityEngine;
using UnityEngine.UI;

public class PlayerEconomics : MonoBehaviour {

	public int fishCount;
	public int cashCount;
	public Text fishText;

	private FishingNet fishingScript;
	private BoatController boatScript;

	void Start(){
		fishingScript = GetComponentInChildren<FishingNet> ();
		boatScript = GetComponent<BoatController> ();

		UpdateTexts ();
	}

	public void AddFish(){
		fishCount += 1;
		UpdateTexts ();
	}

	public void AddFish(int count){
		fishCount += count;
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
	}
}
