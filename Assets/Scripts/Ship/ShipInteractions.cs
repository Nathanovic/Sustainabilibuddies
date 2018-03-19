using UnityEngine;

public delegate void DefaultDelegate();

//this script does all of the sensing between the boat and the other triggers
public class ShipInteractions : MonoBehaviour {

	public event DefaultDelegate onDockEntered;
	public delegate void BounceDelegate(Vector3 bounceDir);
	public event BounceDelegate onHitObstacle;

	void OnTriggerEnter(Collider other){
		if (other.tag == "Dock") {
			if (onDockEntered != null) {
				onDockEntered ();
			}
		}
	}

	void OnCollisionEnter(Collision coll){
		if (coll.collider.tag == "Obstacle") {
			Vector3 bounceDir = coll.relativeVelocity;
			bounceDir.y = 0f;
			onHitObstacle (bounceDir);

			Debug.Log("bounce with magnitude: " + bounceDir.magnitude.ToString());
			//if(Physics.Raycast(transform.position, coll.collider.po
			//DebugBounce (bounceDir);
		}
	}

	void DebugBounce(Vector3 bounceDir){
		bounceDir.y = 0f;
		Debug.DrawRay (transform.position, bounceDir, Color.red, 1f);
		GetComponent<Rigidbody> ().AddForce (bounceDir);
	}
}