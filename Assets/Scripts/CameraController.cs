using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	[SerializeField]private Transform target;
	[SerializeField]private float rotSpeed = 3f;
	private Vector3 camOffset;

	void Start () {
		camOffset = transform.position - target.position;
	}

	void Update () {
		transform.position = target.position + camOffset;
		float rotHorizontal = -Input.GetAxis ("CamHorizontal") * Time.deltaTime;
		rotHorizontal *= rotSpeed;

		transform.RotateAround (target.position, Vector3.up, rotHorizontal);
		transform.LookAt (target.position);
		camOffset = transform.position - target.position;
	}
}
