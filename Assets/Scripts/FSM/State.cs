using UnityEngine;

//state voor vis
namespace FishFSM{
	public abstract class State {

		protected FishData myData;
		protected Fish fish;
		protected FiniteStateMachine fsm;

		public void Init(FiniteStateMachine _fsm, Fish _fish, FishData data){
			fsm = _fsm;
			fish = _fish;
			myData = data;
		}

		public abstract void Enter ();
		public abstract Vector3 GetCombinedVector ();
		public abstract void Exit ();
	}

	public class DefaultState : State {

		public override void Enter (){
			
		}

		public override Vector3 GetCombinedVector (){
			throw new System.NotImplementedException ();
		}

		public override void Exit (){
			
		}
	}
}
