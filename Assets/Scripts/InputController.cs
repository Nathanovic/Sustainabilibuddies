using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;
using ShipTest;

public class InputController : MonoBehaviour {

	private ShipController ship;
	[SerializeField]private VirtualJoystick joystickScript;

	public UnityEngine.UI.Text gyroText;

	void Start () {
		ship = GetComponent<ShipController> ();
		#if UNITY_ANDROID
		Input.gyro.enabled = true;
		#endif
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
		else {
			h = Input.gyro.attitude.eulerAngles.z / 90f;
			v = Input.GetMouseButtonDown (0) ? 1f : 0f;
		}

		gyroText.text = Input.gyro.attitude.eulerAngles.ToString();

		ship.Move (h, v);//, v, 0f);
	}
}
