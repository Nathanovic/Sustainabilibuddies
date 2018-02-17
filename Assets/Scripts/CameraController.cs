using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	private Transform target;
	[SerializeField]private float rotSpeed = 3f;
	private Vector3 relCamPos;

	[SerializeField]private float camSlerpSpeed = 0.5f;

	public void StartTracking (Transform _target) {
		target = _target;
		relCamPos = target.InverseTransformPoint (transform.position);
	}

	void LateUpdate () {
		/*
		transform.position = target.position + camOffset;

		float rotHorizontal = -Input.GetAxis ("CamHorizontal") * Time.deltaTime;
		rotHorizontal *= rotSpeed;

		transform.RotateAround (target.position, Vector3.up, rotHorizontal);

		transform.LookAt (target.position);
		*/

		Vector3 targetPos = target.TransformPoint (relCamPos);
		transform.position = Vector3.Slerp (transform.position, targetPos, camSlerpSpeed * Time.deltaTime);
		transform.LookAt (target.position);
	}
}
