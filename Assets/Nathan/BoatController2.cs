using UnityEngine;

//handles all of the movement, and nothing more!
public class BoatController2 : MonoBehaviour {

	private float forwardSpeed;
	public BoatStats myStats;
	[SerializeField]private  VirtualJoystick joystickScript;
	private Rigidbody rb;

	void Start () {
		rb = GetComponent<Rigidbody> ();
	}

	void FixedUpdate () {
		Vector3 targetDir = joystickScript.movementVector;

		//rotation
		Quaternion targetRot = Quaternion.LookRotation (targetDir);
		Quaternion newRot = Quaternion.RotateTowards (transform.rotation, targetRot, myStats.rotationSpeed);
		rb.MoveRotation (newRot);

		//movement:
		/*
		forwardSpeed -= myStats.deceleration;
		if(targetDir.sqrMagnitude > 0f){
			forwardSpeed += targetDir.magnitude * myStats.acceleration;
		}

		Vector3 targetVelocity = targetDir * forwardSpeed;
		targetVelocity = CompromisedVelocity (targetVelocity, rb.velocity);
		rb.velocity = targetVelocity;
		*/
		rb.AddForce (transform.forward * targetDir.magnitude * 2f);
		Vector3 velocity = rb.velocity;
		velocity = Vector3.ClampMagnitude (velocity, myStats.maxForwardSpeed);
		rb.velocity = velocity;
	}

	Vector3 CompromisedVelocity(Vector3 targetVelocity, Vector3 currentVelocity){
		float otherFactor = 1f - myStats.drag;
		Vector3 newVelocity = currentVelocity * myStats.drag + targetVelocity * otherFactor;
		newVelocity = Vector3.ClampMagnitude (newVelocity, myStats.maxForwardSpeed);

		return newVelocity;
	}
}

[System.Serializable]
public class BoatStats{
	public float acceleration;
	public float deceleration;
	public float maxForwardSpeed;
	public float rotationSpeed;
	[Range(0f, 1f)]public float drag;
}