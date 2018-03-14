using UnityEngine;

//vangt alle input af mbv InputHandler.cs

public class InputController : ManagedBehaviour {

	private InputController _instance;

	private ShipController ship;
	[SerializeField]private VirtualJoystick joystickScript;

	private InputHandler inputHandler;

	private bool doubleTapped;

	void Awake(){
		_instance = null;
	}

	void Start () {
		ship = GetComponent<ShipController> ();

		if (inputHandler == null)
			SetUpInputHandler ();
	}

	private void SetUpInputHandler(){
		if (Application.platform == RuntimePlatform.Android)
			inputHandler = new TouchInputHandler ();
		else
			inputHandler = new MouseInputHandler();
	}

	public static void AddNotClickable(RectTransform noInputEle){
		if (_instance.inputHandler == null)
			_instance.SetUpInputHandler ();

		_instance.inputHandler.AddNoInputElement (noInputEle);
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
