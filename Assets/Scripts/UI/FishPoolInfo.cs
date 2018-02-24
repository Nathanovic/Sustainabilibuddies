using UnityEngine;
using UnityEngine.UI;

public class FishPoolInfo : MonoBehaviour {

	private CanvasGroup cvg;
	private Text fishSpecieText;
	private Text fishInfoText;
	private Text fishResultText;

	void Start () {
		cvg = GetComponent<CanvasGroup> ();
		cvg.Deactivate ();
		fishSpecieText = transform.GetChild (0).GetComponent<Text> ();
		fishInfoText = transform.GetChild (1).GetComponent<Text> ();
		fishResultText = transform.GetChild (2).GetComponent<Text> ();
	}

	public void Activate(FishPool fishPool, out bool poolExterminated){
		poolExterminated = false;
		cvg.Activate ();

		fishSpecieText.text = "Specie: " + fishPool.fishName;
		int remainedFish = fishPool.RemainingFishCount ();
		fishInfoText.text = "School size/maximum fish amount: " + remainedFish.ToString () + "/" + fishPool.maxFishAmount;

		float fishPercentage = (float)remainedFish / fishPool.maxFishAmount;
		string fishPopulationHealt = "exterminated...";

		if (remainedFish < fishPool.minFishAmount) {
			poolExterminated = true;			
		}
		else if (fishPercentage > 0f && fishPercentage <= 0.33333f) {
			fishPopulationHealt = "endangered";
		}
		else if (fishPercentage > 0.333f && fishPercentage < 0.66666f) {
			fishPopulationHealt = "stable";
		}
		else if (fishPercentage >= 0.66666f) {
			fishPopulationHealt = "healthy";
		}

		fishResultText.text = "Fish population health: " + fishPopulationHealt;
	}

	public void Deactivate(){
		cvg.Deactivate ();
	}
}
