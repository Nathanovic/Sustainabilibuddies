using System.Collections.Generic;
using UnityEngine;

//determines the game loop for all fishes in the same level
public class FishManager : MonoBehaviour {

	public FishPool[] myFishPools;

	public void StartSpawning(){
		myFishPools = GetComponentsInChildren<FishPool> ();
		for (int i = 0; i < myFishPools.Length; i++) {
			myFishPools [i].StartSpawning ();
		}	
	}
}
