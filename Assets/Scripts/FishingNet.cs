using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FishingNet : MonoBehaviour {

	private BoatController boatScript;
	private PlayerEconomics statsScript;
	private Collider myColl;
	private Renderer myRenderer;

	public bool active;

	public List<GameObject> fishesInNet = new List<GameObject> ();

	[SerializeField]private Image fillRect;
	public int maxFishCount;

	public FishNetState myState = FishNetState.netUp;

	void Start(){
		boatScript = transform.parent.GetComponent<BoatController> ();
		statsScript = transform.parent.GetComponent<PlayerEconomics> ();
		myColl = GetComponent<Collider> ();
		myRenderer = GetComponent<Renderer> ();
		fillRect.fillAmount = 0f;
		Deactivate ();
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.Space)) {
			if(myState == FishNetState.netUp)
				ThrowOutNet ();
			else if(myState == FishNetState.netDown)
				StartCoroutine(PullUpNet ());
		}
	}

	void ThrowOutNet(){
		GameManager.instance.GameFeedback ("You cast out your nets...");
		myState = FishNetState.netDown;
		active = true;
		myColl.enabled = true;
		myRenderer.enabled = true;
		boatScript.NetDown ();
	}

	void Deactivate(){
		active = false;
		myColl.enabled = false;
		myRenderer.enabled = false;		
	}

	void OnTriggerEnter(Collider other){
		Debug.Log ("trigger with: " + other.tag);
		if (other.tag == "Fish") {
			TryPutFishInNet (other.gameObject);
		}
	}

	void TryPutFishInNet(GameObject fish){
		Debug.Log ("try put fish in net");
		if (myState != FishNetState.netDown)
			return;

		fishesInNet.Add (fish);
		fish.SetActive (false);
		fillRect.fillAmount = (float)fishesInNet.Count / maxFishCount;

		if (fishesInNet.Count > maxFishCount) {
			GameManager.instance.GameFeedback ("All of your fishes escape!", true);
			foreach (GameObject f in fishesInNet) {
				f.SetActive (true);
			}
		}
	}

	IEnumerator PullUpNet(){
		GameManager.instance.GameFeedback ("You pull up your net...");
		myState = FishNetState.pullingNet;

		yield return new WaitForSeconds (4f);

		Deactivate ();
		fillRect.fillAmount = 0f;

		int fishCount = fishesInNet.Count;
		GameManager.instance.GameFeedback ("You find " + fishCount + " fishes in your net");
		statsScript.AddFish (fishCount);
		for (int i = 0; i < fishCount; i++) {
			GameObject.Destroy (fishesInNet [i]);
		}

		fishesInNet.Clear ();	
		boatScript.NetUp ();
		myState = FishNetState.netUp;
	}
}

[System.Serializable]
public enum FishNetState{
	netDown,
	pullingNet,
	netUp
}