using UnityEngine;
using UnityEngine.UI;

public class TradePanel : MonoBehaviour {

	private PlayerEconomics playerEcoScript;
	private CanvasGroup cvg;
	public Text fishFeedbackText;

	private City currentCity;
	private int fishCost;

	void Start(){
		playerEcoScript = FindObjectOfType<PlayerEconomics> ();
		cvg = GetComponent<CanvasGroup> ();
		cvg.Deactivate ();
	}

	public void Activate(int _fishCost, City _currentCity){
		fishCost = _fishCost;
		currentCity = _currentCity;
		fishFeedbackText.text = "Heyy, makkerrrr, we geven jou " + fishCost.ToString () + " voor een vissie";

		cvg.Activate ();
	}

	public void SellFish(){//Button Event
		if (playerEcoScript.fishCount > 0) {
			playerEcoScript.SellFish (1, fishCost);
		}
	}

	public void LeavePort(){//Button Event
		currentCity.LeavePort ();
	}

	public void Deactivate(){
		cvg.Deactivate ();
	}
}
