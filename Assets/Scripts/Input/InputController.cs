using UnityEngine;

public class InputController : ManagedBehaviour {

	private ShipController ship;
	[SerializeField]private VirtualJoystick joystickScript;

	void Start () {
		ship = GetComponent<ShipController> ();
	}

	public override void ManagedFixedUpdate () {
		float h = 0, v = 0f;

		if (joystickScript.movementVector.sqrMagnitude > 0f) {
			Vector3 inputVector = transform.InverseTransformDirection (joystickScript.movementVector);

			v = inputVector.magnitude;
			h = inputVector.x;
			if (inputVector.z < 0f) {
				if (h < 0f) {
					h = -1f;
				}
				else {
					h = 1f;
				}
			}
		}

		ship.Move (h, v);//, v, 0f);
	}
}
