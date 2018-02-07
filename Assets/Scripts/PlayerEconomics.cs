using UnityEngine;
using UnityEngine.UI;

public class PlayerEconomics : MonoBehaviour {

	public int fishCount;
	public int cashCount;
	public Text fishText;
	public Text cashText;

	void Start(){
		UpdateTexts ();
	}

	public void AddFish(){
		fishCount += 1;
		UpdateTexts ();
	}

	public void SellFish(int fish, int cost){
		fishCount -= fish;
		cashCount += cost;
		UpdateTexts ();
	}

	void UpdateTexts(){
		fishText.text = "Fishes: " + fishCount.ToString ();		
		cashText.text = "Cash: " + cashCount.ToString ();
	}
}
