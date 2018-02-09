using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour {

	private PlayerInput inputScript;
	private Rigidbody rb;
	private Transform boatMesh;

	[Header("Move behaviour:")]
	[Range(0f,1f)]
	public float netOutSpeedFactor = 0.7f;
	public float moveSpeedAcceleration;
	public float maxSpeed = 20f;

	[Range(0f,1f)]
	public float changeDirectionCompromiser = 0.8f;

	private float currentSpeedFactor = 1f;

	[Header("Rotate behaviour:")]
	public float rotationAcceleration;
	public float maxRotationSpeed;

	public float rotationDegenSpeed;

	private float rotateSpeed;

	void Start () {
		inputScript = GetComponent<PlayerInput> ();
		rb = GetComponent<Rigidbody> ();
		boatMesh = transform.GetChild (0);
	}

	void FixedUpdate() {
		//get input:
		float moveInput = inputScript.moveInput;
		float horizontalInput = inputScript.horizontalInput;
		if (inPort) {
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
			return;
		}

		//movement:
		float forwardSpeed = moveInput * moveSpeedAcceleration * currentSpeedFactor;
		Vector3 targetVelocity = transform.forward * forwardSpeed;
		CompromiseVelocity (ref targetVelocity, rb.velocity);
		if(GameManager.instance.CanSail ())
			targetVelocity += Sea.instance.seaCurrent;
		targetVelocity.y = 0f;

		rb.velocity = targetVelocity;

		//rotation setvalue:
		float rotateAcceleration = horizontalInput * Time.deltaTime * rotationAcceleration;
		rotateSpeed += rotateAcceleration;
		float maxRotSpeed = 0.5f * maxRotationSpeed + maxRotationSpeed * (rb.velocity.magnitude / (maxSpeed * 2));
		rotateSpeed = Mathf.Clamp (rotateSpeed, -maxRotSpeed, maxRotSpeed);

		if(horizontalInput == 0)
			StabilizeFloat (ref rotateSpeed, maxRotationSpeed, rotationDegenSpeed);

		//rotate to input
		Vector3 rotEuler = transform.rotation.eulerAngles;
		rotEuler.y += rotateSpeed;
		boatMesh.transform.localEulerAngles = new Vector3 (-forwardSpeed * 12f, 0f, -rotateSpeed * 5);

		//rotate with the current:
		Quaternion velocityRot = Quaternion.LookRotation (targetVelocity);
		Quaternion updatedRot = Quaternion.Euler (rotEuler);
		updatedRot = Quaternion.Slerp (updatedRot, velocityRot, 0.01f);
		rb.MoveRotation (updatedRot);
	}

	void StabilizeFloat(ref float value, float max, float degeneration){
		if (value > 0) {
			value -= degeneration * Time.fixedDeltaTime;
			value = Mathf.Clamp (value, 0f, max);
		} else if (value < 0) {
			value += degeneration * Time.fixedDeltaTime;
			value = Mathf.Clamp (value, -max, 0f);		
		}
	}

	void CompromiseVelocity(ref Vector3 newVelocity, Vector3 currentVelocity){
		float otherFactor = 1f - changeDirectionCompromiser;
		float newSpeed = newVelocity.magnitude + currentVelocity.magnitude;
		newSpeed = Mathf.Min (newSpeed, maxSpeed);
		newVelocity = newVelocity * changeDirectionCompromiser + currentVelocity * otherFactor;//change direction
		newVelocity = newVelocity.normalized * newSpeed;
	}

	public bool inPort;
	public void EnterPort(Transform dock){
		inPort = true;
		rotateSpeed = 0f;

		transform.position = dock.position;
		transform.rotation = dock.rotation;
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
	}

	public void LeavePort(Transform dock){
		inPort = false;

		transform.position = dock.position;
		transform.rotation = dock.rotation;
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;		
	}

	public void NetDown(){
		currentSpeedFactor = netOutSpeedFactor;
	}

	public void NetUp(){
		currentSpeedFactor = 1f;
	}

	public void UpgradeSpeed(float extraSpeed){
		maxSpeed += extraSpeed;
	}
}
