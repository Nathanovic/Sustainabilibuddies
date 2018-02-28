using UnityEngine;

namespace FishFSM{
	public class FiniteStateMachine {

		public State currentState{get;private set;}

		public FiniteStateMachine (State[] states, Fish f, FishData fData){
			foreach (State s in states) {
				s.Init (this, f, fData);
			}
		}

		public void EnterState(State nextState){
			if (currentState != null) {
				currentState.Exit ();
			}

			currentState = nextState;
			currentState.Enter ();
		}
	}
}
