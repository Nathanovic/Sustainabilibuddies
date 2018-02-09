using UnityEngine;

public delegate void DefaultDelegate();

public class BoatInteractions : MonoBehaviour {

	private PlayerEconomics economicScript;
	private BoatController boatScript;
	public TradePanel tradePanel;
	private Counter fishCounter;

	public int getFishCooldown = 5;

	void Start(){
		economicScript = GetComponent<PlayerEconomics> ();
		boatScript = GetComponent<BoatController> ();
		fishCounter = new Counter ();
		fishCounter.onCount += economicScript.AddFish;
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "FishPool") {
			fishCounter.StartCounter (getFishCooldown);
		} else if (other.tag == "City" && !boatScript.inPort) {
			Debug.Log ("enter port");
			City currentCity = other.transform.parent.GetComponent<City> ();
			currentCity.EnterPort (tradePanel, boatScript);
		}
	}

	void OnTriggerExit(Collider other){
		if (other.tag == "FishPool") {
			fishCounter.StopCounter ();
		}
	}
}