using UnityEngine;

//state voor vis
namespace FishFSM{
	public abstract class State {

		protected FishModel myData;
		protected Fish fish;
		protected FiniteStateMachine fsm;

		public void Init(FiniteStateMachine _fsm, Fish _fish, FishModel data){
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
			return fish.Alignment() + fish.Cohesion() + fish.Separation() + fish.GoalSteering();
		}

		public override void Exit (){
			
		}
	}
}
