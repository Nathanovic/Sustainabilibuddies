using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour {

	private Rigidbody rb;
	public float movementSpeed;
	public float rotationSpeed;

	public float maxSpeed = 20f;

	public float moveInput;

	public Vector2 maxVelocity = new Vector2(5,10);

	[Header("Rotation:")]
	public float rotateAcceleration;
	public float rotateSpeed;

	public float maxRotationAcceleration;
	public float maxRotationSpeed;

	public float rotationDegenSpeed;

	void Start () {
		rb = GetComponent<Rigidbody> ();
	}

	void Update () {
		if (!GameManager.instance.CanSail ())
			return;

		float horizontalInput = Input.GetAxis ("Horizontal");
		moveInput = Mathf.Clamp (Input.GetAxis ("Vertical"), 0f, 1f);

		rotateAcceleration = horizontalInput * Time.deltaTime * rotationSpeed;
		rotateAcceleration = Mathf.Clamp (rotateAcceleration, -maxRotationAcceleration, maxRotationAcceleration);
		rotateSpeed += rotateAcceleration;
		float maxRotSpeed = maxRotationSpeed * rb.velocity.magnitude / maxSpeed;
		rotateSpeed = Mathf.Clamp (rotateSpeed, -maxRotSpeed, maxRotSpeed);

		StabilizeFloat (ref rotateSpeed, maxRotationSpeed, rotationDegenSpeed);
	}

	public Vector3 worldForce;
	void FixedUpdate() {
		Vector3 rotEuler = transform.rotation.eulerAngles;
		rotEuler.y += rotateSpeed;
		rb.AddForce (transform.forward * moveInput * movementSpeed);
		Vector3 velocity = rb.velocity;
		velocity = Vector3.ClampMagnitude (velocity, maxSpeed);
		rb.velocity = velocity;
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

	public void EnterPort(Transform dock){
		transform.position = dock.position;
		rb.velocity = Vector3.zero;
	}

	public void LeavePort(Transform dock){
		transform.rotation = dock.rotation;
	}
}
