using UnityEngine;
using UnityStandardAssets.Vehicles.Car;
using ShipTest;

public class InputController : MonoBehaviour {

	private ShipController ship;
	[SerializeField]private VirtualJoystick joystickScript;

	void Start () {
		ship = GetComponent<ShipController> ();
	}

	void FixedUpdate () {
		float h = 0, v = 0f;

		if (joystickScript.movementVector.sqrMagnitude > 0f) {
			Vector3 inputVector = transform.InverseTransformDirection (joystickScript.movementVector).normalized;
			v = inputVector.magnitude;
			if (inputVector.y < 0f) {
				h = (h < 0) ? -1f : 1f;
				v *= 0.5f;
			}
			h = inputVector.x;
		}

		ship.Move (h, v);//, v, 0f);
	}
}
