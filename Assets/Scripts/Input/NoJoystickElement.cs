using UnityEngine;

//dit script vraagt aan joystickscript: zorg maar dat er niet op mij een joystick kan verschijnen!
public class NoJoystickElement : MonoBehaviour {

	private VirtualJoystick joystickScript;

	void Start () {
		joystickScript = transform.parent.GetComponentInChildren<VirtualJoystick> ();
		//do stuff with it!
	}

	void Update () {
		
	}
}
