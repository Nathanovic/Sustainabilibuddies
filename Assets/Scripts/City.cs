using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour {

	private Transform boatOrigin;

	[SerializeField]private int upgradeNetCost = 7;
	[SerializeField]private int upgradeNetSize = 1;
	[SerializeField]private int upgradeSpeedCost = 4;
	[SerializeField]private float upgradeSpeedAmount = 1f;

	void Start () {
		boatOrigin = transform.GetChild (0);		
	}

	public void EnterPort(TradePanel tradePanel, BoatController boatScript){
		boatScript.EnterPort (boatOrigin);
		tradePanel.Activate (this);
		//GameManager.instance.playerActivity = PlayerActivity.atDock;//wont work with two players!
	}

	public void LeavePort(BoatController boatScript){
		boatScript.LeavePort (boatOrigin);
	}

	public void TryUpgradeNet(PlayerEconomics ecoScript){
		if (ecoScript.fishCount >= upgradeNetCost) {
			ecoScript.UpgradeNet (upgradeNetCost, upgradeNetSize);
		}
	}

	public void TryUpgradeSpeed(PlayerEconomics ecoScript){
		if (ecoScript.fishCount >= upgradeSpeedCost) {
			ecoScript.UpgradeSpeed (upgradeSpeedCost, upgradeSpeedAmount);
		}
	}
}