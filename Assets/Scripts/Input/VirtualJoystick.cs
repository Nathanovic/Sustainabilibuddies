using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualJoystick : ManagedBehaviour {

	private static VirtualJoystick _instance;

	private RectTransform background;
	private RectTransform joystick;
	private CanvasGroup cvg;
	private CanvasCounter disableCounter;
	[SerializeField]private float cvgDisableTime = 0.8f;
	[SerializeField]private float fadeOutTime = 0.5f;
	private bool disabledCVG;

	private Vector3 inputVector;
	public Vector3 movementVector;

	public Transform camTransform;
	public DirectionPointer directionPointer;
	public float maxSecondTouchDist = 90f;

	public bool useDirectionPointer;

	private bool joystickActive = true;

	public ShipInteractions interactionScript;

	protected override void Awake(){
		base.Awake ();
		_instance = this;
	}

	private void Start(){
		background = GetComponent<RectTransform> ();
		joystick = transform.GetChild (0).GetComponent<RectTransform> ();

		cvg = GetComponent<CanvasGroup> ();
		disableCounter = new CanvasCounter (cvg, cvgDisableTime, fadeOutTime);
		disableCounter.onCount += DisableCVG;
		//DisableCVG ();

		//avoid the joystick from popping up when the boat should not be controlled
		GameManager.instance.onBoatControlsDisabled += BoatControlsDisabled;
		GameManager.instance.onBoatControlsEnabled += BoatControlsEnabled;
	}

	public override void ManagedUpdate(){
		bool isDragging = inputHandler.isDragging;
		bool pointerUp = false;//make sure we always update inputHandler.isDragging
		if(isDragging){
			pointerUp = inputHandler.PointerUp();
			print ("pointer up: " + pointerUp);
		}

		if (!joystickActive)
			return;

		if (!isDragging) {
			if(inputHandler.PointerDown())
				PointerDown ();
		} else {
			if (!pointerUp)
				DragPointer ();
			else
				PointerUp ();
		} 
	}

	private void PointerDown(){
		Vector2 localPoint = new Vector2 ();
		if (disabledCVG) {
			RectTransformUtility.ScreenPointToLocalPointInRectangle (background, Input.mousePosition, null, out localPoint);
			Vector2 normalizedPoint = background.anchoredPosition + localPoint;
			//background.anchoredPosition = normalizedPoint;

			disabledCVG = false;
		}
		else {
			RectTransformUtility.ScreenPointToLocalPointInRectangle (background, Input.mousePosition, null, out localPoint);
			if(localPoint.magnitude > maxSecondTouchDist){
				Vector2 normalizedPoint = background.anchoredPosition + localPoint;
				//background.anchoredPosition = normalizedPoint;				
			}
		}

		if (useDirectionPointer) {
			directionPointer.Enable ();
		}

		EnableCVG ();
	}

	private void DragPointer (){
		Vector2 pos = new Vector2 ();
		if(RectTransformUtility.ScreenPointToLocalPointInRectangle(background,
			Input.mousePosition, null, out pos))
		{
			pos.x = pos.x / background.sizeDelta.x;
			pos.y = pos.y / background.sizeDelta.y;

			inputVector = new Vector3 (pos.x * 2f, 0f, pos.y * 2f);
			inputVector = (inputVector.magnitude > 1f) ? inputVector.normalized : inputVector;

			joystick.anchoredPosition = new Vector3 (inputVector.x * background.sizeDelta.x * 0.5f, inputVector.z * background.sizeDelta.y * 0.5f, 0f);

			movementVector = inputVector;
			movementVector = camTransform.TransformDirection (movementVector);
			directionPointer.Rotate (movementVector);
		}
	}

	private void PointerUp (){
		//directionPointer.Disable ();
		inputVector = movementVector = Vector3.zero;
		joystick.anchoredPosition = inputVector;
		Debug.Log ("pointer up!");
		//disableCounter.StartCounter ();
	}

	private void DisableCVG(){
		disableCounter.StopCounter ();
		disabledCVG = true;
		cvg.Deactivate ();
	}

	private void EnableCVG(){
		cvg.Activate ();
		disableCounter.StopCounter ();
		DragPointer ();		
	}

	private void BoatControlsDisabled(){
		PointerUp ();
		DisableCVG ();
		disableCounter.StartCounter ();
		joystickActive = false;
	}

	private void BoatControlsEnabled(){
		joystickActive = true;
		EnableCVG ();
	}
}