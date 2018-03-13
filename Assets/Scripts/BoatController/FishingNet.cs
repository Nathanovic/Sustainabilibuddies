using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FishingNet : MonoBehaviour {

	private Collider myColl;
	private Renderer myRenderer;
	private ShipStats shipStats;

	public List<GameObject> fishesInNet = new List<GameObject> ();

	[SerializeField]private Image fillRect;

	public FishNetState myState = FishNetState.netUp;

	public event DefaultDelegate onNetDown;
	public delegate void NetUpEvent(int fishCount);
	public event NetUpEvent onNetUp;

	private bool canHandleNet;

	//shipStats implementation:
	private int maxFishCount{
		get{
			return shipStats.netSize;
		}
	}

	void Start(){
		shipStats = transform.root.GetComponent<ShipStats> ();
		myColl = GetComponent<Collider> ();
		myRenderer = GetComponent<Renderer> ();
		fillRect.fillAmount = 0f;
		Deactivate ();

		//make sure we can only use the net if the game manager allows us to:
		GameManager.instance.onBoatControlsDisabled += BoatControlsDisabled;
		GameManager.instance.onBoatControlsEnabled += BoatControlsEnabled;
	}

	void Update(){
		if (Input.GetKeyUp(KeyCode.Space)) {
			ToggleNet ();
		}
	}

	public void ToggleNet(){
		if(myState == FishNetState.netUp)
			ThrowOutNet ();
		else if(myState == FishNetState.netDown || myState == FishNetState.netBroken)
			StartCoroutine(PullUpNet ());		
	}

	void ThrowOutNet(){
		GameManager.instance.GameFeedback ("You cast out your nets...");
		myState = FishNetState.netDown;
		myColl.enabled = true;
		myRenderer.enabled = true;

		if (onNetDown != null) {
			onNetDown ();
		}
	}

	void Deactivate(){
		myColl.enabled = false;
		myRenderer.enabled = false;		
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "Fish") {
			TryPutFishInNet (other.gameObject);
		}
	}

	void TryPutFishInNet(GameObject fishObject){
		if (myState != FishNetState.netDown)
			return;
		
		if (fishesInNet.Count == maxFishCount) {
			myState = FishNetState.netBroken;
			GameManager.instance.GameFeedback ("All of your fishes escape!", true);
			foreach (GameObject f in fishesInNet) {
				FishModel fish = f.GetComponent<FishModel> ();
				fish.Escape ();
			}

			fillRect.fillAmount = 0f;
			fishesInNet.Clear ();
		} else {
			fishesInNet.Add (fishObject);
			FishModel fish = fishObject.GetComponent<FishModel> ();
			fish.CatchMe (transform);
		}

		fillRect.fillAmount = (float)fishesInNet.Count / maxFishCount;
	}

	IEnumerator PullUpNet(){
		GameManager.instance.GameFeedback ("You pull up your net...");
		myState = FishNetState.pullingNet;

		yield return new WaitForSeconds (4f);

		Deactivate ();
		fillRect.fillAmount = 0f;

		int fishCount = fishesInNet.Count;
		GameManager.instance.GameFeedback ("You find " + fishCount + " fishes in your net");
		for (int i = 0; i < fishCount; i++) {
			GameObject.Destroy (fishesInNet [i]);
		}

		fishesInNet.Clear ();	
		myState = FishNetState.netUp;

		if (onNetUp != null) {
			onNetUp (fishCount);
		}
	}

	private void BoatControlsDisabled(){
		canHandleNet = false;
	}

	private void BoatControlsEnabled(){
		canHandleNet = true;
	}
}

[System.Serializable]
public enum FishNetState{
	netDown,
	pullingNet,
	netUp,
	netBroken
}