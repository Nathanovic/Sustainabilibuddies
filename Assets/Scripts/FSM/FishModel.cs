using System.Collections.Generic;
using UnityEngine;

//agent data voor vis, wordt gebruikt door de states om de state te checken
public class FishModel : MonoBehaviour {

	public event DefaultDelegate onCaught;
	public event DefaultDelegate onEscaped;


	public void CatchMe(){
		onCaught ();
	}

	public void Escape(){
		onEscaped ();
	}
}
