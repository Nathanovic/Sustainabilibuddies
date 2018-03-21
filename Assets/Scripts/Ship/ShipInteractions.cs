using UnityEngine;

public delegate void DefaultDelegate();

//this script does all of the sensing between the boat and the other triggers
public class ShipInteractions : MonoBehaviour {

	public event DefaultDelegate onDockEntered;

	void OnTriggerEnter(Collider other){
		if (other.tag == "Dock") {
			if (onDockEntered != null) {
				onDockEntered ();
			}
		}
	}
}