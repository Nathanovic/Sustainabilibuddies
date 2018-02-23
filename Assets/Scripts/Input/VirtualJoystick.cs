using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IRanByGameManager {

	private static VirtualJoystick _instance;

	private RectTransform background;
	private RectTransform joystick;
	private CanvasGroup cvg;
	private CanvasCounter disableCounter;
	[SerializeField]private float cvgDisableTime = 0.8f;
	[SerializeField]private float fadeOutTime = 0.5f;
	private bool disabledCVG;

	private Vector3 inputVector;
	[HideInInspector]public Vector3 movementVector;

	public Transform camTransform;
	public DirectionPointer directionPointer;
	public float maxSecondTouchDist = 90f;

	private InputHandler inputHandler;

	void Awake(){
		_instance = this;
	}

	void Start(){
		GameManager.instance.InitGameLoopDependable (this);
		background = GetComponent<RectTransform> ();
		joystick = transform.GetChild (0).GetComponent<RectTransform> ();

		cvg = GetComponent<CanvasGroup> ();
		disableCounter = new CanvasCounter (cvg, cvgDisableTime, fadeOutTime);
		disableCounter.onCount += DisableCVG;
		DisableCVG ();

		if (inputHandler == null)
			SetUpInputHandler ();
	}

	void SetUpInputHandler(){
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

	public void ManagedUpdate(){
		Debug.Log (inputHandler.GetType ().ToString () + " drags: " + inputHandler.isDragging);
		if (!inputHandler.isDragging) {
			if(inputHandler.PointerDown())
				PointerDown ();
		} else if (inputHandler.isDragging) {
			if (!inputHandler.PointerUp ())
				DragPointer ();
			else
				PointerUp ();
		} 
	}

	void PointerDown(){
		Vector2 localPoint = new Vector2 ();
		if (disabledCVG) {
			RectTransformUtility.ScreenPointToLocalPointInRectangle (background, Input.mousePosition, null, out localPoint);
			Vector2 normalizedPoint = background.anchoredPosition + localPoint;
			background.anchoredPosition = normalizedPoint;

			disabledCVG = false;
		}
		else {
			RectTransformUtility.ScreenPointToLocalPointInRectangle (background, Input.mousePosition, null, out localPoint);
			if(localPoint.magnitude > maxSecondTouchDist){
				Vector2 normalizedPoint = background.anchoredPosition + localPoint;
				background.anchoredPosition = normalizedPoint;				
			}
		}

		directionPointer.Enable ();
		cvg.Activate ();
		disableCounter.StopCounter ();
		DragPointer ();
	}

	void DragPointer (){
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

	void PointerUp (){
		directionPointer.Disable ();
		movementVector = new Vector3 ();
		joystick.anchoredPosition = inputVector;
		disableCounter.StartCounter ();
	}

	void DisableCVG(){
		disableCounter.StopCounter ();
		disabledCVG = true;
		cvg.Deactivate ();
	}
}