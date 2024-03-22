namespace GDpsx_Project.addons.GDpsx.Game.Scripts.First_Person.States
{
	public partial class GDpsx_State_FPS_Airborn : GDpsx_State
	{
		public override void Enter()
		{
			base.Enter();
		}
		public override void Exit()
		{
			base.Exit();
		}

		public override void PhysicsUpdate(float delta)
		{
			StateMachine.PlayerMovement.AirTime += delta;

			if (StateMachine.PlayerMovement.IsOnFloor())
			{
				StateMachine.PlayerMovement.AirTime = 0.0f;
				StateMachine.TransitionTo("GDpsx State | Idle");
			}

		}
	}
}
