using UnityEngine;

public class PlayerInput : MonoBehaviour {

	[SerializeField]private string horizontalAxis;
	[SerializeField]private string verticalAxis;
	[SerializeField]private KeyCode fishnetKey;

	[HideInInspector]public float horizontalInput;
	[HideInInspector]public float moveInput;

	void Update () {
		if (!GameManager.instance.CanSail ())
			return;
		
		horizontalInput = Input.GetAxis (horizontalAxis);
		moveInput = Mathf.Clamp (Input.GetAxis (verticalAxis), -0.2f, 1f);
	}

	public bool FishNetKeyUp(){
		return Input.GetKeyUp (fishnetKey);
	}
}
