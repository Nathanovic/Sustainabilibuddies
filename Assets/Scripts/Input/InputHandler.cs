﻿using System.Collections.Generic;
using UnityEngine;

public abstract class InputHandler {
	private List<RectTransform> noJoyElements = new List<RectTransform> ();
	public bool isDragging;

	public void AddNoInputElement(RectTransform njEl){
		noJoyElements.Add (njEl);
	}

	protected bool PointerInNoJoyElement(Vector2 inputPos){
		for (int i = 0; i < noJoyElements.Count; i++) {
			if(RectTransformUtility.RectangleContainsScreenPoint (noJoyElements[i], Input.mousePosition)){
				return true;
			}
		}

		return false;
	}

	public abstract bool PointerDown ();
	public abstract bool PointerUp ();
	public abstract Vector2 PointerPos ();
}

public class MouseInputHandler : InputHandler{
	public override bool PointerDown (){
		if (Input.GetMouseButtonDown (0)) {
			if (!PointerInNoJoyElement (Input.mousePosition)) {
				isDragging = true;
				return true;
			}
		}

		return false;
	}
	public override bool PointerUp (){
		if (Input.GetMouseButtonUp (0)) {
			isDragging = false;
			return true;
		}

		return false;
	}
	public override Vector2 PointerPos (){
		return Input.mousePosition;
	}
}

public class TouchInputHandler : InputHandler{
	private Touch currentTouch;

	public override bool PointerDown (){
		for(int i = 0; i < Input.touchCount; i ++){
			currentTouch = Input.GetTouch(i);
			if (currentTouch.phase == TouchPhase.Began && !PointerInNoJoyElement (currentTouch.position)) {
				isDragging = true;
				return true;
			}
		}

		return false;
	}
	public override bool PointerUp (){
		for(int i = 0; i < Input.touchCount; i ++){
			if (Input.GetTouch (i).phase != TouchPhase.Ended) {
				return false;
			}
		}

		isDragging = false;
		return true;
	}
	public override Vector2 PointerPos (){
		return currentTouch.position;
	}
}