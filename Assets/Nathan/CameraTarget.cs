using UnityEngine;

public class CameraTarget : MonoBehaviour {

	void Start(){
		Camera.main.GetComponent<CameraController> ().StartTracking (transform);
	}
}
