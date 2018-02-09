using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour {

	private FishPool myPool;
	private FishConfig conf;

	private Vector3 acceleration;
	public Vector3 position;
	public Vector3 velocity;

	public void Init(FishPool pool, FishConfig config){
		myPool = pool;
		conf = config;
		position = transform.position;
		velocity = new Vector3(RandomVal(0.1f), RandomVal(0.05f), RandomVal(0.1f));
	}

	void Update(){
		Vector3 combineVec = Combine ();
		acceleration += combineVec;
		acceleration = Vector3.ClampMagnitude (acceleration, conf.maxAcceleration);
		velocity += acceleration * Time.deltaTime;
		velocity = Vector3.ClampMagnitude (velocity, conf.maxVelocity);
		position += velocity * Time.deltaTime;
		transform.position = position;

		if (combineVec == Vector3.zero && velocity != Vector3.zero) {
			acceleration = Vector3.zero;
			velocity = Vector3.MoveTowards (velocity, Vector3.zero, conf.deceleration * Time.deltaTime);
		}
	}

	Vector3 Avoidance(){
		Vector3 avoidVec = new Vector3 ();

		List<Fish> neighbors = myPool.GetNeighbors (this, conf.avoidanceRadius);
		if (neighbors.Count == 0) 
			return avoidVec;

		foreach (Fish fish in neighbors) {
			Vector3 avoidDir = position - fish.position;
			avoidVec += avoidDir.normalized / avoidDir.magnitude;
		}

		return avoidVec.normalized;
	}

	Vector3 Combine(){
		Vector3 combinedVec = Avoidance () * conf.avoidancePriority;
		return combinedVec;
	}

	Vector3 NeutralizedVelocity(){
		return Vector3.zero;
	}

	float RandomVal(float r){
		return Random.Range(-r, r);
	}
}
