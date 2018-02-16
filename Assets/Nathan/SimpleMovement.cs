using UnityEngine;

public class SimpleMovement : MonoBehaviour {

	public Transform camTransform;
	public VirtualJoystick joystickScript;

	void Update () {
		Vector3 movement = joystickScript.movementVector;
		transform.Translate (movement * Time.deltaTime, Space.World);
		camTransform.Translate (movement * Time.deltaTime, Space.World);
		transform.LookAt (transform.position + movement);
	}
}
