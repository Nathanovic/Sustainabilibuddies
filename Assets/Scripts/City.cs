using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour {

	private Transform boatOrigin;
	public TradePanel tradePanel;
	public int fishCost;
	public BoatController boat;

	void Start () {
		boatOrigin = transform.GetChild (0);		
	}

	public void EnterPort(){
		tradePanel.Activate (fishCost, this);
		boat.EnterPort (boatOrigin);
		GameManager.instance.playerActivity = PlayerActivity.atDock;
	}

	public void LeavePort(){
		tradePanel.Deactivate ();
		boat.LeavePort (boatOrigin);
		GameManager.instance.playerActivity = PlayerActivity.sailing;
	}
}