using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour {

	private Rigidbody rb;

	[Header("Move behaviour:")]
	public float moveSpeedAcceleration;
	public float maxSpeed = 20f;

	[Range(0f,1f)]
	public float changeDirectionCompromiser = 0.8f;

	private float moveInput;

	[Header("Rotate behaviour:")]
	public float rotationAcceleration;
	public float maxRotationSpeed;

	public float rotationDegenSpeed;

	private float rotateSpeed;

	void Start () {
		rb = GetComponent<Rigidbody> ();
	}

	void Update () {
		if (!GameManager.instance.CanSail ())
			return;

		float horizontalInput = Input.GetAxis ("Horizontal");
		moveInput = Mathf.Clamp (Input.GetAxis ("Vertical"), 0f, 1f);

		float rotateAcceleration = horizontalInput * Time.deltaTime * rotationAcceleration;
		rotateSpeed += rotateAcceleration;
		float maxRotSpeed = 0.5f * maxRotationSpeed + maxRotationSpeed * (rb.velocity.magnitude / (maxSpeed * 2));
		rotateSpeed = Mathf.Clamp (rotateSpeed, -maxRotSpeed, maxRotSpeed);

		if(horizontalInput == 0)
			StabilizeFloat (ref rotateSpeed, maxRotationSpeed, rotationDegenSpeed);
	}

	void FixedUpdate() {
		//movement:
		Vector3 targetVelocity = transform.forward * moveInput * moveSpeedAcceleration;
		CompromiseVelocity (ref targetVelocity, rb.velocity);
		if(GameManager.instance.CanSail ())
			targetVelocity += Sea.instance.seaCurrent;
		
		rb.velocity = targetVelocity;

		//rotation:
		Vector3 rotEuler = transform.rotation.eulerAngles;
		rotEuler.y += rotateSpeed;
		rb.MoveRotation (Quaternion.Euler(rotEuler));
	}

	void StabilizeFloat(ref float value, float max, float degeneration){
		if (value > 0) {
			value -= degeneration * Time.deltaTime;
			value = Mathf.Clamp (value, 0f, max);
		} else if (value < 0) {
			value += degeneration * Time.deltaTime;
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

	public void EnterPort(Transform dock){
		transform.position = dock.position;
		rb.velocity = Vector3.zero;
	}

	public void LeavePort(Transform dock){
		transform.rotation = dock.rotation;
	}
}
