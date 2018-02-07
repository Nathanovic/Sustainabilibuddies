using UnityEngine;
using System.Collections.Generic;

public class CountManager : MonoBehaviour {

	public static CountManager instance;

	private List<Counter> myCounters = new List<Counter>();

	public void InitCounter(Counter newCounter){
		myCounters.Add (newCounter);
	}

	void Update(){
		for(int i = 0; i < myCounters.Count; i ++){
			myCounters [i].updateCounter ();
		}
	}
}
