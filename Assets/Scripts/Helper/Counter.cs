using UnityEngine;

public class Counter{

	private float countTime;
	private float startCountTime;
	private bool count;

	public event DefaultDelegate onCount;

	public Counter(){
		CountManager.instance.InitCounter (this);
	}

	public void StartCounter(float _countTime){
		count = true;
		startCountTime = Time.time; 
		countTime = _countTime;
	}

	public void Update(){
		if (count && (Time.time - startCountTime) >= countTime) {
			onCount ();
			startCountTime = Time.time; 
		}
	}

	public void StopCounter(){
		count = false;
	}
}