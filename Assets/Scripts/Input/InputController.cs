using UnityEngine;

public class InputController : MonoBehaviour {

	private ShipController ship;
	[SerializeField]private VirtualJoystick joystickScript;

	void Start () {
		ship = GetComponent<ShipController> ();
	}

	void FixedUpdate () {
		float h = 0, v = 0f;

		Debug.Log ("sqr input magn: " + joystickScript.movementVector.sqrMagnitude.ToString ());
		if (joystickScript.movementVector.sqrMagnitude > 0f) {
			Vector3 inputVector = transform.InverseTransformDirection (joystickScript.movementVector);
			v = inputVector.magnitude;
			h = inputVector.x;
		}

		ship.Move (h, v);//, v, 0f);
	}
}
