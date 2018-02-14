using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour {

	private Camera mainCam;
	private RectTransform background;
	private RectTransform joystick;
	private Vector3 inputVector;

	void Start(){
		background = GetComponent<RectTransform> ();
		joystick = transform.GetChild (0).GetComponent<RectTransform> ();
		mainCam = Camera.main;
	}

	void Update(){
		if (Input.GetMouseButtonDown (0)) {
			StartDrag ();
		} else if (Input.GetMouseButton (0)) {
			DragPointer ();
		} else if (Input.GetMouseButtonUp (0)) {
			PointerUp ();
		}
	}

	void StartDrag(){
		Debug.Log (background.anchoredPosition);
		Debug.Log ("pointer down");
		Vector2 localPoint = new Vector2 ();
		RectTransformUtility.ScreenPointToLocalPointInRectangle (background, Input.mousePosition, mainCam, out localPoint);
		Debug.Log (localPoint);
		Vector2 normalizedPoint = background.anchoredPosition + localPoint;
		background.anchoredPosition = normalizedPoint;
		DragPointer ();
	}

	void DragPointer (){
		Debug.Log ("dragging");
		Vector2 pos = new Vector2 ();
		if(RectTransformUtility.ScreenPointToLocalPointInRectangle(background,
			Input.mousePosition, mainCam, out pos))
		{
			pos.x = pos.x / background.sizeDelta.x;
			pos.y = pos.y / background.sizeDelta.y;

			inputVector = new Vector3 (pos.x * 2f, 0f, pos.y * 2f);
			inputVector = (inputVector.magnitude > 1f) ? inputVector.normalized : inputVector;

			joystick.anchoredPosition = new Vector3 (inputVector.x * background.sizeDelta.x * 0.5f, inputVector.z * background.sizeDelta.y * 0.5f, 0f);
		}
	}

	public void PointerUp (){
		Debug.Log ("pointer up");
		inputVector = new Vector3 ();
		joystick.anchoredPosition = inputVector;
	}
}
