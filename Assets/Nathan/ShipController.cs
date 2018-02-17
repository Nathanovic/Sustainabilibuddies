using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShipTest{
	public class ShipController : MonoBehaviour {

		private Rigidbody rb;
		private Transform visual;
		private Vector3 visualDefaultEuler;

		private float currentSpeed;
		public float maxForwardSpeed;
		public float backwardsSpeedFactor;
		public float accSpeed;
		[Range(0f,7f)]public float speedDegenTime = 4f;

		private float currentRotateSpeed;
		public float maxRotationSpeed;
		public float accRotation;
		[Range(0f,3f)]public float rotDegenTime = 1f;

		[Range(0f,1f)]public float rotSpeedRelation = 0.5f;

		void Start(){
			rb = GetComponent<Rigidbody> ();
			visual = transform.GetChild (0);
			visualDefaultEuler = visual.rotation.eulerAngles;
		}

		public void Move(float horizontal, float vertical){
			//set rotate speed:
			if(horizontal != 0){
				currentRotateSpeed += horizontal * accRotation;
				float maxRotSpeed = 0.5f * maxRotationSpeed + maxRotationSpeed * (currentSpeed / (maxForwardSpeed * 2));
				currentRotateSpeed = Mathf.Clamp (currentRotateSpeed, -maxRotSpeed, maxRotSpeed);
			}
			if(Mathf.Abs(horizontal) < 0.15f){
				currentRotateSpeed = ResetValueOverTime (currentRotateSpeed, maxRotationSpeed, rotDegenTime);
			}

			//rotate:
			Vector3 myEuler = rb.rotation.eulerAngles;
			float myRotation = currentRotateSpeed * rotSpeedRelation * currentSpeed;
			myEuler.y += myRotation;
			rb.MoveRotation (Quaternion.Euler (myEuler));

			//rotate visual:
			visual.transform.localEulerAngles = visualDefaultEuler + new Vector3 (currentSpeed * 0.4f, 0f, 0f);

			//set movement speed:
			if (vertical != 0) {
				float acceleration = vertical * accSpeed;
				currentSpeed += acceleration;
				currentSpeed = Mathf.Clamp (currentSpeed, -maxForwardSpeed, maxForwardSpeed);
			}
			else {
				currentSpeed = ResetValueOverTime (currentSpeed, maxForwardSpeed, speedDegenTime);
			}

			//move:
			rb.velocity = transform.forward * currentSpeed;
		}

		float ResetValueOverTime(float currentValue, float maxValue, float degenTime){
			if (currentValue == 0f)
				return 0f;
			
			float positive = (currentValue > 0f) ? 1f : -1f;
			float t = 1f - (Mathf.Abs(currentValue) / maxValue) + Time.fixedDeltaTime * (1f/degenTime);
			return Mathf.Lerp (maxValue, 0f, t) * positive;
		}
	}
}
