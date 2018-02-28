using System.Collections.Generic;
using UnityEngine;
using FishFSM;

public class Fish : MonoBehaviour {

	private FiniteStateMachine fsm;

	private FishModel myData;

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

		myData = GetComponent<FishModel> ();
		myData.onCaught += CaughtInNet;
		myData.onEscaped += EscapeFromNet;

		State[] myStates = new State[1];
		myStates [0] = new DefaultState ();
		fsm = new FiniteStateMachine (myStates, this, myData);
	}

	void Update(){
		Vector3 combineVec = fsm.currentState.GetCombinedVector ();
		acceleration += combineVec;
		acceleration = Vector3.ClampMagnitude (acceleration, conf.maxAcceleration);
		velocity += acceleration * Time.deltaTime;
		velocity = Vector3.ClampMagnitude (velocity, conf.maxVelocity);
		position += velocity * Time.deltaTime;
		transform.position = position;

		if (combineVec == Vector3.zero && velocity != Vector3.zero) {
			acceleration = Vector3.zero;
			velocity = Vector3.MoveTowards (velocity, Vector3.zero, conf.deceleration * Time.deltaTime);
		}else{
			transform.LookAt(transform.position + velocity);
		}
	}

	public Vector3 Separation(){
		Vector3 vec = new Vector3 ();

		List<Fish> neighbors = myPool.GetNeighbors (this, conf.separationRadius);
		if (neighbors.Count == 0) 
			return vec;

		foreach (Fish fish in neighbors) {
			Vector3 avoidDir = position - fish.position;
			vec += avoidDir.normalized / avoidDir.magnitude;
		}

		return vec.normalized * conf.separationPriority;
	}

	public Vector3 Cohesion(){
		Vector3 vec = new Vector3 ();

		List<Fish> neighbors = myPool.GetNeighbors (this, conf.cohesionRadius);
		if (neighbors.Count == 0) 
			return vec;

		foreach (Fish fish in neighbors) {
			if (OtherInFOV (fish.position)) {
				Vector3 dir = fish.position - position;
				vec += dir.normalized;
			}
		}

		return vec.normalized * conf.cohesionPriority;
	}

	public Vector3 Alignment(){
		Vector3 vec = new Vector3 ();

		List<Fish> neighbors = myPool.GetNeighbors (this, conf.alignmentRadius);
		if (neighbors.Count == 0) 
			return vec;

		foreach (Fish fish in neighbors) {
			if (OtherInFOV (fish.position)) {
				float priority = (fish.position - position).magnitude;
				vec += fish.velocity * priority;
			}
		}

		return vec.normalized * conf.alignmentPriority;
	}

	public Vector3 GoalSteering(){
		Vector3 vec = transform.parent.position - transform.position;
		return vec.normalized * conf.seekTargetPriority;
	}

	float RandomVal(float r){
		return Random.Range(-r, r);
	}

	void CaughtInNet(){
		Renderer[] visuals = GetComponentsInChildren<Renderer> ();
		for (int i = 0; i < visuals.Length; i++) {
			visuals [i].enabled = false;
		}

		myPool.FishCaught (this);
	}
	void EscapeFromNet(){
		Renderer[] visuals = GetComponentsInChildren<Renderer> ();
		for (int i = 0; i < visuals.Length; i++) {
			visuals [i].enabled = true;
		}		

		myPool.FishEscaped (this);
	}

	public bool OtherInFOV(Vector3 otherPos){
		return Vector3.Angle (velocity, otherPos - position) <= conf.vof;
	}

	public float RandomBinomial(){
		return Random.Range (0f, 1f) - Random.Range (0f, 1f);
	}

	void OnDrawGizmosSelected(){
		Gizmos.color = new Color (0,1f,0f);
		Gizmos.DrawWireSphere (position, conf.separationRadius);
		Gizmos.color = new Color (0.5f,0.5f,0f);
		Gizmos.DrawWireSphere (position, conf.cohesionRadius);
		Gizmos.color = new Color (1f,0f,0f);
		Gizmos.DrawWireSphere (position, conf.alignmentRadius);
	}
}
