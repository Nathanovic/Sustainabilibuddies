using UnityEngine;
using UnityEngine.UI;

public class PlayerEconomics : MonoBehaviour {

	public int fishCount;
	public Text fishText;

	public void AddFish(){
		fishCount++;
		fishText.text = "Fishes: " + fishCount.ToString ();
	}
}
