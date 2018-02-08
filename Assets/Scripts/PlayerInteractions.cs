using UnityEngine;

public delegate void DefaultDelegate();

public class PlayerInteractions : MonoBehaviour {

	private PlayerEconomics economicScript;
	public TradePanel tradePanel;
	private Counter fishCounter;

	public int getFishCooldown = 5;

	void Start(){
		economicScript = GetComponent<PlayerEconomics> ();
		fishCounter = new Counter ();
		fishCounter.onCount += economicScript.AddFish;
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "FishPool") {
			fishCounter.StartCounter (getFishCooldown);
		} else if (other.tag == "City") {
			City currentCity = other.GetComponent<City> ();
			currentCity.EnterPort ();
		}
	}

	void OnTriggerExit(Collider other){
		if (other.tag == "FishPool") {
			fishCounter.StopCounter ();
		}
	}
}