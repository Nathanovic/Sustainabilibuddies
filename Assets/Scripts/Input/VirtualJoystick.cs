using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour {

	private Camera mainCam;
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
	public Transform dirPointerOrigin;
	public float dirPointerDist = 1f;
	public Transform directionPointer;

	void Start(){
		background = GetComponent<RectTransform> ();
		joystick = transform.GetChild (0).GetComponent<RectTransform> ();
		mainCam = null;

		cvg = GetComponent<CanvasGroup> ();
		disableCounter = new CanvasCounter (cvg, cvgDisableTime, fadeOutTime);
		disableCounter.onCount += DisableCVG;
		DisableCVG ();
	}

	void Update(){
		if (Input.GetMouseButtonDown (0)) {
			PointerDown ();
		} else if (Input.GetMouseButton (0)) {
			DragPointer ();
		} else if (Input.GetMouseButtonUp (0)) {
			PointerUp ();
		}
	}

	void PointerDown(){
		Vector2 localPoint = new Vector2 ();
		if (disabledCVG) {
			RectTransformUtility.ScreenPointToLocalPointInRectangle (background, Input.mousePosition, mainCam, out localPoint);
			Vector2 normalizedPoint = background.anchoredPosition + localPoint;
			background.anchoredPosition = normalizedPoint;

			disabledCVG = false;
		}
		else {
			if (!RectTransformUtility.ScreenPointToLocalPointInRectangle (background, Input.mousePosition, mainCam, out localPoint)) {
				Debug.Log ("outside of rect");//not working, yet
				Vector2 normalizedPoint = background.anchoredPosition + localPoint;
				background.anchoredPosition = normalizedPoint;
			}
		}

		cvg.Activate ();
		disableCounter.StopCounter ();
		DragPointer ();
	}

	void DragPointer (){
		Vector2 pos = new Vector2 ();
		if(RectTransformUtility.ScreenPointToLocalPointInRectangle(background,
			Input.mousePosition, mainCam, out pos))
		{
			pos.x = pos.x / background.sizeDelta.x;
			pos.y = pos.y / background.sizeDelta.y;

			inputVector = new Vector3 (pos.x * 2f, 0f, pos.y * 2f);
			inputVector = (inputVector.magnitude > 1f) ? inputVector.normalized : inputVector;

			joystick.anchoredPosition = new Vector3 (inputVector.x * background.sizeDelta.x * 0.5f, inputVector.z * background.sizeDelta.y * 0.5f, 0f);

			movementVector = inputVector;
			movementVector = camTransform.TransformDirection (movementVector);
			movementVector.Normalize ();
			Debug.DrawRay (dirPointerOrigin.position, movementVector, Color.blue);

			if (movementVector.sqrMagnitude != 0f) {
				directionPointer.rotation = Quaternion.LookRotation (movementVector);
			}
			directionPointer.position = dirPointerOrigin.position + movementVector * dirPointerDist;
		}
	}

	public void PointerUp (){
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
