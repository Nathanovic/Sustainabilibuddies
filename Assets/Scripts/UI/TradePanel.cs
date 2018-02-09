using UnityEngine;
using UnityEngine.UI;

public class TradePanel : MonoBehaviour {

	private CanvasGroup cvg;
	public Text fishFeedbackText;

	[SerializeField]private City dock;

	public BoatController boatScript;
	public PlayerEconomics ecoScript;

	void Start(){
		cvg = GetComponent<CanvasGroup> ();
		cvg.Deactivate ();
	}

	public void Activate(City _currentCity){
		dock = _currentCity;

		cvg.Activate ();
	}

	public void UpgradeNet(){//Button Event
		dock.TryUpgradeNet(ecoScript);
	}
	public void UpgradeSpeed(){//Button Event
		dock.TryUpgradeSpeed(ecoScript);
	}
	public void LeavePort(){//Button Event
		dock.LeavePort (boatScript);
		cvg.Deactivate ();
	}
}
