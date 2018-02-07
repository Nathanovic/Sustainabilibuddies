using UnityEngine;
using System.Collections;

public delegate void DefaultDelegate();

public class Fisher : MonoBehaviour {

	private PlayerEconomics economicScript;
	public Counter fishCounter;

	void Start(){
		economicScript = GetComponent<PlayerEconomics> ();
		fishCounter = new Counter (5f);
		fishCounter.onCount += economicScript.AddFish;
	}

	void Update(){
		//if(isFishing
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "Fish") {
			fishCounter.StartCounter ();
		}
	}

	void OnTriggerExit(Collider other){
		if (other.tag == "Fish") {
			fishCounter.StopCounter ();
		}
	}
}

public class Counter{
	private float startCountTime;
	private bool count;
	//private bool 
	public float countTime;
	public event DefaultDelegate onCount;
	public event DefaultDelegate updateCounter;

	public Counter(float _countTime){
		countTime = _countTime;
		updateCounter += Update;
		onCount += StartCounter;
	}

	public void StartCounter(){
		count = true;
		startCountTime = Time.time; 
	}

	void Update(){
		if (count && (Time.time - startCountTime) >= countTime) {
			onCount ();
		}
	}

	public void StopCounter(){
		count = false;
	}
}