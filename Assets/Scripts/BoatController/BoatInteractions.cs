using UnityEngine;

public delegate void DefaultDelegate();

public class BoatInteractions : MonoBehaviour {

	private BoatController boatScript;
	public TradePanel tradePanel;

	public int getFishCooldown = 5;

	void Start(){
		boatScript = GetComponent<BoatController> ();
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "City" && !boatScript.inPort) {
			Debug.Log ("enter port");
			City currentCity = other.transform.parent.GetComponent<City> ();
			currentCity.EnterPort (tradePanel, boatScript);
		}
	}
}