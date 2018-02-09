using System.Collections.Generic;
using UnityEngine;

public class FishPool : MonoBehaviour {

	private FishConfig config;

	private Counter fishInMeCounter;
	private Counter spawnFishCounter;

	public int boostedFishRespawnWaitTime;//de respawnrate is boosted na deze waittime
	public float minRespawnTime = 7f;
	public float fishRespawnTime = 10f;
	public float boostedFishRespawnRate = 1.3f;
	private float currentFishRespawnRate;

	public GameObject fishPrefab;
	public List<Fish> myFishes;//QQQ
	public int fishCount;
	public float poolRadius;

	void Start () {
		config = GetComponent<FishConfig> ();

		myFishes = new List<Fish> (fishCount * 2);
		for (int i = 0; i < fishCount; i++) {
			SpawnFish ();
		}

		fishInMeCounter = new Counter ();
		fishInMeCounter.onCount += BoostRespawnRate;
		SetRespawnRateToDefault ();

		spawnFishCounter = new Counter ();
		spawnFishCounter.onCount += SpawnFishAfterTime;
		spawnFishCounter.StartCounter (RespawnFishTime());
	}

	void SpawnFish(){
		Vector3 pos = transform.position + Random.insideUnitSphere * poolRadius;
		pos.y = transform.position.y + Random.Range (-0.25f, 0.25f);
		GameObject newFish = GameObject.Instantiate (fishPrefab, pos, Quaternion.identity, transform);
		Fish fish = newFish.GetComponent<Fish> ();
		myFishes.Add (fish);
		fish.Init (this, config);		
	}

	void SpawnFishAfterTime(){
		SpawnFish();
		spawnFishCounter.StartCounter (RespawnFishTime());
	}

	void OnDrawGizmosSelected(){
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere (transform.position, poolRadius);
	}

	void BoostRespawnRate(){
		fishInMeCounter.StartCounter (boostedFishRespawnWaitTime);
		currentFishRespawnRate = boostedFishRespawnRate;		
	}

	void SetRespawnRateToDefault(){
		fishInMeCounter.StartCounter (boostedFishRespawnWaitTime);
		currentFishRespawnRate = 1f;
	}

	float RespawnFishTime(){
		return minRespawnTime + fishRespawnTime / (myFishes.Count * currentFishRespawnRate);
	}

	public List<Fish> GetNeighbors(Fish fish, float radius){
		List<Fish> neighbors = new List<Fish> ();
		for (int i = 0; i < myFishes.Count; i++) {
			Fish other = myFishes [i];
			if (fish == other)
				continue;

			if (Vector3.Distance (fish.position, other.position) <= radius) {
				neighbors.Add (other);
			}
		}

		return neighbors;
	}
}
